using System;

namespace back_end
{
	public class Driver : User
	{	
		protected Status status;
		Donor pickup;
		
		public Driver (): base("","","Driver",null) { }
		public Driver( string uname, string upass, GPS loc ) 
				: base(uname, upass, "Driver", loc ) {
		}
		
		public Status getStatus() { return status; }
		public Donor getPickup() { return pickup; }
		public Status updateStatus( Status stat ) { 
			status = stat; 
			return status; 
		}
		public void assignPickup( Donor donor ) {
			if( donor != null ) {
				status = Status.assigned;
				pickup = donor;
			}
			else {
				//TODO throw error				
			}
		}
	}
}

