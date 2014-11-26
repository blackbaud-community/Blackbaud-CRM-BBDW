using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Data.OData;
using Blackbaud.OpenFx.ODataSample.Web.Models;

namespace Blackbaud.OpenFx.ODataSample.Web.DAL
{
    public class TotalGivenByYearDataAccess
    {

        public TotalGivenByYear GetTotalGivenByYear(Guid queryID)
        {

            var totalGiven = new TotalGivenByYear();
            totalGiven.GivingYears=new List<GivingYear>();

            var helper = new ODataHelper();

            //Get the necessary OData URIs
            string oDataURI = helper.GetODataSmartQueryURI(queryID);
            string oDataMetaDataURI = helper.GetODataMetaDataSmartQueryURI(queryID);

            //Create a new request message to get the metadata from the OData feed
            var metaDataRequestMessage = new ODataRequestMessage();

            //The content type must always be XML for metadata requests per OData specification
            metaDataRequestMessage.SetHeader("Content-Type", "application/xml");
            metaDataRequestMessage.Url = new Uri(oDataMetaDataURI);

            var readerSettings = new ODataMessageReaderSettings();
            readerSettings.MaxProtocolVersion = ODataVersion.V3;

            var responseMessage = new ODataResponseMessage(metaDataRequestMessage.GetStream());
            responseMessage.SetHeader("Content-Type", "application/xml");

            Microsoft.Data.Edm.IEdmModel model;

            //Create the EDM model based on the response of the metadata request
            using (var messageReader = new ODataMessageReader(responseMessage, readerSettings)) {
	            model = messageReader.ReadMetadataDocument();
                }

            //Create a new request message to actually get the data
            var requestMessage = new ODataRequestMessage();

            //OData requests for data should use a content type of application/json to minimize the payload size
            requestMessage.SetHeader("Content-Type", "application/json");
            requestMessage.Url = new Uri(oDataURI);

            //Create a new ODataMessageReader to read the query results
            using (var messageReader = new ODataMessageReader(requestMessage, readerSettings, model))
            {
                //EntityContainers contain one or more EntitySets
                //The OData feeds from ODataQuery.ashx will always return 
                //one EntityContainer called QueryContainer which contains one EntitySet called QuerySet
                var entityContainer = model.FindDeclaredEntityContainer("QueryContainer");
                var entitySet = entityContainer.FindEntitySet("QuerySet");

                var reader = messageReader.CreateODataFeedReader(entitySet, entitySet.ElementType);

                //Loop through the response
                while (reader.Read()) {
                    switch (reader.State)
                    {

                        case ODataReaderState.EntryEnd:
                            ODataEntry entry = (ODataEntry)reader.Item;
                            
                            var givingYear = new GivingYear();

                            //Loop through the fields (ODataProperties) returned for each query row
                            foreach (ODataProperty prop in entry.Properties)
                            {
                                if (prop.Name.ToUpper() == "CALENDARYEAR")
                                
                                    givingYear.CalendarYear = (Int16)prop.Value;

                                else if (prop.Name.ToUpper() == "TOTALAMOUNT")

                                    givingYear.TotalGiven = (Decimal)prop.Value;
                                    givingYear.TotalGivenFormatted = String.Format("{0:C}", givingYear.TotalGiven);
                            }
                            
                            //Add them to the TotalGivenByYear collection which will be returned to the caller
                            totalGiven.GivingYears.Add(givingYear);

                            break;
                    }
                }

            }

            return totalGiven;
    }
    }
}