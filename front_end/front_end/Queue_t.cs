using System;
using System.Collections.Generic;

namespace back_end
{
	public class Pair_t<T> {
		int val;
		T data;
		public Pair_t( int val, T data ) {
			this.val = val;
			this.data = data;
		}
		public int compareTo( Pair_t<T> another ) {
			//-1,0,1 <,==,>
			if( this.val < another.val ) {
				return -1;				
			}
			else if( this.val > another.val ) {
				return 1;
			}
			else {
				return 0;				
			}
		}
		public T getData() { return this.data; }
		public int getVal() { return this.val; }
	}
	
	public class Queue_t<T>
		// min queue
	{
		List<Pair_t<T>> nodes;
		
	    public Queue_t() {
			nodes = new List<Pair_t<T>>();
		}		
		
		public void insert( Pair_t<T> pair ) {
			nodes.Add(pair);
			//Init heapify
			int current = nodes.Count-1;
			while( current > 0 && nodes[current].compareTo(nodes[(current-1)/2]) < 0 ) {
				swap(current, (current-1)/2);
				current = (current-1)/2;
			}
		}
		
		public Pair_t<T> pop() {
			if( nodes.Count < 1 ) {
				return default(Pair_t<T>);				
			}
			Pair_t<T> result = nodes[0];
			//Console.WriteLine(result.getData());
			nodes[0] = nodes[nodes.Count-1];
			nodes.RemoveAt(nodes.Count-1);
			min_heapify(0);
			
			return result;
		}
		
		int swap(int pos1, int pos2) {
//			Console.WriteLine("swap("+pos1+","+pos2+")");
			Pair_t<T> tmp = nodes[pos1];
			nodes[pos1] = nodes[pos2];
			nodes[pos2] = tmp;
			return 1;
		}
		
		void min_heapify( int position ) {
//			Console.WriteLine("min_heap("+position+")");
			int left = 2*position+1,
				right = 2*position+2,
				largest = position;
			if( left < nodes.Count && nodes[left].compareTo(nodes[largest]) < 0 ) {
				largest = left;
			}
			if( right < nodes.Count && nodes[right].compareTo(nodes[largest]) < 0 ) {
				largest = right;
			}
			if( largest != position ) {
				//swap largest and position
				int er = swap(largest, position);
				if( er < 0 ) {
					Console.WriteLine("swap error");
				}
//				Console.WriteLine("Largest:"+largest);
				min_heapify( largest );
			}
		}
		
		public bool is_empty() {
			return nodes.Count < 1;			
		}
		
		public string toString() {
			if( nodes.Count < 1 ) {
				return "Queue is empty.";
			}
			string result = "Queue: \n";
			for (int i = 0; i < nodes.Count; i++) {
				result += nodes[i].getVal() + " " + nodes[i].getData() + "\n";
			}
			return result;
		}
	}
}

