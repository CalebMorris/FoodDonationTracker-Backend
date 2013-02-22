using System;

namespace back_end
{
	public class Driver : User
	{	
		protected string status;
		Donor pickup;
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
		public Donor getPickup() { return pickup; }
		public Receiver getDropoff() { return dropoff; }
		public string updateStatus( string stat ) { 
			status = stat; 
			return status; 
		}
		public void assignPickup( Donor donor ) {
			if( donor != default(Donor) ) {
				status = "assigned";
				pickup = donor;
			}
			else {
				//TODO throw error				
			}
		}
		public void assignDropoff( Receiver reciever ) {
			if( reciever != default(Receiver) ) {
				dropoff = reciever;
			}
			else {
				//TODO throw error				
			}
		}
		
		public void getPickup( Donor[] donors ) {
			//if( donors.Length <= 0  )
			//	return null;
			int closest = 0;
			double D = this.location.difference(donors[0].location);
			
			for( int i = 1; i < donors.Length; ++i ) {
				double tmp = this.location.difference(donors[i].location);
				if( tmp < D ) {
					closest = i;
					D = tmp;
				}
			}
			this.pickup = donors[closest];
		}
	}
}
