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
    public class lib_Condtion
    {
        public class Item
        {
            public string Field { get; set; }
            public string FieldType { get; set; }
            public string Symbol { get; set; }
            public string Value { get; set; }
            public string Value2 { get; set; }
        }

        public class ConditionObject
        {
            public List<Item> data { get; set; }
        }

        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        public class Dw_Condition
        {
            public Int32 sys_id { set; get; }
            public String sys_createuser { set; get; }
            public DateTime sys_createdate { set; get; }
            public String sys_updateuser { set; get; }
            public DateTime sys_updatedate { set; get; }
            public String ConditionName { set; get; }
            public String ConditionJson { set; get; }
            public String RouteNo { set; get; }
            public String MappingName { set; get; }
            public String TableName { set; get; }
            public Int32 Status { set; get; }
        }

        public void createDw_Condition(Dw_Condition Dw1_report)
        {
            try
            {
                string Sql_Insert = @"INSERT INTO [dbo].[DW_Condition]
           ([sys_createuser],[sys_createdate],[sys_updateuser],[sys_updatedate],[ConditionName],[ConditionJson],[RouteNo],[MappingName],[TableName],[Status])
     VALUES
           ('host',getdate(),'host',getdate(),@ConditionName,@ConditionJson,@RouteNo,@MappingName,@TableName,1)";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@ConditionName", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.ConditionName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@ConditionJson", SqlDbType.NVarChar);
                sp1.Value = Dw1_report.ConditionJson;
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

                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Sql_Insert, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public Dw_Condition GetDw_Condition(string ConditionName)
        {

            string SQL = "select * from Dw_Condition where ConditionName = @ConditionName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@ConditionName", SqlDbType.NVarChar);
            sp1.Value = ConditionName;
            lstP.Add(sp1);

            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            //Dw_1_report d = new Dw_1_report();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int s = 0;
                int.TryParse(dr["Status"].ToString(), out s);

                Dw_Condition o = new Dw_Condition()
                {

                    MappingName = dr["MappingName"] == System.DBNull.Value ? "" : dr["MappingName"].ToString(),
                    RouteNo = dr["RouteNo"] == System.DBNull.Value ? "" : dr["RouteNo"].ToString(),
                    TableName = dr["TableName"] == System.DBNull.Value ? "" : dr["TableName"].ToString(),
                    ConditionName = dr["ConditionName"] == System.DBNull.Value ? "" : dr["ConditionName"].ToString(),
                    ConditionJson = dr["ConditionJson"] == System.DBNull.Value ? "" : dr["ConditionJson"].ToString(),
                    Status = dr["Status"] == System.DBNull.Value ? 0 : s

                };

                return o;
            }
            return null;
        }
        
    }
}
