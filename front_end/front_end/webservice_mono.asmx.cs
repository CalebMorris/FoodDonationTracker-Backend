using System;
using System.Web;
using System.Web.Services;

namespace front_end
{
	public class webservice_mono : System.Web.Services.WebService
	{
		public webservice_mono(){}
		
		[WebMethod]
		public int sum(int a, int b){
			return a+b;
		}
		
		public void writePos_pickup(  ) {
			/*	Writes a gps to DB of a pickup
			 */
		}
		public void writePos_driver() {
			/*	Writes a gps to DB of a driver
			 * 	periodicly called by android app
			 */
		}
	}
}

