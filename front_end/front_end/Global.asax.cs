using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

using System.Collections.Generic;
using back_end;

namespace front_end
{
	public class Global : System.Web.HttpApplication
	{	
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
			Application["donors"] = new List<Donor>();
			((List<Donor>)Application["donors"]).Add(new Donor("donor",webservice_mono.saltPass("pass"),new GPS(033419084, -111938109)));
			Application["receivers"] = new List<Receiver>();
			((List<Receiver>)Application["receivers"]).Add(new Receiver("receiver",webservice_mono.saltPass("pass"),new GPS(033419505, -111912800),"","",""));
			Application["drivers"] = new List<Driver>();
			((List<Driver>)Application["drivers"]).Add(new Driver("driver",webservice_mono.saltPass("pass"), new GPS(1,1)));
			Application["users"] = new Dictionary<String, Tuple<User, String>>();
			Application["activeDonations"] = new List<Donor>();
			Application["queue"] = new Queue_t<Donation>();
		}
		
		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Session_End (Object sender, EventArgs e)
		{	
		}
		
		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

