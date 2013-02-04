using System;
using System.Collections.Generic;
using back_end;

namespace front_end
{
	public class Queue
	{
		uint size;
		SortedDictionary<int, Donor> q;
		
		public Queue ()
		{
			/* A simple priority queue sorted by 
			 * the TTL of the foodstuffs
			 * */	
			size = 0;
		}
		public bool isEmpty() {
			if ( this.size == 0 ) {
				return true;
			}				
			else return false;
		}
	}
}

