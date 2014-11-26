using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.OData;
using System.IO;
using System.Net;

namespace Blackbaud.OpenFx.ODataSample.Web.DAL
{ 
public class ODataRequestMessage : IODataRequestMessage
{

    private Dictionary<string, string> _headers = new Dictionary<string, string>();

    //Hard-coding usernames and passwords is very insecure and a terrible idea
    //Please do something better if using in production
    private string _domain = "BLACKBAUDHOST";
    private string _username = "SuperUser001";
    private string _password = "MySecretP@ssword";

    public string GetHeader(string headerName)
    {

        var value = string.Empty;

        if (_headers.TryGetValue(headerName, out value))
        {
            return value;
        }
        else
        {
            return null;
        }

    }

    public Stream GetStream()
	{

		var request = (HttpWebRequest)WebRequest.Create(this.Url);

		foreach (KeyValuePair<string, string> header_loopVariable in _headers) {
			var header = header_loopVariable;
			if (header.Key == "Content-Type") {
				request.ContentType = header.Value;
			} else if (header.Key == "User-Agent") {
				request.UserAgent = header.Value;
			} else if (header.Key == "Accept") {
				request.Accept = header.Value;
			} else {
				request.Headers.Add(header.Key, header.Value);
			}
		}

        var myCredential = new NetworkCredential();
        myCredential.Domain = _domain;
        myCredential.UserName = _username;
        myCredential.Password = _password;

        request.Credentials = myCredential;

		var response = request.GetResponse();

        return response.GetResponseStream(); 
    }

    public IEnumerable<KeyValuePair<string, string>> Headers
    {
        get { return _headers; }
    }

    public string Method { get; set; }


    public void SetHeader(string headerName, string headerValue)
    {
        _headers.Add(headerName, headerValue);
    }

    public Uri Url { get; set; }

}
}