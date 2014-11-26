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
    public class ODataResponseMessage : IODataResponseMessage
    {
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private Stream _stream;
        private int _statusCode;

        public ODataResponseMessage(Stream stream)
        {
	        _stream = stream;
        }

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
            return _stream;
        }

        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get { return _headers; }
        }

        public void SetHeader(string headerName, string headerValue)
        {
            _headers.Add(headerName, headerValue);
        }

        public int StatusCode
        {
            get { return _statusCode;}

            set { _statusCode = value; }
        }
    }
}