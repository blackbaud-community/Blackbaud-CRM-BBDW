using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blackbaud.OpenFx.ODataSample.Web.DAL;
//using System.Web.Helpers;
using System.Collections;
using Blackbaud.OpenFx.ODataSample.Web.Models;
using System.Web.Routing;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace Blackbaud.OpenFx.ODataSample.Web.Controllers
{
    public class TotalGivenByYearController : Controller
    {
        // GET: TotalGiven
        public ActionResult Index()
        {

            //Read list of smart query instance IDs from XML file
            string xmlData = HttpContext.Server.MapPath("~/App_Data/Queries.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);

            //Loop through query IDs and add them to list
            var queries = new List<Query>();
            queries = (from rows in ds.Tables[0].AsEnumerable()
                       select new Query
                       {
                           ID = new Guid(rows[0].ToString()),
                           Caption = rows[1].ToString(),
                           Type = rows[2].ToString()
                       }
                           ).ToList();

            //Pass query list to ViewData
            ViewData["Queries"] = queries;

            return View();
        }

        public ActionResult DrawChart(Guid queryID, String caption)
        {

            //Create the chart object
            var chart = new System.Web.UI.DataVisualization.Charting.Chart();

            chart.Width = 300;
            chart.Height = 200;

            var chartSeries = new Series();

            chartSeries.ChartType = SeriesChartType.Bar;

            var givingYears = new TotalGivenByYearDataAccess();

            //Make the request to the OData feed and return the results
            var totalGiven = givingYears.GetTotalGivenByYear(queryID);

            //Loop through the results and add them to the chart
            foreach (GivingYear year in totalGiven.GivingYears)
            {
                chartSeries.Points.AddXY(year.CalendarYear, year.TotalGiven);
            }

            chart.Series.Add(chartSeries);
            
            var area = new ChartArea();

            area.AxisY.LabelStyle.Format = "{C}";

            chart.Titles.Add(caption);

            chart.ChartAreas.Add(area);
            
            //Create a png file with the chart image to return to the view
            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms.ToArray(), "image/png", queryID.ToString());
            }

        }

    }
}