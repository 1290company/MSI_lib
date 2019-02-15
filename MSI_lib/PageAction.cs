using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MSI_lib
{
    public class PageAction : TableWidget
    {
        /// <summary>
        /// PageAction
        /// </summary>
        /// <param name="_PortalID">PortalID</param>
        public PageAction(int _PortalID) : base(_PortalID)
        {
            PortalID = _PortalID;
        }

        public override bool CopyData(CopyData_Param c)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;

                //get PortalName
                string sql = "select PortalName from PortalLocalization where PortalID = " + PortalID;
                string portalName = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sql).ToString();

                //get PageAction
                sql = string.Format("select top 1 * from {0}_PageAction where ActionName = @ActionName", portalName);
                SqlParameter[] arParms = new SqlParameter[1];
                arParms[0] = new SqlParameter("@ActionName", SqlDbType.NVarChar);
                arParms[0].Value = c.SourceActionName;
                SqlDataReader dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, arParms);
                DataTable dt = new DataTable();
                dt.Load(dr);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //開始取代 RouteNo
                    string jsonStr = dt.Rows[0]["JSONString"].ToString();
                    var d = ConvertDeserialize(jsonStr);
                    var _d = ReplaceRoute(d, c);

                    //重組 SQL
                    string sqlStr = ConvertSql(_d);

                    //
                    sql = string.Format(@"
                        INSERT INTO {0}_PageAction
                            ([sys_CreateDate]
                            ,[sys_CreateUser]
                            ,[ActionName]
                            ,[ActionSQL]
                            ,[JSONString])
                        VALUES
                            (getdate()
                            ,@sys_CreateUser
                            ,@ActionName
                            ,@ActionSQL
                            ,@JSONString)", portalName);
                    arParms = new SqlParameter[4];
                    arParms[0] = new SqlParameter("@sys_CreateUser", SqlDbType.NVarChar);
                    arParms[0].Value = c.LoginUser;
                    arParms[1] = new SqlParameter("@ActionName", SqlDbType.NVarChar);
                    arParms[1].Value = c.TargetActionName;
                    arParms[2] = new SqlParameter("@ActionSQL", SqlDbType.NVarChar);
                    arParms[2].Value = sqlStr;
                    arParms[3] = new SqlParameter("@JSONString", SqlDbType.NVarChar);
                    arParms[3].Value = ConvertSerialize(_d);
                    int result = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, arParms);
                    return result > 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                var objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("Log Message", ex.ToString(), DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);
                return false;
            }
        }
    }
}
