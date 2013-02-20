using System;
using System.Net;
using System.IO;
using System.Text;

namespace front_end
{
	public class Pusher
	{
		public Pusher () {}
		
		public static string SendNotification(string deviceId, string message) {
			deviceId = "APA91bF7QrhCcjwXKNiXB9HOMnBJA8CEdWemgPCzEl7j5Uvd0RQrIs4iAF2oPE4houHibikUs8Mvgb0Dvt_RgWcOcRsMnI2kpgTdnL1iAtnqFSkH69Ryj4X71dBQ-MOPZZkg4iXzuc99-oHO-PEoanNlNl7Oh3zuAA";
			message = "test";
			string GoogleAppID = "AIzaSyBlcyCnxYJNfk5mF4sfzcOt6q2j5S_wQRI";
			//var SENDER_ID = "89910554410";
			var value = message;
			WebRequest tRequest;
			tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
			tRequest.Method = "post";
			tRequest.ContentType = " application/json";
			tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
			//tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
				
			string postData = "{"
					+ "\"registration_id\":\"" + deviceId + "\"" + ","
					+"\"data\":{"
						+ "\"time\":\"" 
						+ System.DateTime.Now.ToString() + "\"" + ","
						+ "\"message\":\""
						+ value + "\"" + ","
						+ "\"status\":\""
						+ "assigned" + "\""
					+"}"
				+"}";
			Console.WriteLine(postData);
			Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
			tRequest.ContentLength = byteArray.Length;
		
			Stream dataStream = tRequest.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();
	
			WebResponse tResponse = tRequest.GetResponse();
	
			dataStream = tResponse.GetResponseStream();
	
			StreamReader tReader = new StreamReader(dataStream);
	
			String sResponseFromServer = tReader.ReadToEnd();
	
			tReader.Close();
			dataStream.Close();
			tResponse.Close();
			return sResponseFromServer;
		}
	}
}

