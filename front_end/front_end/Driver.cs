using System;

namespace back_end
{
	public class Driver : User
	{
		public enum Status {
			available, unavailable, 
			assigned, enroute, 
			pickedup, droppedoff,
			unable_to_deliver
		};
		
		protected Status status;
		Donor pickup;
		
		public Driver (): base("","",null) { }
		public Driver( string uname, string upass, GPS loc ) 
				: base(uname, upass, loc ) {
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

