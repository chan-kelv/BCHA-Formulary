using System;
using RestSharp;
using Connectivity.Plugin;

namespace BCHAFormulary
{
	public class WebHelper
	{
		public WebHelper ()
		{
		}

		public string webGet(string uri){
			var responseBody = string.Empty;
			var client = new RestClient (uri);
			var request = new RestRequest(Method.GET);
//			client.ExecuteAsync(request, response=> {
//				if(response.ErrorException != null){
//					Console.WriteLine("Error in GET due to {0}", response.ErrorMessage);
//					responseBody = response.Content;
//				}
//
//				responseBody = response.Content;
//			});
			var response = client.Execute(request);
			responseBody = response.Content;
			return responseBody;
		}

		public bool isConnected(){
			var connectionManager = CrossConnectivity.Current;
			return (connectionManager.IsConnected);
		}
	}

	public static class Uri
	{
		public const string dropboxRoot = "https://www.dropbox.com/";

		//TODO change these back to official ones before release
		public const string updateEndpoint = dropboxRoot + "s/4cvo08xnmlg7qr6/update.txt?dl=1";

		public const string formularyEndpoint = dropboxRoot + "s/3qdfzzfeucp83nt/formulary.csv?dl=1";
		public const string excludedEndpoint = dropboxRoot + "s/lj6ucd9o7u1og3k/excluded.csv?dl=1";
		public const string restrictedEndpoint = dropboxRoot + "s/n4so74xl4n7wbhy/restricted.csv?dl=1";

	}
}

