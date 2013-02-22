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
		public webservice_mono(){

			appState = HttpContext.Current.Application;
		}
		
		protected HttpApplicationState appState;
		
		public struct Query {
			public string status;
			public string message;
			public Query( string status, string m ) {
				this.status = status;
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
			public string status;
			public string authenToken;
			public string message;
			public string role;
			public int expiry;
			
			public Authen( string tok, string mes, string rol ) {
				authenToken = tok; message = mes; role = rol;
				status = "unavailable";//Status.unavailable;
				expiry = 0;
			}
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
			if(tmpDr == null) tmpDr = new List<Driver>();
			if(tmpDr == null) tmpDo = new List<Donor>();
			if(tmpDr == null) tmpR = new List<Receiver>();
			
			bool flag = false;
			int i = 0;
			User uTmp = null;

			uTmp = tmpDr.Find( x => x.username().Equals(email));
			if( uTmp != null ) {
				flag = uTmp.authenticate(email, password);
			}

			uTmp = tmpDr.Find(x => x.username().Equals(email));

			if( uTmp != null ) {
				flag = uTmp.authenticate(email, password);
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
			if( uTmp == null ) {
				return new Authen( "", "User Not Found", "User" );
			}
			else if( flag == false ) {
				return new Authen( "", "Password Incorrect", "User" );
			}
			else {
				
				((Dictionary<String, Tuple<User,String>>)appState["users"])[hash] = new Tuple<User,String>(uTmp, "");
				return new Authen( hash, "Succesful Authen", uTmp.getRole() );
			}
		}
		
		[WebMethod]
		public Query statusChange( string authenToken, string status ) {
			//Return status, message
			//TODO add state change here 
			//TODO   if state because available, push the next pickup (if available)
			List<Driver> tmpDr = 
				(List<Driver>)appState["drivers"];
			Dictionary<String, Tuple<User, String>> tmpAuthn = 
				(Dictionary<String, Tuple<User, String>>)appState["users"];
			User uTmp = null;
			if( tmpAuthn.ContainsKey(authenToken) ) {
				uTmp = tmpAuthn[authenToken].Item1;
			}
			else {
				return new Query( "error", "Authen Token Not Recognized");				
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
				return new Query( "unauthenticated", "Unauthenticated" );
			}
		}
		
		[WebMethod]
		public bool locationUpdate( string authenToken, int latitude, int longitude ) {
			//@TODO implement
			return false;//Failure
		}
		
		[WebMethod]
		public bool GCMRegister( string authenToken, string regId ) {
			//@TODO Implement
			Dictionary<String, Tuple<User, String>> users = (Dictionary<String, Tuple<User,String>>)appState["users"];
			if( appState["users"] == null ) {
				//There aren't any users
				appState["users"] = new Dictionary<String, Tuple<User,String>>();
				return false;
			}
			if( !users.ContainsKey(authenToken) ) {
				//Token doesn't exists
				return false;
			}
			
			User uTmp = users[authenToken].Item1;
			users[authenToken] = new Tuple<User, String>(uTmp, regId);
			return true;
		}
		
		[WebMethod]
		public Query queryStatus( string authenToken ) {
			//Return status, message
			List<Driver> tmpDr = 
				(List<Driver>)appState["drivers"];
			Dictionary<String, Tuple<User, String>> tmpAuthn = 
				(Dictionary<String, Tuple<User,String>>)appState["users"];
			User uTmp = null;
			if( tmpAuthn.ContainsKey(authenToken) ) {
				uTmp = tmpAuthn[authenToken].Item1;
			}
			else{
				return new Query( "error", "Authen Token Not Recognized");	
			}
			int i;
			for( i = 0; i < tmpDr.Count && 
			    tmpDr[i].username() != uTmp.username(); ++i );
			if( i != tmpDr.Count ) {
				return new Query( tmpDr[i].getStatus(), "Current Status" );
			}
			else {
				return new Query( "unauthenticated", "Unauthenticated");
			}
		}
		
		[WebMethod]
		public Donation queryDonation( string authenToken ) {
			//@TODO implement
			
			
			return new Donation("","","",1,1,"","","",1,1,"");
		}		
		
		[WebMethod]
		public bool submitDonation( string upName, string upPhone, string upDetails, int upLat, int upLon,
			                 string dName, string dPhone, string dDetails, int dLat, int dLon,
			                 string message ) {
			//@TODO implement
			return false;
		}
		
		/* Debugging Methods */

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
		
		[WebMethod]
		public string testPush (string authenToken, string message)
		{
			Dictionary<String, Tuple<User,String>> users = ((Dictionary<String, Tuple<User,String>>)appState ["users"]);
			if (users == null) {
				users = new Dictionary<string, Tuple<User, string>> ();
				return "No Users";
			}
			if (!users.ContainsKey (authenToken)) {
				return "Token not recognized";
			}
			String deviceId = users[authenToken].Item2;
			return Pusher.SendNotification(deviceId, message);			
		}
		
		[WebMethod]
		public bool writeDriver( string user, string pass ) {
			
			if( appState["drivers"] == null ) {
				appState["drivers"] = new List<Driver>();
			}
			for( int i = 0; i < ((List<Driver>)appState["drivers"]).Count; ++i ) {
				if( user == ((List<Driver>)appState["drivers"])[i].username() ) {
					return false;
				}
			}
			((List<Driver>)appState["drivers"]).Add( new Driver( user, pass, new GPS(33.71,141.13) ));
			
			return true;
		}
		
		[WebMethod]
		public bool writeDonor( string user, string pass ) {
			
			if( appState["donors"] == null ) {
				appState["donors"] = new List<Donor>();
			}
			for( int i = 0; i < ((List<Donor>)appState["donors"]).Count; ++i ) {
				if( user == ((List<Donor>)appState["donors"])[i].username() ) {
					return false;
				}
			}
			((List<Donor>)appState["donors"]).Add( new Donor( user, pass, new GPS(1,1), /*ttl*/0.0 ));
			
			return true;
		}
		
		[WebMethod]
		public bool writeReceiver( string user, string pass ) {
			
			if( appState["receivers"] == null ) {
				appState["receivers"] = new List<Receiver>();
			}
			for( int i = 0; i < ((List<Receiver>)appState["receivers"]).Count; ++i ) {
				if( user == ((List<Receiver>)appState["receivers"])[i].username() ) {
					return false;
				}
			}
			((List<Receiver>)appState["receivers"]).Add( new Receiver( user, pass, new GPS(1,1) ));
			
			return true;
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
		public string printUsers() {
			string result = "";
			Dictionary<String, Tuple<User,String>> users = (Dictionary<String, Tuple<User,String>>)appState["users"];
			if( users == null )
				users = new Dictionary<string, Tuple<User, string>>();
			if( users.Count == 0 )
				return "No Users\n";
			foreach( String key in users.Keys) {
				result += key + " " + users[key].Item1 + " " + users[key].Item2 + "\n";
			}
			return result;
		}		          
	}
}

