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
		
		protected List<Donor> donors;
		protected List<Receiver> recievers;
		protected List<Driver> drivers;
		
		public struct Query {
			public Status stat;
			public string message;
			public Query( Status s, string m ) {
				stat = s;
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
			public string token;
			public string message;
			public string role;
			
			public Authen( string t, string m, string r ) {
				token = t; message = m; role = r;
				if( r == "Donor" ) {
					status = Status.unavailable;
				}
				else if( r == "Driver" ) {
					status = Status.unavailable;
				}
				else if( r == "Receiver" ) {
					status = Status.unavailable;
				}
			}
		}
		
		public webservice_mono(){
			/*
			appState.Clear();
			appState.Add("donors", donors);
			appState.Add("receivers", recievers);
			appState.Add("drivers", drivers);
			appState["donors"] = new List<Donor>();
			appState["receivers"] = new List<Receiver>();
			appState["drivers"] = new List<Driver>();
			*/
		}
		
		[WebMethod]
		public string printDrivers() {
			
			string ret = "";
			/*
			for( int i = 0; i < ((List<Driver>)appState["drivers"]).Count; ++i ) {
				ret += ((List<Driver>)appState["drivers"])[i].username() + "\n";
			}
			*/
			return ret;
			
		}
		
		[WebMethod]
		public int countActive() {
			HttpApplicationState appState;
			appState = Application.Contents;
			int OnlineUsers = 0;
			if (appState["Counter"] != null)
	        {
	            OnlineUsers = ((int)appState["Counter"]);
	        }
			return OnlineUsers;
		}
		
		[WebMethod]
		public int writeReceiver( string user, string pass ) {
			/*
			if( appState["receivers"] == null ) {
				appState["receivers"] = new List<Receiver>();
			}
			for( int i = 0; i < ((List<Receiver>)appState["receivers"]).Count; ++i ) {
				if( user == ((List<Receiver>)appState["receivers"])[i].username() ) {
					return -1;
				}
			}
			((List<Receiver>)appState["recievers"]).Add( new Receiver( user, pass, null ));
			*/
			return 1;
		}
		
		[WebMethod]
		public int writeDriver( string user, string pass ) {
			/*
			if( appState["donors"] == null ) {
				appState["donors"] = new List<Donor>();
			}
			for( int i = 0; i < ((List<Driver>)appState["drivers"]).Count; ++i ) {
				if( user == ((List<Driver>)appState["drivers"])[i].username() ) {
					return -1;
				}
			}
			((List<Driver>)appState["drivers"]).Add( new Driver( user, pass, null ));
			*/
			return 1;
		}
		
		[WebMethod]
		public int writeDonor( string user, string pass ) {
			/*
			if( appState["drivers"] == null ) {
				appState["drivers"] = new List<Driver>();
			}
			for( int i = 0; i < ((List<Donor>)appState["donors"]).Count; ++i ) {
				if( user == ((List<Donor>)appState["donors"])[i].username() ) {
					return -1;
				}
			}
			((List<Donor>)appState["donors"]).Add( new Donor( user, pass, null ));
			*/
			return 1;
		}
		
		[WebMethod]
		public Authen authenticateUser( string email, string pass ) {
			//Return status, token, role, message
			MD5 token = MD5.Create(email+pass);
			Console.Write(token.Hash);
			
			bool flag = false;
			int i = 0;
			User uTmp = null;
			for( i = 0; i < drivers.Count 
			  && drivers[i].username() != email; ++i );
			if( i != drivers.Count ) {
				flag = drivers[i].authenticate(email, pass);
				uTmp = drivers[i];
			}
			else {
				for( i = 0; i < donors.Count 
			      && donors[i].username() != email; ++i );
				if( i != donors.Count ) {
					flag = donors[i].authenticate(email, pass);
					uTmp = donors[i];
				}
				else {
					for( i = 0; i < recievers.Count 
			    	  && recievers[i].username() != email; ++i );
					if( i != recievers.Count ) {
						flag = recievers[i].authenticate(email, pass);
						uTmp = recievers[i];
					}
				}
			}
			
			Application["Authenticated"] = flag;
			return Authen( 
			              token.Hash, 
			              "", 
			              uTmp.getRole() );
		}
		
		[WebMethod]
		public Query statusChange( Driver d, Status s ) {
			//Return status, message
			if( (bool)Application["Authenticated"] ) {
				//Authenticated
				return Query( d.updateStatus(s), "" );
			}
			else {
				return Query( null, "Unauthenticated" );
			}
		}
		
		[WebMethod]
		public Query queryStatus( Driver d ) {
			//Return status, message
			if( (bool)Application["Authenticated"] ) {
				return Query( d.getStatus(), "" );
			}
			else {
				return Query(null, "Unauthenticated");
			}
		}
		
		[WebMethod]
		public Place queryPickup( Donor d ) {
			//Return time, location, details
			if( (bool)Application["Authenticated"] ) {
				return Place( System.DateTime.Now, d.getLoc(), "" );
			}
			else {
				return Place(null, null, "Unauthenticated");	
			}
		}
		
		[WebMethod]
		public Place queryDropoff( Receiver r ) {
			//Return time, location, details
			if( (bool)Application["Authenticated"] ) {
				return Place( System.DateTime.Now, r.getLoc(), "" );
			}
			else {
				return Place(null, null, "Unauthenticated");	
			}
		}
	}
}

