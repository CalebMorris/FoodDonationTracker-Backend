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
		
		public struct Transfer {
			public string 	pickupContactName;
			public string 	pickupContactPhone;
			public string 	pickupExtraDetails;
			public int 		pickupLatitude;
			public int 		pickupLongitude;
			public string	dropoffContactName;
			public string	dropoffContactPhone;
			public string	dropoffExtraDetails;
			public int		dropoffLatitude;
			public int		dropoffLongitude;
			public string 	message;
			
			public Transfer( string upName, string upPhone, string upDetails, int upLat, int upLon,
			                 string dName, string dPhone, string dDetails, int dLat, int dLon,
			                 string message) {
				pickupContactName = upName; pickupContactPhone = upPhone; 
				pickupExtraDetails = upDetails;
				pickupLatitude = upLat; pickupLongitude = upLon;
				dropoffContactName = dName; dropoffContactPhone = dPhone; dropoffExtraDetails = dDetails;
				dropoffLatitude = dLat; dropoffLongitude = dLon; this.message = message;
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
				uTmp.authToken = hash;
				((Dictionary<String, Tuple<User,String>>)appState["users"])[hash] = new Tuple<User,String>(uTmp, "");
				return new Authen( hash, "Succesful Authen", uTmp.getRole() );
			}
		}
		
		[WebMethod]
		public Query statusChange( string authenToken, string status ) {
			//Return status, message
			//@TODO push state on change
			Dictionary<String, Tuple<User, String>> tmpAuthn = 
				(Dictionary<String, Tuple<User, String>>)appState["users"];
			User uTmp = null;
			if( tmpAuthn.ContainsKey(authenToken) ) {
				uTmp = tmpAuthn[authenToken].Item1;
			}
			else {
				return new Query( "error", "Authen Token Not Recognized");				
			}
			if( uTmp.getRole() == "Driver" ) {
				//Authenticated
				//@TODO if state becomes available, push the next pickup (if available)
				
				if(status == "available") {
				
					List<Donor> donations = ((List<Donor>)appState["activeDonations"]);
					
					if( donations != null && donations.Count > 0 ) {
						Donor nextDonor = donations[0];
						Driver driver = (Driver)uTmp;
						Receiver dropoff = nextDonor.findBestDropOff( ((List<Receiver>)appState["receivers"]).ToArray() );
						if( dropoff != default(Receiver) ) {
							// There is at least one reciever
							driver.assignPickup(nextDonor);
							driver.assignDropoff(dropoff);
							testPush(driver.authToken,"Donation Available");
						}
						else {
							// No drop-off. what do?
							//@TODO ???
							return new Query("error", "User is empty?");
						}
					}
				}
				
				return new Query( ((Driver)uTmp).updateStatus(status), "Status Succesfully Updated" );
			}
			else {
				return new Query( "unauthenticated", "Unauthenticated" );
			}
		}
		
		[WebMethod]
		public bool locationUpdate( string authenToken, int latitude, int longitude ) {
			/* Updates the location of a user with matching authenToken */ 
			Dictionary<String, Tuple<User,String>> users = ((Dictionary<String, Tuple<User,String>>)appState ["users"]);
			if (users == null) {
				users = new Dictionary<string, Tuple<User, string>> ();
				return false;
			}
			if (!users.ContainsKey (authenToken)) {
				return false;
			}
			User uTmp = users[authenToken].Item1;
			uTmp.updateLoc(new GPS(latitude, longitude));
			
			return true;
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
		public Transfer queryDonation( string authenToken ) {
			/* Retrieves information of donation */
			//@TODO dummy return
			return new Transfer("Caleb", "480-123-4567", "Use the doorbell", 
			           033419084, -111938109, "Michael", "602-987-6543", "Don't fall into the valcano",
			           033419505, -111912800, "Test Transfer");
			Dictionary<String, Tuple<User,String>> users = ((Dictionary<String, Tuple<User,String>>)appState ["users"]);
			if (users == null) {
				users = new Dictionary<string, Tuple<User, string>> ();
				return default(Transfer);
			}
			if (!users.ContainsKey (authenToken)) {
				return default(Transfer);
			}
			User uTmp = users[authenToken].Item1;
			if( uTmp.getRole() == "Driver" ) {
				/* Need to return both donor and dropoff info */
				Donation tDon = ((Driver)uTmp).getPickup().donation;
				Receiver tDrop = ((Driver)uTmp).getDropoff();
				return new Transfer(tDon.pickupContactName, tDon.pickupContactPhone,
				  tDon.pickupExtraDetails, tDon.pickupLatitude, tDon.pickupLongitude,
				  tDrop.contactName, tDrop.contactPhone, tDrop.extraDetails, 
				  tDrop.location.getLatI(), tDrop.location.getLonI(), "");
				                    
			}
			else return default(Transfer);
		}		
		
		[WebMethod]
		public string submitDonation( string authenToken, string pickupContactName, string pickupContactPhone, 
		                     string pickupExtraDetails, int pickupLatitude, int pickupLongitude ) {
			//@TODO implement
			Dictionary<String, Tuple<User,String>> users = ((Dictionary<String, Tuple<User,String>>)appState ["users"]);
			if (users == null) {
				users = new Dictionary<string, Tuple<User, string>> ();
				return "No Users";
			}
			if (!users.ContainsKey (authenToken)) {
				return "User Doesn't Exist";
			}
			User uTmp = users[authenToken].Item1;
			
			if( uTmp.getRole() == "Donor" ) {
				((Donor)uTmp).addDonation(new Donation( pickupContactName, pickupContactPhone, 
		                      pickupExtraDetails, pickupLatitude, pickupLongitude));
				
				Driver driver = ((Donor)uTmp).findBestDriver( ((List<Driver>)appState["drivers"]).ToArray() );
				if( driver != default(Driver) ) {
					// There is at least one driver available
					Receiver dropoff = ((Donor)uTmp).findBestDropOff( ((List<Receiver>)appState["receivers"]).ToArray() );
					if( dropoff != default(Receiver) ) {
						// There is at least one reciever
						driver.assignPickup((Donor)uTmp);
						driver.assignDropoff(dropoff);
						testPush(driver.authToken,"Donation Available");
					}
					else {
						// No drop-off. what do?
						//@TODO ???
						return "User is empty?";
					}
				}
				else {
					((List<Donor>)appState["activeDonations"]).Add((Donor)uTmp);
					return "No Driver Available";
				}
				return "Pushing Donation";
			}
			else {
				return "Donating as a non-Donator";
			}
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
		public bool writeDonor( string user, string pass, int lat, int lon ) {
			
			if( appState["donors"] == null ) {
				appState["donors"] = new List<Donor>();
			}
			for( int i = 0; i < ((List<Donor>)appState["donors"]).Count; ++i ) {
				if( user == ((List<Donor>)appState["donors"])[i].username() ) {
					return false;
				}
			}
			((List<Donor>)appState["donors"]).Add( new Donor( user, pass, new GPS(lat,lon), 100 ));
			
			return true;
		}
		
		[WebMethod]
		public bool writeReceiver( string user, string pass, int lat, int lon,
		                           string contactName, string contactPhone,
		                           string extraDetails ) {
			
			if( appState["receivers"] == null ) {
				appState["receivers"] = new List<Receiver>();
			}
			for( int i = 0; i < ((List<Receiver>)appState["receivers"]).Count; ++i ) {
				if( user == ((List<Receiver>)appState["receivers"])[i].username() ) {
					return false;
				}
			}
			((List<Receiver>)appState["receivers"]).Add( 
					new Receiver( user, pass, new GPS(lat,lon), contactName, contactPhone, extraDetails ));
			
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
		public string printQueue() {
			return "printQueue dummy data.";
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

