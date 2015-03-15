using System.Collections.Generic;
using UnityEngine;

public class Utils {
	/* Shuffles the full list */
	public static void shuffle<T>(IList<T> list) {
		shuffle (list, list.Count);
	}

	/* Shuffles the first numberOfItems of the list
	 */
	public static void shuffle<T>(IList<T> list, int numberOfItems)
	{  
		System.Random rnd = new System.Random ();

		int n = 0;
		 
		while (n < list.Count && numberOfItems > 0) {
			// Random.next() is exclusive on the max
			int k = rnd.Next(n, list.Count);

			T value = list[k];
			list[k] = list[n];
			list[n] = value;

			n++;
			numberOfItems--;
		}
	}
}