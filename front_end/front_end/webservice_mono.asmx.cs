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
	}
}

