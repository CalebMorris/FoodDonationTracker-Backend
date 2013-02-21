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
			Application["receivers"] = new List<Receiver>();
			Application["drivers"] = new List<Driver>();
			Application["users"] = new Dictionary<String, User>();
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

