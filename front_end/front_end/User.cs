using System;

namespace back_end
{
	public class User
	{	
		string uName;
		string uPass;
		string role;
		public GPS location;
		public string authToken;
		
		public User () { uName = ""; uPass = ""; 
			role = ""; location = null; }
		public User ( string uname, string upass, string role, GPS loc ) {
			uName = uname; uPass = upass;
			location = loc; this.role = role;
		}
		
		public string username() { return uName; }
		public string password() { return uPass; } 
		public GPS getLoc() { return location; }
		
		public void updateLoc( GPS loc ) { 
			location = loc;
		}
		
		public string getRole() { return role; }
		
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

