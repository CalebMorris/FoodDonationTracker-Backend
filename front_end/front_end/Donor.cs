using System;

namespace back_end
{
	public struct Donation {
		public string 	pickupContactName;
		public string 	pickupContactPhone;
		public string 	pickupExtraDetails;
		public int 		pickupLatitude;
		public int 		pickupLongitude;
		
		public Donation( string upName, string upPhone, 
		                string upDetails, int upLat, int upLon ) {
			pickupContactName = upName; pickupContactPhone = upPhone; 
			pickupExtraDetails = upDetails;
			pickupLatitude = upLat; pickupLongitude = upLon;
		}
	}
		
	public class Donor : User
	{
		public int ttl; // time until food expires in seconds
		public TimeSpan lastCheck;
		public Donation donation;
		public string status;
		
		public Donor ():base("","","Donor",null) {
			lastCheck = DateTime.Now.TimeOfDay;
			status = "empty";
		}
		public Donor( string uname, string upass, GPS loc, int timetolive ) 
				: base(uname, upass, "Donor", loc ) {
			lastCheck = DateTime.Now.TimeOfDay;
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
		
		public int timeLeft() {
			// Updates the ttl
			// If the ttl is reached 0 or less return -1 
			//   and send message that time has expired
			lastCheck = (DateTime.Now.TimeOfDay - this.lastCheck);
			ttl -= (int)lastCheck.TotalSeconds;
			if( ttl <= 0 ) {
				return -1;
			}
			else{
				return 1;
			}
		}
		
		public void addDonation( Donation donation ) {
			this.donation = donation;
		}
	}
}
