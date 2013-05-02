using System;

namespace back_end
{
	public class Donation
	{
		public string pickupContactName, pickupContactPhone, 
		       pickupExtraDetails;
		public int pickupLatitude, pickupLongitude;
		public bool active;
		public Donor donor;
		public int epoch_time;
		
		public Donation ( string pickupContactName, 
		                  string pickupContactPhone, 
		                  string pickupExtraDetails,
						  int pickupLatitude, int pickupLongitude, 
		                  Donor donor, int epoch )
		{
			// Needs to add a time field that is made when created/activated
			this.pickupContactName  = pickupContactName;
			this.pickupContactPhone = pickupContactPhone;
			this.pickupExtraDetails = pickupExtraDetails;
			this.pickupLatitude		= pickupLatitude;
			this.pickupLongitude 	= pickupLongitude;
			this.active 			= true;
			this.donor				= donor;
			this.epoch_time			= epoch;
		}
		
		public int compareTo( Donation another ) {
			// -1 if this is < another
			// 0 if they are equivilent
			// +1 if this is > another
			//if(  ) {
			return 0;
			//}
		}
		
		public int getEpoch() {
			return this.epoch_time;			
		}
	}
}
