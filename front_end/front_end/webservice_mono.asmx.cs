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
			public Driver.Status stat;
			public string message;
			public Query( Driver.Status s, string m ) {
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
		public bool authenticateUser( string email, string pass ) {
			//Return status, token, role, message
			MD5 token = MD5.Create(email+pass);
			Console.Write(token.Hash);
			
			return false;
		}
		
		[WebMethod]
		public Query statusChange( Driver d, Driver.Status s ) {
			//Return status, message
			if( false ) {
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
			if( false ) {
				return Query( d.getStatus(), "" );
			}
			else {
				return Query(null, "Unauthenticated");
			}
		}
		
		[WebMethod]
		public Place queryPickup( Donor d ) {
			//Return time, location, details
			if( false ) {
				return Place( DataTime.Now, d.getLoc(), "" );
			}
			else {
				return Place(null, null, "Unauthenticated");	
			}
		}
		
		[WebMethod]
		public Place queryDropoff( Receiver r ) {
			//Return time, location, details
			if( false) {
				return Place( DataTime.Now, r.getLoc(), "" );
			}
			else {
				return Place(null, null, "Unauthenticated");	
			}
		}
	}
}

