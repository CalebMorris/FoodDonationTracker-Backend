using System;

namespace back_end
{		
	public class Donor : User
	{
		public Donation donation;
		public string status;
		
		public Donor ():base("","","Donor",null) {
			status = "empty";
		}
		public Donor( string uname, string upass, GPS loc, int timetolive ) 
				: base(uname, upass, "Donor", loc ) {
			status = "empty";
		}
		
		public Receiver findBestDropOff( Receiver[] receivers ) {
			if( receivers.Length == 0 ) {
				return default(Receiver);
			}
			int best = 0;
			double currentDist = this.location.difference(receivers[0].location);
			for( int i = 0; i < receivers.Length; ++i ) {
				double tmp = this.location.difference(receivers[i].location);
				if( tmp < currentDist ) {
					best = i;
					currentDist = tmp;
				}
			}
			return receivers[best];
		}
		
		public Driver findBestDriver( Driver[] drivers ) {
			if( drivers.Length == 0 ) {
				return default(Driver);
			}
			int best = -1;
			double currentDist = Double.MaxValue;
			for( int i = 0; i < drivers.Length; ++i ) {
				double tmp = this.location.difference(drivers[i].location);
				if( drivers[i].getStatus() == "available" && tmp < currentDist ) {
					best = i;
					currentDist = tmp;
				}
			}
			if( best > -1 ) {
				return drivers[best];
			}
			else {
				return default(Driver);
			}
		}
		
		public void addDonation( Donation donation ) {
			this.donation = donation;
		}
	}
}
