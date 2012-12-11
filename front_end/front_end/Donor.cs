using System;

namespace back_end
{
	public class Donor : User
	{
		public Donor ():base("","",null) { }
		public Donor( string uname, string upass, GPS loc ) 
				: base(uname, upass, loc ) {
		}
	}
}
