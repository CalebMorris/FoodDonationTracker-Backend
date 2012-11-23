using System;

namespace back_end
{
	public class Driver : User
	{
		enum Status {
			available, unavailable, 
			assigned, enroute, 
			pickedup, droppedoff,
			unable_to_deliver
		};
		
		protected Status status;
		Donor pickup;
		
		public Driver () { }
		
		public Status getStatus() { return status; }
		public Donor getPickup() { return pickup; }
		public void updateStatus( Status stat ) { status = stat; }
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

