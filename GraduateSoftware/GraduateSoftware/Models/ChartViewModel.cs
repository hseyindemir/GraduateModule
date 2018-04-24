using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraduateSoftware.Models
{
    public class ChartViewModel
    {

        [JsonProperty("WorkArea")]
        public string WorkAreaName { get; set; }
        [JsonProperty("CountRate")]
        public string Count { get; set; }

        [JsonProperty("dataTable")]
        public VisualizationDataTable DataTable { get; set; }

    }
}