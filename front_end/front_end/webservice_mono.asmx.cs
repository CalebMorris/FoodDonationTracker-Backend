using System;
using System.Web;
using System.Web.Services;

using System.Collections.Generic;
using System.Security.Cryptography;

using back_end;

namespace front_end
{
	//public class webservice_mono : System.Web.Services.WebService
	[WebService(Namespace = "http://capstone485.asu.edu/")]
	public partial class webservice_mono : System.Web.UI.Page
	{
		protected HttpApplicationState appState;
		
		public struct Query {
			public Status status;
			public string message;
			public Query( Status s, string m ) {
				status = s;
				message = m;
			}
		}
		
		public struct Place {
			//Return time, location, details
			public DateTime time;
			public GPS location;
			public string message;
			public Place( DateTime t, GPS l, string m ) {
				time = t;
				location = l;
				message = m;
			}
		}
		
		public struct Authen {
			//Return status, token, role, message
			public Status status;
			public string authenToken;
			public string message;
			public string role;
			public int expiry;
			
			public Authen( string tok, string mes, string rol ) {
				authenToken = tok; message = mes; role = rol;
				status = Status.unavailable;
				expiry = 0;
			}
		}
		
		public webservice_mono(){

			appState = HttpContext.Current.Application;
		}
		
		[WebMethod]
		public string printDrivers() {
			string ret = "Drivers: \n";
			List<Driver> tmp = (List<Driver>)appState["drivers"];
			if( tmp.Count == 0 ) {
				return "No Drivers";
			}
			else {
				for( int i = 0; i < tmp.Count; ++i ) {
					ret += tmp[i].username() + "\n";
				}
				return ret;
			}
		}
		
		[WebMethod]
		public string printDonors() {
			string ret = "Donors: \n";
			List<Donor> tmp = (List<Donor>)appState["donors"];
			if( tmp.Count == 0 ) {
				return "No Donors";
			}
			else {
				for( int i = 0; i < tmp.Count; ++i ) {
					ret += tmp[i].username() + "\n";
				}
				return ret;
			}
		}
		
		[WebMethod]
		public string printReceivers() {
			string ret = "Receivers: \n";
			List<Receiver> tmp = (List<Receiver>)appState["receivers"];
			if( tmp.Count == 0 ) {
				return "No Receivers";
			}
			else {
				for( int i = 0; i < tmp.Count; ++i ) {
					ret += tmp[i].username() + "\n";
				}
				return ret;
			}
		}
		
		[WebMethod]
		public int writeDriver( string email, string password) {
			
			if( appState["donors"] == null ) {
				appState["donors"] = new List<Donor>();
			}
			for( int i = 0; i < ((List<Driver>)appState["drivers"]).Count; ++i ) {
				if( email == ((List<Driver>)appState["drivers"])[i].username() ) {
					return -1;
				}
			}
			((List<Driver>)appState["drivers"]).Add( new Driver( email, password, new GPS(33.71,141.13) ));
			
			return 1;
		}
		
		[WebMethod]
		public int writeDonor( string email, string password, GPS location, double ttl ) {
			
			if( appState["drivers"] == null ) {
				appState["drivers"] = new List<Driver>();
			}
			for( int i = 0; i < ((List<Donor>)appState["donors"]).Count; ++i ) {
				if( email == ((List<Donor>)appState["donors"])[i].username() ) {
					return -1;
				}
			}
			((List<Donor>)appState["donors"]).Add( new Donor( email, password, location, ttl ));
			
			return 1;
		}
		
		[WebMethod]
		public int writeReceiver( string email, string password, GPS location ) {
			
			if( appState["receivers"] == null ) {
				appState["receivers"] = new List<Receiver>();
			}
			for( int i = 0; i < ((List<Receiver>)appState["receivers"]).Count; ++i ) {
				if( email == ((List<Receiver>)appState["receivers"])[i].username() ) {
					return -1;
				}
			}
			((List<Receiver>)appState["receivers"]).Add( new Receiver( email, password,  location ));
			
			return 1;
		}
		
		[WebMethod]
		public Authen authenticateUser( string email, string password ) {
			//Return status, token, role, message
			MD5 hasher = MD5.Create();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (Byte b in hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(email+password)))
                    sb.Append(b.ToString("x2").ToLower());
			string hash = sb.ToString();
			
			List<Driver> 	tmpDr = (List<Driver>)appState["drivers"];
			List<Donor> 	tmpDo = (List<Donor>)appState["donors"];
			List<Receiver> 	tmpR  = (List<Receiver>)appState["receivers"];
			
			bool flag = false;
			int i = 0;
			User uTmp = null;
			for( i = 0; i < tmpDr.Count 
			  && tmpDr[i].username() != email; ++i );
			if( i != tmpDr.Count ) {
				flag = tmpDr[i].authenticate(email, password);
				uTmp = tmpDr[i];
			}
			else {
				for( i = 0; i < tmpDo.Count 
			      && tmpDo[i].username() != email; ++i );
				if( i != tmpDo.Count ) {
					flag = tmpDo[i].authenticate(email, password);
					uTmp = tmpDo[i];
				}
				else {
					for( i = 0; i < tmpR.Count 
			    	  && tmpR[i].username() != email; ++i );
					if( i != tmpR.Count ) {
						flag = tmpR[i].authenticate(email, password);
						uTmp = tmpR[i];
					}
				}
			}
			
