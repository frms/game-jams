using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PriorityQueue<T> {
	private List<T> list;
	private IComparer<T> comparer;

	public int Count
	{
		get {
			return list.Count;
		}
	}

	public PriorityQueue(IComparer<T> comparer) {
		list = new List<T> ();
		this.comparer = comparer;
	}

	public void queue(T t) {
		int pos = 0;

		for (int i = list.Count-1; i >= 0; i--) {
			if(comparer.Compare(t, list[i]) <= 0) {
				pos = i+1;
				break;
			}
		}

		list.Insert (pos, t);
	}

	public T dequeue() {
		T ret = default(T);

		if(list.Count > 0) {
			ret = list [list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		return ret;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder ();
		sb.Append ("Count: ").Append (list.Count);

		for (int i = 0; i < list.Count; i++) {
			sb.Append("\n ");

			if(typeof(T) == typeof(int[])) {
				sb.Append("[");

				int[] arr = list[i] as int[];

				for(int j = 0; j < arr.Length; j++) {
					sb.Append(arr[j]);
					if(j < arr.Length-1) {
						sb.Append(", ");
					}
				}

				sb.Append("]");
			} else {
				sb.Append(list[i]);
			}
		}
		return sb.ToString();
	}
}
