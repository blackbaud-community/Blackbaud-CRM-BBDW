using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blackbaud.OpenFx.ODataSample.Web.Models
{
    public class TotalGivenByYear
    {
        public List<GivingYear> GivingYears;
    }

    public class GivingYear
    {
        public Int16 CalendarYear { get; set; }
        public Decimal TotalGiven { get; set; }
        public String TotalGivenFormatted { get; set; }
    }
}