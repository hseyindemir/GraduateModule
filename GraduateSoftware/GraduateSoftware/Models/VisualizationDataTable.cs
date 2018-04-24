using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraduateSoftware.Models
{
    public class VisualizationDataTable
    {
        [JsonProperty("cols")]
        public List<Column> Columns { get; } = new List<Column>();
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; } = new List<Row>();

        public void AddColumn(string label, string type)
        {
            Columns.Add(new Column { Label = label, Type = type });
        }

        public void AddRow(IEnumerable<object> values)
        {
            Rows.Add(new Row(values));
        }

        public VisualizationDataTable NewRow(params object[] values)
        {
            Rows.Add(new Row(values));
            return this;
        }

        public class Column
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("label")]
            public string Label { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public struct Row
        {
            public Row(IEnumerable<object> values)
            {
                Values = values.Select(v => new RowValue(v)).ToArray();
            }

            [JsonProperty("c")]
            private RowValue[] Values { get; }

            private struct RowValue
            {
                public RowValue(object val)
                {
                    Val = val;
                }
                [JsonProperty("v")]
                private object Val { get; }
            }
        }

    }
}