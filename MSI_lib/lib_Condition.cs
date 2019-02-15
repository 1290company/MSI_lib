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
   public class lib_Condition
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;

        public class dw_Condition
        {
            public string ConditionName { set; get; }
            public string ConditionJson { set; get; }
            public string RouteNo { set; get; }
            public string MappingName { set; get; }
            public string TableName { set; get; }
            public string Status { set; get; }
        }
        public void CreateNewCondition(dw_Condition Condition)
        {
            try
            {
                CreateCondition(Condition);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void DeleteCondition(dw_Condition Condition)
        {
            try
            {
                string Sql_Delete = @"
update DW_Condition set Status=0 where ConditionName=@ConditionName and RouteNo = @RouteNo and MappingName=@MappingName
";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@ConditionName", SqlDbType.NVarChar);
                sp1.Value = Condition.ConditionName;
                lstP.Add(sp1);
                 sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = Condition.RouteNo;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                sp1.Value = Condition.MappingName;
                lstP.Add(sp1);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Sql_Delete, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
        public void CreateCondition(dw_Condition Condition)
        {
            try
            {
                string Sql_Insert = @"
insert into dw_condition (sys_createdate,sys_createuser,sys_updatedate,sys_updateuser,ConditionName,ConditionJson,RouteNo,MappingName,TableName,Status)
values 
(GETDATE(),'Host',GETDATE(),'Host',@ConditionName,@ConditionJson,@RouteNo,@MappingName,@TableName,1)";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@ConditionName", SqlDbType.NVarChar);
                sp1.Value = Condition.ConditionName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@ConditionJson", SqlDbType.NVarChar);
                sp1.Value = Condition.ConditionJson;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = Condition.RouteNo;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                sp1.Value = Condition.MappingName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                sp1.Value = Condition.TableName;
                lstP.Add(sp1);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Sql_Insert, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
        public dw_Condition GetCondition(string ConditionName)
        {
            string SQL = "select * from DW_Condition where ConditionName = @ConditionName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@ConditionName", SqlDbType.NVarChar);
            sp1.Value = ConditionName;
            lstP.Add(sp1);


            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            //Dw_1_report d = new Dw_1_report();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dw_Condition o = new dw_Condition()
                {
                    MappingName = dr["MappingName"] == System.DBNull.Value ? "" : dr["MappingName"].ToString(),
                    RouteNo = dr["RouteNo"] == System.DBNull.Value ? "" : dr["RouteNo"].ToString(),
                    TableName = dr["TableName"] == System.DBNull.Value ? "" : dr["TableName"].ToString(),
                    ConditionName = dr["ConditionName"] == System.DBNull.Value ? "" : dr["ConditionName"].ToString(),
                    ConditionJson = dr["ConditionJson"] == System.DBNull.Value ? "" : dr["ConditionJson"].ToString(),
                    Status = dr["Status"] == System.DBNull.Value ? "" : dr["Status"].ToString(),
                };

                return o;
            }
            return null;


        }

    }
}
