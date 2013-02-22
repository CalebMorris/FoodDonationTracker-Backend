using System;

namespace back_end
{
	public struct Donation {
		public string 	pickupContactName;
		public string 	pickupContactPhone;
		public string 	pickupExtraDetails;
		public int 		pickupLatitude;
		public int 		pickupLongitude;
		public string 	dropoffContactName;
		public string 	dropoffContactPhone;
		public string 	dropoffContactExtraDetails;
		public int 		dropoffLatitude;
		public int 		dropoffLongitude;
		public string 	message;
		
		public Donation( string upName, string upPhone, string upDetails, int upLat, int upLon,
		                 string dName, string dPhone, string dDetails, int dLat, int dLon,
		                 string message ) {
			pickupContactName = upName; pickupContactPhone = upPhone; pickExtraDetails = upDetails;
			pickupLatitude = upLat; pickupLongitude = upLon;
			dropoffContactName = dName; dropoffContactPhone = dPhone; dropoffContactExtraDetails = dDetails;
			dropoffLatitude = dLat; dropoffLongitude = dLon;
			this.message = message;
		}
	}
		
	public class Donor : User
	{
		//TODO add food type 
		public double ttl; // time until food expires in seconds
		public TimeSpan lastCheck;
		public Donation donation;
		
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
