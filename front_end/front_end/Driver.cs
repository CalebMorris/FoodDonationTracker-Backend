using System;

namespace back_end
{
	public class Driver : User
	{	
		protected string status;
		Donation pickup;
		Receiver dropoff;
		
		public Driver (): base("","","Driver",null) {
			pickup = null;
			dropoff = null;
		}
		public Driver( string uname, string upass, GPS loc ) 
				: base(uname, upass, "Driver", loc ) {
			pickup = null;
			dropoff = null;
		}
		
		public string getStatus() { return status; }
		public Donation getPickup() { return pickup; }
		public Receiver getDropoff() { return dropoff; }
		public string updateStatus( string stat ) { 
			status = stat; 
			return status; 
		}
		public void assignPickup( Donation donation ) {
			if( donation != default(Donation) ) {
				status = "assigned";
				pickup = donation;
			}
			else {
				// throw error				
			}
		}
		public void assignDropoff( Receiver reciever ) {
			if( reciever != default(Receiver) ) {
				dropoff = reciever;
			}
			else {
				// throw error				
			}
		}
		
	}
}
