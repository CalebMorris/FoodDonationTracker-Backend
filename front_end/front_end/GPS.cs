using System;

namespace back_end
{
	public class GPS
	{
		protected double latitude;
		protected double longitude;
		
		public GPS () { latitude = -1; longitude = -1; }
		public GPS ( double lat, double lon ) {
			latitude = lat; longitude = lon;
		}
		public GPS ( int lat, int lon ) {
			this.latitude = (int)(lat*1000000);
			this.longitude = (int)(lon*1000000);
		}
		
		public double getLat() { return latitude; }
		public double getLon() { return longitude; }
		public void setLat( double lat ) { latitude = lat; }
		public void setLon( double lon ) { longitude = lon; }
		
		public double difference( GPS other ) {
			return Math.Pow((this.latitude-other.latitude),2.0) 
				 + Math.Pow((this.longitude-other.longitude),2.0);
		}
	}
}

