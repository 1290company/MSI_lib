using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
   public class lib_Chart
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        public void createdw_1_report(Dw_1_report Dw1_report)
        {
            try
            {
                string Sql_Insert = @"INSERT INTO [DW_1_Report]
           ([sys_createdate],[sys_createuser],[sys_updatedate],[sys_updateuser],[report_name],[report_desc],[show_search],[RouteNo],[MappingName],[TableName],[ReportType],[Condition],[JsonString],[PortalID])
     VALUES
           (getdate(),@sys_CreateUser,getdate(),@sys_CreateUser,@report_name,@report_desc,@show_search,@RouteNo,@MappingName,@TableName,@ReportType,@Condition,@JsonString,@PortalID)";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@report_name", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.reportName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@report_desc", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.report_desc;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@show_search", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.show_search;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.RouteNo;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.MappingName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.TableName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@ReportType", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.ReportType;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@Condition", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.Condition;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@JsonString", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.JsonString;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@PortalID", SqlDbType.Int);
                sp1.Value = Dw1_report.PortalID;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@sys_CreateUser", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.sys_CreateUser;
                lstP.Add(sp1);

                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Sql_Insert, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public void DeleteDw1_Rpeort(Dw_1_report Dw)
        {
            string sql = @"DELETE FROM DW_1_Report WHERE RouteNo =@RouteNo AND MappingName=@MappingName and TableName =@TableName";
            List<SqlParameter> lstP = new List<SqlParameter>();

            SqlParameter sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = Dw.MappingName;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = Dw.RouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
            sp1.Value = Dw.TableName;
            lstP.Add(sp1);

            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, lstP.ToArray());
        }
        public Dw_1_report GetDW_1_report(string report_name)
        {

            string SQL = "select * from DW_1_Report where report_name = @Report_name";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@Report_name", SqlDbType.NVarChar);
            sp1.Value = report_name;
            lstP.Add(sp1);


            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            //Dw_1_report d = new Dw_1_report();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int s = 0;
                int.TryParse(dr["PortalID"].ToString(), out s);
                Dw_1_report o = new Dw_1_report()
                {
                    MappingName = dr["MappingName"] == System.DBNull.Value ? "" : dr["MappingName"].ToString(),
                    RouteNo = dr["RouteNo"] == System.DBNull.Value ? "" : dr["RouteNo"].ToString(),
                    TableName = dr["TableName"] == System.DBNull.Value ? "" : dr["TableName"].ToString(),
                    reportName = dr["report_name"] == System.DBNull.Value ? "" : dr["report_name"].ToString(),
                    report_desc = dr["report_desc"] == System.DBNull.Value ? "" : dr["report_desc"].ToString(),
                    show_search = dr["show_search"] == System.DBNull.Value ? "" : dr["show_search"].ToString(),
                    ReportType = dr["ReportType"] == System.DBNull.Value ? "" : dr["ReportType"].ToString(),
                    Condition = dr["Condition"] == System.DBNull.Value ? "" : dr["Condition"].ToString(),
                    JsonString = dr["JsonString"] == System.DBNull.Value ? "" : dr["JsonString"].ToString(),
                    PortalID = dr["PortalID"] == System.DBNull.Value ? 0 : s
                };

                return o;
            }
            return null;
        }
        public class Dw_1_report
        {
            public string reportName { set; get; }
            public string report_desc { set; get; }
            public string show_search { set; get; }
            public string RouteNo { set; get; }
            public string MappingName { set; get; }
            public string TableName { set; get; }
            public string ReportType { set; get; }
            public string Condition { set; get; }
            public string JsonString { set; get; }
            public int PortalID { set; get; }
            public string sys_CreateUser { set; get; }

        }
    }
}
