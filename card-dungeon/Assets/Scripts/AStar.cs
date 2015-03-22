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
			return (int)(nodeArray[a[0], a[1]].estimatedTotalCost - nodeArray[b[0], b[1]].estimatedTotalCost);
		}
	}


	public List<int[]> findPathAStar(MapData graph, int[] start, int[] end) {
		/* Using manhattan distance since I assume this graph is a 4 direction grid.
		 * Make AStar more customizable with more distance heuristics (like Euclidean) */
		ManhattanDistHeuristic heuristic = new ManhattanDistHeuristic (end);
		
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
			
			/* If its the goal node then terminate */
			if(equals(currentNode, end)) {
				break;
			}
			
			List<int[]> connections = graph.getConnectedNodes(currentNode);

			for(var i = 0; i < connections.Count; i++) {
				int[] endNode = connections[i];
				float endNodeCost = current.costSoFar + 1;
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
			List<int[]> path = new List<int[]>();
			path.Add(currentNode);

			/* Work back along the path, accumulating connections */
			while(!equals(current.node, start)) {
				path.Add(current.lastNode);
				current = nodeArray[current.lastNode[0], current.lastNode[1]];
			}
			
			/* Reverse the path so the connections are from start to finish */
			path.Reverse();
			
			return path;
		}
	}

	private static bool equals(int[] a, int[] b) {
		return (a == null && b == null) || (a != null && b != null && a[0] == b[0] && a[1] == b[1]);
	}

}
