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
  public  class lib_Route_Permission
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        public  bool ReadRouteCheck(string MappingName, string UserName)
        {
            bool Status = false;
            string sql = "select * from dw_Route_Permission where MappingName = @MappingName and UserName = @UserName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = MappingName;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@UserName", SqlDbType.NVarChar);
            sp1.Value = UserName;
            lstP.Add(sp1);
             DataSet ds =   Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql, lstP.ToArray());
            if (ds.Tables[0].Rows.Count>0)
            {
                Status = true;
            }
            return Status;
        }
        public bool RouteCheck(string RouteNo, string UserName)
        {
            bool Status = false;
            string sql = "select * from dw_Route_Permission where RouteNo = @RouteNo and UserName = @UserName";

            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = RouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@UserName", SqlDbType.NVarChar);
            sp1.Value = UserName;
            lstP.Add(sp1);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql, lstP.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                Status = true;
            }
            return Status;
        }
        public  DataTable GetFieldMasks(string MappingName)
        {

            //@更新
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@MappingName", MappingName));
            string Sql = @"
select
 a.RouteNo, a.TableName, a.Field,a.MappingName, b.DisplayName, b.HiddenCode
from 
 v_dw_route_st_field a inner join
 DW_FIELD b on a.TableName = b.TableName and a.Field = b.Field
where MappingName = @MappingName and b.HiddenCode = 1";


            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset
              (ConnectionString, System.Data.CommandType.Text, Sql, parameters.ToArray());

            return ds.Tables[0];
        }

        public List<String> GetHidenField(string MappingName)
        {

            string sql = "select Field from v_dw_route_st_field Where HiddenCode = 1 AND MappingName =@MappingName";
            string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = MappingName;
            lstP.Add(sp1);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql, lstP.ToArray());
            var q = from c in ds.Tables[0].AsEnumerable() select c.Field<string>("Field");
            return q.ToList();
        }
    }
}
