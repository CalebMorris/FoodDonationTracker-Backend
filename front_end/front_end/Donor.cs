using System;

namespace back_end
{
	public class Donor : User
	{
		//TODO add food type 
		public double ttl; // time until food expires in seconds
		public TimeSpan lastCheck;
		
		public Donor ():base("","","Donor",null) {
			lastCheck = DateTime.Now.TimeOfDay;
		}
		public Donor( string uname, string upass, GPS loc, double timetolive ) 
				: base(uname, upass, "Donor", loc ) {
			lastCheck = DateTime.Now.TimeOfDay;
		}
		
		public Receiver findBestDropOff( Receiver[] receivers ) {
			if( receivers.Length == 0 ) {
				return null;
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
		
		public int timeLeft() {
			// Updates the ttl
			// If the ttl is reached 0 or less return -1 
			//   and send message that time has expired
			lastCheck = (DateTime.Now.TimeOfDay - this.lastCheck);
			ttl -= lastCheck.TotalSeconds;
			if( ttl <= 0 ) {
				return -1;
			}
			else{
				return 1;
			}
		}
	}
}
