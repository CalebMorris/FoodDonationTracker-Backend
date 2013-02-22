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
			string GoogleAppID = "AIzaSyBlcyCnxYJNfk5mF4sfzcOt6q2j5S_wQRI";
			var value = message;
			WebRequest tRequest;
			tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
			tRequest.Method = "post";
			tRequest.ContentType = " application/json";
			tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
			//tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
				
			string postData = "{"
					+ "\"registration_ids\":[" + "\"" + deviceId + "\""
					+ "]"
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

