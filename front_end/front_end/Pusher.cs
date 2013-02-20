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
			string Id2 = "APA91bFhnzVwJ6bKx_GT2BysxMrH4IyCa6je0w2rJ12tpO8RWl2KOhiR6vBrVC7g0JKcrJw_mAPKCXmseM2bgPG5W88-NIfayPIsypD2y44H9IH7ut4mZBs6FGkUeSNWcyD76xfznQckTIHkQtng6cdL43I1pxBd9w";
			string Id3 = "APA91bFPolfZFHVGV5NgAftyN_rqFcbswi7N9DRHNmsAcG7T6zuUCq7bgH42-UIUbStPS41jWH15YZPdtoHs6W9l_YZBdAV5eS_J_n2TOy-7jj90wCn2DdOGNrZ6yaDjvtafBpmNuLHv2-TsRgLmUYESxXi14Ta0BQ";
			string Id4 = "APA91bEshtaAMt7lsOG428cLEm0oDL4vQwqMlDr5WkN4eGBi6V-6RrGnNYRRacwpvkqLmw5xfZoLXOxn9oPcDnvJo-IN8xzpjyuPfZcvgCWycw79lDBa06IF_2qSsNnORok0iotdsCD1cqrLrtP7KiEKsHsx3sDj1w";
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
					+ "\"registration_ids\":[" + "\"" + deviceId + "\"" + ","
						+ "\"" + Id2 + "\"" + ","
						+ "\"" + Id3 + "\"" + ","
						+ "\"" + Id4 + "\""
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

