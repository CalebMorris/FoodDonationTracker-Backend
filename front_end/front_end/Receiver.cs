using System;

namespace back_end
{
	public class Receiver: User
	{
		public string contactName;
		public string contactPhone;
		public string extraDetails;
		
		public Receiver():base("","","Receiver",null) {}
		public Receiver ( string uname, string pass, GPS loc,
		                  string contactName, string contactPhone,
		                  string extraDetails)
			:base(uname, pass, "Receiver", loc)
		{
			this.contactName = contactName;
			this.contactPhone = contactPhone;
			this.extraDetails = extraDetails;
		}
	}
}

