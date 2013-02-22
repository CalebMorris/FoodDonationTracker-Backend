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
			((List<Donor>)Application["donors"]).Add(new Donor("donor","pass",new GPS(2,2), 100.0));
			Application["receivers"] = new List<Receiver>();
			((List<Receiver>)Application["receivers"]).Add(new Receiver("receiver","pass",new GPS(3,3),"","",""));
			Application["drivers"] = new List<Driver>();
			((List<Driver>)Application["drivers"]).Add(new Driver("driver","pass", new GPS(1,1)));
			Application["users"] = new Dictionary<String, Tuple<User, String>>();
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

