using System;

namespace front_end
{
	public class Gps
	{
		private double lat;
		private double lon;
		
		public Gps (){}
		public Gps( double latitude, double longitude ) {
			lat = latitude; lon = longitude;
		}
		public double getLat() { return lat; }
		public double getLon() { return lon; }
		
		public void setLat( double latitude ) {
			lat = latitude;	
		}
		
		public void setLon( double longitude ) {
			lon = longitude;
		}
		
		public Gps nearest( Gps[] gpsList ) {
			if( gpsList.Length == 0 ) {
				return null;
			}
			Gps ret = gpsList[0];
			double distCur = Math.Sqrt( Math.Pow(this.lat-ret.getLat(), 2) 
			                           + Math.Pow(this.lon-ret.getLon(), 2));
			for( int i = 1; i < gpsList.Length; ++i ) {
				double distTest = Math.Sqrt( Math.Pow(this.lat-gpsList[i].getLat(), 2) 
			                           + Math.Pow(this.lon-gpsList[i].getLon(), 2));
				if( distTest < ret ) {
					ret = gpsList[i];
					distCur = distTest;
				}
			}
			return ret;
		}
	}
}

