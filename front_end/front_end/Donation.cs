using System;

namespace back_end
{
	public class Donation
	{
		public string pickupContactName, pickupContactPhone, 
		       pickupExtraDetails;
		public int pickupLatitude, pickupLongitude;
		public bool active;
		//@TODO put time component in the donation itself
		//public int ttl; // time until food expires in seconds
		//public TimeSpan lastCheck;
		
		public Donation ( string pickupContactName, 
		                  string pickupContactPhone, string pickupExtraDetails,
						  int pickupLatitude, int pickupLongitude )
		{
			// Needs to add a time field that is made when created/activated
			this.pickupContactName  = pickupContactName;
			this.pickupContactPhone = pickupContactPhone;
			this.pickupExtraDetails = pickupExtraDetails;
			this.pickupLatitude		= pickupLatitude;
			this.pickupLongitude 	= pickupLongitude;
			this.active 			= true;
		}
		
		public int compareTo( Donation another ) {
			// -1 if this is < another
			// 0 if they are equivilent
			// +1 if this is > another
			//if(  ) {
			return 0;
			//}
		}
	}
}
