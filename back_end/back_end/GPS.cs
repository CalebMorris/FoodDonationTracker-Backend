using System;

namespace back_end
{
	public class GPS
	{
		protected double latitude;
		protected double longitude;
		
		//public GPS () { }
		public GPS ( double lat, double lon ) {
			latitude = lat; longitude = lon;
		}
		
		public double getLat() { return latitude; }
		public dobule getLon() { return longitude; }
		public void setLat( double lat ) { latitude = lat; }
		public void setLon( double lon ) { longitude = lon; }
	}
}

