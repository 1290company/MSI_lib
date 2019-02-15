using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MSI_lib
{
    public class TableFormula
    {
        public int PortalID { get; set; }

        public class CopyData_Param
        {
            public string SourceRouteNo { set; get; }
            public string TargetRouteNo { set; get; }
            public string LoginUser { set; get; }
        }

        /// <summary>
        /// TableFormula
        /// </summary>
        /// <param name="_PortalID">PortalID</param>
        public TableFormula(int _PortalID)
        {
            PortalID = _PortalID;
        }

        public bool CopyData(CopyData_Param c)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SqlTransaction trans = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = trans;

                try
                {
                    //get TableFormula
                    cmd.CommandText = "select * from DW_TableFormula where TableName like @TableName or TargetTable like @TargetTable";
                    List<SqlParameter> arParms = new List<SqlParameter>();
                    arParms.Add(new SqlParameter() { ParameterName = "@TableName", SqlDbType = SqlDbType.NVarChar, Value = "%" + c.SourceRouteNo + "%" });
                    arParms.Add(new SqlParameter() { ParameterName = "@TargetTable", SqlDbType = SqlDbType.NVarChar, Value = "%" + c.SourceRouteNo + "%" });
                    cmd.Parameters.AddRange(arParms.ToArray());
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string TableName = "";
                        string TargetTable = "";
                        string TargetField = "";
                        string Operator = "";
                        string FormulaValue = "";
                        string FormulaField = "";
                        string FormulaFunction = "";
                        int Type = 0;
                        string GroupName = "";
                        int Existed = 0;
                        int EventType = 0;
                        string Message = "";

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableName = dt.Rows[i]["TableName"] == DBNull.Value ? "" : dt.Rows[i]["TableName"].ToString();
                            TargetTable = dt.Rows[i]["TargetTable"] == DBNull.Value ? "" : dt.Rows[i]["TargetTable"].ToString();
                            TargetField = dt.Rows[i]["TargetField"] == DBNull.Value ? "" : dt.Rows[i]["TargetField"].ToString();
                            Operator = dt.Rows[i]["Operator"] == DBNull.Value ? "" : dt.Rows[i]["Operator"].ToString();
                            FormulaValue = dt.Rows[i]["FormulaValue"] == DBNull.Value ? "" : dt.Rows[i]["FormulaValue"].ToString();
                            FormulaField = dt.Rows[i]["FormulaField"] == DBNull.Value ? "" : dt.Rows[i]["FormulaField"].ToString();
                            FormulaFunction = dt.Rows[i]["FormulaFunction"] == DBNull.Value ? "" : dt.Rows[i]["FormulaFunction"].ToString();
                            Type = dt.Rows[i]["Type"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Type"]);
                            GroupName = dt.Rows[i]["GroupName"] == DBNull.Value ? "" : dt.Rows[i]["GroupName"].ToString();
                            Existed = dt.Rows[i]["Existed"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Existed"]);
                            EventType = dt.Rows[i]["EventType"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["EventType"]);
                            Message = dt.Rows[i]["Message"] == DBNull.Value ? "" : dt.Rows[i]["Message"].ToString();

                            if (!TableName.Equals(c.SourceRouteNo) && !TableName.StartsWith(c.SourceRouteNo + "_ST"))
                                continue;
                            else
                                TableName = TableName.Replace(c.SourceRouteNo, c.TargetRouteNo);

                            if (TargetTable.Equals(c.SourceRouteNo) || TargetTable.StartsWith(c.SourceRouteNo + "_ST"))
                                TargetTable = TargetTable.Replace(c.SourceRouteNo, c.TargetRouteNo);

                            //insert
                            cmd.CommandText = @"
                                INSERT INTO [dbo].[DW_TableFormula]
                                    ([sys_CreateDate]
                                    ,[sys_CreateUser]
                                    ,[TableName]
                                    ,[TargetTable]
                                    ,[TargetField]
                                    ,[Operator]
                                    ,[FormulaValue]
                                    ,[FormulaField]
                                    ,[FormulaFunction]
                                    ,[Type]
                                    ,[GroupName]
                                    ,[Existed]
                                    ,[EventType]
                                    ,[Message])
                                VALUES
                                    (getdate()
                                    ,@sys_CreateUser
                                    ,@TableName
                                    ,@TargetTable
                                    ,@TargetField
                                    ,@Operator
                                    ,@FormulaValue
                                    ,@FormulaField
                                    ,@FormulaFunction
                                    ,@Type
                                    ,@GroupName
                                    ,@Existed
                                    ,@EventType
                                    ,@Message)";
                            arParms = new List<SqlParameter>();
                            arParms.Add(new SqlParameter() { ParameterName = "@sys_CreateUser", SqlDbType = SqlDbType.NVarChar, Value = c.LoginUser });
                            arParms.Add(new SqlParameter() { ParameterName = "@TableName", SqlDbType = SqlDbType.NVarChar, Value = TableName });
                            arParms.Add(new SqlParameter() { ParameterName = "@TargetTable", SqlDbType = SqlDbType.NVarChar, Value = TargetTable });
                            arParms.Add(new SqlParameter() { ParameterName = "@TargetField", SqlDbType = SqlDbType.NVarChar, Value = TargetField });
                            arParms.Add(new SqlParameter() { ParameterName = "@Operator", SqlDbType = SqlDbType.NVarChar, Value = Operator });
                            arParms.Add(new SqlParameter() { ParameterName = "@FormulaValue", SqlDbType = SqlDbType.NVarChar, Value = FormulaValue });
                            arParms.Add(new SqlParameter() { ParameterName = "@FormulaField", SqlDbType = SqlDbType.NVarChar, Value = FormulaField });
                            arParms.Add(new SqlParameter() { ParameterName = "@FormulaFunction", SqlDbType = SqlDbType.NVarChar, Value = FormulaFunction });
                            arParms.Add(new SqlParameter() { ParameterName = "@Type", SqlDbType = SqlDbType.Int, Value = Type });
                            arParms.Add(new SqlParameter() { ParameterName = "@GroupName", SqlDbType = SqlDbType.NVarChar, Value = GroupName });
                            arParms.Add(new SqlParameter() { ParameterName = "@Existed", SqlDbType = SqlDbType.Bit, Value = Existed });
                            arParms.Add(new SqlParameter() { ParameterName = "@EventType", SqlDbType = SqlDbType.Int, Value = EventType });
                            arParms.Add(new SqlParameter() { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Value = Message });
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(arParms.ToArray());
                            int result = cmd.ExecuteNonQuery();
                            if (result == 0)
                            {
                                trans.Rollback();
                                cmd.Dispose();

                                return false;
                            }
                        }

                        trans.Commit();
                        cmd.Dispose();

                        return true;
                    }
                    else
                    {
                        cmd.Dispose();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    var objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    objEventLog.AddLog("Log Message", ex.ToString(), DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);
                    return false;
                }
            }
        }
    }
}
