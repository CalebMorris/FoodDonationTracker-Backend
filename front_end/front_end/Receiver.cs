using System;

namespace back_end
{
	public class Receiver: User
	{
		public Receiver():base("","","Receiver",null) {}
		public Receiver ( string uname, string pass, GPS loc )
			:base(uname, pass, "Receiver", loc)
		{
			
		}
	}
}

