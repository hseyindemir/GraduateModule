using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraduateSoftware.Models;
using Newtonsoft.Json;

namespace GraduateSoftware.Controllers
{
    public class InformationController : Controller
    {
        //Work Area Graph
        public ActionResult Index()
        {
          
            string WorkAreaCategories = "SELECT A.WorkAreaName Alan, COUNT(G.StudentID) as CountRate FROM WorkArea A LEFT JOIN Graduates G ON A.WAID = G.WorkAreaID GROUP BY A.WorkAreaName ORDER BY COUNT(G.StudentID) desc";
            string constr = ConfigurationManager.ConnectionStrings["GraduateModuleEntities"].ConnectionString;
            string ConnectionString = "data source=localhost;initial catalog=GraduateModule;integrated security=True;multipleactiveresultsets=True;";
            List<WorkAreaCountModel> chartData = new List<WorkAreaCountModel>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(WorkAreaCategories))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new WorkAreaCountModel
                            {
                                WorkAreaName = sdr["Alan"].ToString(),
                                CountRate = Convert.ToInt32(sdr["CountRate"])
                            });
                        }
                    }

                    con.Close();
                }
            }

            ViewBag.myData = chartData;

            return View(chartData);

            
        }
        public ActionResult getMapData()
        {
            SqlConnection cn = new SqlConnection("data source = localhost; initial catalog = GraduateModule; integrated security = True; multipleactiveresultsets = True;");
            SqlCommand cmd = new SqlCommand("SELECT A.WorkAreaName Alan, COUNT(G.StudentID) as CountRate FROM WorkArea A LEFT JOIN Graduates G ON A.WAID = G.WorkAreaID GROUP BY A.WorkAreaName ORDER BY COUNT(G.StudentID) desc", cn);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable firstTable = ds.Tables[0];
            List<WorkAreaCountModel> Records = new List<WorkAreaCountModel>();

            var dt = new VisualizationDataTable();
            dt.AddColumn("Alan", "string");
            dt.AddColumn("CountRate", "number");

            for (int i = 0; i < firstTable.Rows.Count; i++)
            {

                dt.NewRow(firstTable.Rows[i]["Alan"].ToString(), Convert.ToInt32(firstTable.Rows[i]["CountRate"]));

            }

            var chart = new ChartViewModel
            {
                WorkAreaName = "Records",
                Count = "Per Machine",
                DataTable = dt
            };
            return Content(JsonConvert.SerializeObject(chart), "application/json");

        }

    }
}