			//appState["Authenticated"] = flag;
			if( i == tmpR.Count ) {
				return new Authen( "", "User Not Found", "User" );
			}
			else if( flag == false ) {
				return new Authen( "", "Password Incorrect", "User" );
			}
			else {
				((Dictionary<String, User>)appState["users"]).Add(hash, uTmp);
				return new Authen( hash, "Succesful Authen", uTmp.getRole() );
			}
		}
		
		[WebMethod]
		public string testPush( string deviceId, string message ) {
			return Pusher.SendNotification(deviceId, message);			
		}
		
		[WebMethod]
		public Query statusChange( string authenToken, Status status ) {
			//Return status, message
			//TODO add state change here 
			//TODO   if state because available, push the next pickup (if available)
			List<Driver> tmpDr = 
				(List<Driver>)appState["drivers"];
			Dictionary<String, User> tmpAuthn = 
				(Dictionary<String, User>)appState["users"];
			User uTmp = null;
			if( tmpAuthn.ContainsKey(authenToken) ) {
				uTmp = tmpAuthn[authenToken];
			}
			else {
				return new Query( Status.error, "Authen Token Not Recognized");				
			}
			bool userIsDriver = false;
			int i;
			for( i = 0; i < tmpDr.Count && uTmp.username() != tmpDr[i].username(); ++i );
			userIsDriver = (i != tmpDr.Count);
			if( userIsDriver ) {
				//Authenticated
				return new Query( tmpDr[i].updateStatus(status), "Status Succesfully Updated" );
			}
			else {
				return new Query( Status.unauthenticated, "Unauthenticated" );
			}
		}
		
		/*
		[WebMethod]
		public Query statusChange( Driver d, Status s ) {
			//Return status, message
			if( (bool)Application["Authenticated"] ) {
				//Authenticated
				return new Query( d.updateStatus(s), "" );
			}
			else {
				return new Query( Status.unauthenticated, "Unauthenticated" );
			}
		}
		*/
		
		[WebMethod]
		public Query queryStatus( string authenToken ) {
			//Return status, message
			List<Driver> tmpDr = 
				(List<Driver>)appState["drivers"];
			Dictionary<String, User> tmpAuthn = 
				(Dictionary<String, User>)appState["users"];
			User uTmp = null;
			if( tmpAuthn.ContainsKey(authenToken) ) {
				uTmp = tmpAuthn[authenToken];
			}
			else{
				return new Query( Status.error, "Authen Token Not Recognized");	
			}
			int i;
			for( i = 0; i < tmpDr.Count && 
			    tmpDr[i].username() != uTmp.username(); ++i );
			if( i != tmpDr.Count ) {
				return new Query( tmpDr[i].getStatus(), "Current Status" );
			}
			else {
				return new Query( Status.unauthenticated, "Unauthenticated");
			}
		}
		
		/*
		[WebMethod]
		public Query queryStatus( Driver d ) {
			//Return status, message
			if( (bool)Application["Authenticated"] ) {
				return new Query( d.getStatus(), "" );
			}
			else {
				return new Query( Status.unauthenticated, "Unauthenticated");
			}
		}
		*/
		
		[WebMethod]
		public Place queryPickup( string email ) {
			//Return time, location, details
			List<Donor> tmpDr = (List<Donor>)appState["donors"];
			Donor d; int i;
			for( i = 0; i < tmpDr.Count 
			  && tmpDr[i].username() != email; ++i );
			if( i != tmpDr.Count ) {
				d = tmpDr[i];
				return new Place( System.DateTime.Now, d.getLoc(), "" );
			}
			else {
				return new Place( new System.DateTime(), new GPS(), "Unauthenticated");	
			}
		}
		
		/*
		[WebMethod]
		public Place queryPickup( Donor d ) {
			//Return time, location, details
			if( (bool)Application["Authenticated"] ) {
				return new Place( System.DateTime.Now, d.getLoc(), "" );
			}
			else {
				return new Place( new System.DateTime(), new GPS(), "Unauthenticated");	
			}
		}
		*/
		
		[WebMethod]
		public Place queryDropoff( string email ) {
			//Return time, location, details
			List<Receiver> tmpDr = (List<Receiver>)appState["receivers"];
			Receiver d; int i;
			for( i = 0; i < tmpDr.Count 
			  && tmpDr[i].username() != email; ++i );
			if( i != tmpDr.Count ) {
				d = tmpDr[i];
				return new Place( System.DateTime.Now, d.getLoc(), "" );
			}
			else {
				return new Place( new System.DateTime(), new GPS(), "Unauthenticated");	
			}
		}
		/*
		[WebMethod]
		public Place queryDropoff( Receiver r ) {
			//Return time, location, details
			if( (bool)Application["Authenticated"] ) {
				return new Place( System.DateTime.Now, r.getLoc(), "" );
			}
			else {
				return new Place( new System.DateTime(), new GPS(), "Unauthenticated");	
			}
		}
		*/
	}
}

