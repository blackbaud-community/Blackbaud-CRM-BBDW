using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blackbaud.OpenFx.ODataSample.Web
{
    public class ODataHelper
    {
        //This URL must be updated with the URL to the ODataQuery.ashx endpoint and include the databasename QueryString
        private string _rootURL = "http://mywebserver/bbappfx/ODataQuery.ashx?databasename=BBInfinity&";

        public string GetODataAdHocQueryURI(System.Guid queryID)
        {

            var oDataURL = string.Concat(_rootURL, "AdHocQueryID=",queryID.ToString());

            return oDataURL;
        }

        public string GetODataMetaDataAdHocQueryURI(System.Guid queryID)
        {

            var oDataURL = string.Concat(_rootURL, "AdHocQueryID=", queryID.ToString(),"&mode=metadata");

            return oDataURL;
        }

        public string GetODataSmartQueryURI(System.Guid queryID)
        {
            var oDataURL = string.Concat(_rootURL, "SmartQueryInstanceID=", queryID.ToString());
            
            return oDataURL;
        }

        public string GetODataMetaDataSmartQueryURI(System.Guid queryID)
        {
            var oDataURL = string.Concat(_rootURL, "SmartQueryInstanceID=", queryID.ToString(), "&mode=metadata");

            return oDataURL;
        }


    }
}