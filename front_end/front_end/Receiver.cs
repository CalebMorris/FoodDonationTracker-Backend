using System;

namespace back_end
{
	public class Receiver: User
	{
		public Receiver():base("","",null) {}
		public Receiver ( string uname, string pass, GPS loc )
			:base(uname, pass, loc)
		{
			
		}
	}
}

