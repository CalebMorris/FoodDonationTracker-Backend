using System;

namespace back_end
{
	public enum Status {
			available, unavailable, 
			assigned, enroute, 
			pickedup, droppedoff,
			unable_to_deliver,
		
			unassigned, waiting,
			unauthenticated
		};
	
	public class User
	{	
		
		protected string uName; // or Password
		protected string uPass; //TODO CHANGE THIS
		protected string role;
		protected GPS location;
		
		
		public User () { uName = ""; uPass = ""; 
			role = ""; location = null; }
		public User ( string uname, string upass, string role, GPS loc ) {
			uName = uname; uPass = upass;
			location = loc; this.role = role;
		}
		
		public string username() { return uName; }
		/*
		public string password() { //Shouldn't be able to get pass
			return uPass; 
		} 
		*/
		public GPS getLoc() { return location; }
		
		public void updateLoc( double lat, double lon ) { 
			location = new GPS(lat, lon);
		}
		
		public string getRole() { return role; }
		
		public void updateLoc( GPS loc ) { location = loc; }
		
		public void changePass( string pOld, string pNew ) {
			if (pOld == uPass) {
				uPass = pNew;
			}
		}
		
		public bool authenticate( string user, string pass ) {
			return (user == uName && pass == uPass);
		}
	}
}

