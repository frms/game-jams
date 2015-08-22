using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

	class ManhattanDistHeuristic {
		private int[] goal;

		public ManhattanDistHeuristic(int[] goal) {
			this.goal = goal;
		}

		public float estimate(int[] node) {
			return Mathf.Abs(node [0] - goal [0]) + Mathf.Abs(node [1] - goal [1]);
		}
	}

	class DiagonalDistHeuristic {
		private int[] goal;
		private static float D = 1;
		private static float D2 = Mathf.Sqrt (2) * D;


		public DiagonalDistHeuristic(int[] goal) {
			this.goal = goal;
		}
		
		public float estimate(int[] node) {
			int dx = Mathf.Abs (node [0] - goal [0]);
			int dy = Mathf.Abs (node [1] - goal [1]);
			return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min (dx, dy);
		}
	}

	class EuclideanDistHeuristic {
		private int[] goal;
		
		public EuclideanDistHeuristic(int[] goal) {
			this.goal = goal;
		}
		
		public float estimate(int[] node) {
			int dx = Mathf.Abs (node [0] - goal [0]);
			int dy = Mathf.Abs (node [1] - goal [1]);
			return Mathf.Sqrt (dx * dx + dy * dy);
		}
	}


	enum Category { open, closed };

	class Record {
		public int[] node;
		public int[] lastNode;
		public float costSoFar;
		public float estimatedTotalCost;
		public Category category;
	}

	class NodeComparer : IComparer<int[]> {
		public Record[,] nodeArray;

		public NodeComparer(Record[,] nodeArray) {
			this.nodeArray = nodeArray;
		}

		public int Compare(int[] a, int[] b) {
			float f = nodeArray[a[0], a[1]].estimatedTotalCost - nodeArray[b[0], b[1]].estimatedTotalCost;
			if (f > 0) {
				return 1;
			} else if (f == 0) {
				return 0;
			} else {
				return -1;
			}
		}
	}


	public static LinePath findPath(MapData graph, Vector3 startPos, Vector3 endPos, GameObj target) {
		int[] start = graph.worldToMapPoint(startPos);
		int[] end = graph.worldToMapPoint(endPos);

		/* Using diagonal distance since I assume this graph is a 8 direction grid.
		 * Make AStar more customizable with more distance heuristics (like Euclidean) */
		EuclideanDistHeuristic heuristic = new EuclideanDistHeuristic (end);
		
		/* Create the record for the start node */
		Record startRecord = new Record ();
		startRecord.node = start;
		startRecord.lastNode = null;
		startRecord.costSoFar = 0;
		startRecord.estimatedTotalCost = heuristic.estimate(start);
		startRecord.category = Category.open;
		
		/* Initialize the node array and open list */
		Record[,] nodeArray = new Record[graph.width, graph.height];
		nodeArray[startRecord.node[0], startRecord.node[1]] = startRecord;
		
		NodeComparer comparer = new NodeComparer (nodeArray);
		PriorityQueue<int[]> open = new PriorityQueue<int[]>(comparer);
		open.queue(start);
		
		int[] currentNode = null;
		Record current = null;
		
		/* Iterate through process each node in open */
		while(open.Count > 0) {
			
			/* Find the smallest element in the open list */
			currentNode = open.dequeue();
			
			current = nodeArray[currentNode[0], currentNode[1]];
			
			//console.log(currentNode);
			
			/* If its the goal node or part of the target object then terminate */
			if(equals(currentNode, end)) {
				break;
			}

			/* If its part of the target object then terminate (and change the end node to be equal to the current node) */
			if(target != null && graph.objs [currentNode [0], currentNode [1]] == target) {
				end[0] = currentNode[0];
				end[1] = currentNode[1];
				break;
			}
			
			List<Connection> connections = graph.getConnectedNodes(currentNode, target);

			for(var i = 0; i < connections.Count; i++) {
				int[] endNode = connections[i].toNode;
				float endNodeCost = current.costSoFar + connections[i].cost;
				float endNodeHeuristic;
				
				/* Try and get the node record from the array */
				Record endNodeRecord = nodeArray[endNode[0], endNode[1]];
				
				/* Assume we add the endNodeRecord to the open list priority queue */
				bool addToOpenList = true;
				
				/* If an endNodeRecord is present then it is either closed or open */
				if(endNodeRecord != null) {
					
					/* If the endNodeRecord has a lower cost than the current connection 
					 * then continue on to the next connection */
					if(endNodeRecord.costSoFar <= endNodeCost) {
						continue;
					}
					
					/* We can use the node record's old cost values to calculate
					 * the heuristic without having to call the heuristic function */
					endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
					
					/* If the existing node record is already on the open list then we do not need to add it */
					if(endNodeRecord.category == Category.open) {
						addToOpenList = false;
					}
				}
				/* else we know we have an unvisited node so make a record for it and
				 * add it to the node array */
				else {
					endNodeRecord = new Record();
					endNodeRecord.node = endNode;
					
					/* We'll need to calculate the heuristic value using the function,
					 * since we don't have an existing record to use */
					endNodeHeuristic = heuristic.estimate(endNode);
					
					/* Add the new node record to the node array */
					nodeArray[endNode[0], endNode[1]] = endNodeRecord;
				}
				
				/* If we reach here than we either have a new node record which needs to be
				 * assigned a cost and connection or we have an existing node record from the 
				 * open or closed list whose cost and connection need to be updated since our current 
				 * connection has a lower cost. */
				endNodeRecord.costSoFar = endNodeCost;
				endNodeRecord.lastNode = currentNode;
				endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
				/* Make sure it has a category of open (redundant if the node was already open
				 * but important for closed/unvisted nodes) */
				endNodeRecord.category = Category.open;	
				
				/* If the open list doesn't already contain this record then add it to the list */
				if(addToOpenList) {
					open.queue(endNode);
				}
			}
			
			/* We've finished looking at the connections for the current node, so set the
			 * node record to closed */
			current.category = Category.closed;
		}
		
		/* We're here if we have either found the goal or if we have no more nodes to search */
		if(!equals(currentNode, end)) {
			/* We must have run out of nodes before finding the goal */
			return null;
		} else {
			List<Vector3> path = new List<Vector3>();

			path.Add(graph.mapToWorldPoint(currentNode[0], currentNode[1]));

			Vector3 lastDir = new Vector3();

			/* Work back along the path, accumulating connections */
			while(!equals(current.node, start)) {
				Vector3 nextNode = graph.mapToWorldPoint(current.lastNode[0], current.lastNode[1]);

				Vector3 currentDir = nextNode - path[path.Count-1];

				if(path.Count >= 2 && currentDir == lastDir) {
					path.RemoveAt(path.Count-1);
				}

				path.Add(nextNode);
				current = nodeArray[current.lastNode[0], current.lastNode[1]];

				lastDir = currentDir;
			}
			
			/* Reverse the path so the connections are from start to finish */
			path.Reverse();

//			smoothPath (target, path);

			return new LinePath(path.ToArray());
		}
	}

	private static bool equals(int[] a, int[] b) {
		return (a == null && b == null) || (a != null && b != null && a[0] == b[0] && a[1] == b[1]);
	}

//	private static void smoothPath (GameObj target, List<Vector3> path)
//	{
//		bool saveSetting = Physics2D.raycastsStartInColliders;
//		Physics2D.raycastsStartInColliders = false;
//
//		int originIndex = 0;
//
//		for (int i = 2; i < path.Count; i++) {
//			Vector3 dir = path [i] - path [originIndex];
//			float dist = dir.magnitude;
//
//			RaycastHit2D hit = Physics2D.Raycast (path [originIndex], dir, dist, GameManager.defaultMask);
//			if (hit.transform == null || (target != null && hit.transform == target.transform)) {
//				path.RemoveAt (i - 1);
//				i--;
//			}
//			else {
//				originIndex = i - 1;
//			}
//		}
//
//		Physics2D.raycastsStartInColliders = saveSetting;
//	}

}
