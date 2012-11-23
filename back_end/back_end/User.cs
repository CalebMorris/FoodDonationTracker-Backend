using System;

namespace back_end
{
	public class User
	{	
		protected string uName; // or Password
		protected string uPass; //TODO CHANGE THIS	
		protected GPS location;
		
		
		//public User () {}
		public User ( string uname, string upass, GPS loc ) {
			uName = uname; uPass = upass;
			location = loc; status = Status.unavailable;
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

