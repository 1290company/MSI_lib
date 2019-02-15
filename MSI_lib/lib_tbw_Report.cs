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
    public class lib_tbw_Report
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
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
            public string sys_createuser { set; get; }
            public string sys_UpdateUser { set; get; }

        }
        public class Dw1_Setting
        {
            public int RecordCount { set; get; }
            public Int32 sysID { set; get; }
            public String RouteNo { set; get; }
            public String MappingName { set; get; }
            public String SortingNo { set; get; }
            public String DisplayName { set; get; }
            public String FieldType { set; get; }
            public String PortalName { set; get; }
            public String Field { set; get; }
            public DateTime sysCreateDate { set; get; }
            public String sysCreateUser { set; get; }
            public DateTime sysUpdateDate { set; get; }
            public String sysUpdateUser { set; get; }
        }

        public class Dw_Route_ST_FIELD
        {
            public String RouteNo{set;get;}
            public String MappingName{set;get;}
            public String TableName{set;get;}
            public String Field{set;get;}
            public String FieldProperty{set;get;}
            public String StationNo{set;get;}
            public String PK{set;get;}
            public DateTime CreateDate{set;get;}
            public String CreateUser{set;get;}
            public String Deleted{set;get;}
            public Int32 SortingNo{set;get;}
        }

        public void Tbw_After(List<Dw_Route_ST_FIELD> Route_ST, List<Dw1_Setting> Dw1_Setting, Dw_1_report Report, string status)
        {
            try
            {
                List<string> Mapping = new List<string>();
                if (status.ToLower()=="new")
                {
                    Mapping =  GetDw1_SettingMapping(Report.TableName);
                }
                dw_1_reportInsert_forDW1(Route_ST, Dw1_Setting, Report, status, Mapping);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.ToString());
            }
        }

       
        public  Int32 dw_1_reportInsert_forDW1(List<Dw_Route_ST_FIELD> Route_ST,List<Dw1_Setting> Dw1_Setting, Dw_1_report Report,string Status,List<string> Mapping)
        {
            string Sql = "";
            Sql = "INSERT INTO dw_1_report(Condition,ReportType,TableName,RouteNo,MappingName,sys_createdate,sys_createuser,sys_updatedate,sys_updateuser,report_name,report_desc,show_search,PortalID,JsonString) VALUES (@Condition,@ReportType,@TableName,@RouteNo,@MappingName,Getdate(),@sys_createuser,Getdate(),@sys_updateuser,@report_name,@report_desc,@show_search,@PortalID,@JsonString);select @@identity;";
            int NewId = 0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("DW1Report");
                command.Connection = conn;
                command.Transaction = transaction;


                command.CommandText = @"DELETE FROM DW_ROUTE_ST_FIELD WHERE  TableName =@TableName;
                                        DELETE FROM DW1_SETTING WHERE  RouteNo=@RouteNo AND MappingName = @MappingName";
                command.Parameters.AddWithValue("@TableName", Route_ST[0].TableName);
                command.Parameters.AddWithValue("@RouteNo", Route_ST[0].RouteNo);
                command.Parameters.AddWithValue("@MappingName", Route_ST[0].MappingName);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                
                try
                {
                    if (Status.ToLower() == "new")
                    {
                        command.CommandText = Sql;
                        command.Parameters.AddWithValue("@sys_createuser", Report.sys_createuser);
                        command.Parameters.AddWithValue("@sys_updateuser", Report.sys_UpdateUser);
                        command.Parameters.AddWithValue("@report_name", Report.reportName);
                        command.Parameters.AddWithValue("@report_desc", Report.report_desc);
                        command.Parameters.AddWithValue("@show_search", Report.show_search);
                        command.Parameters.AddWithValue("@MappingName", Report.MappingName);
                        command.Parameters.AddWithValue("@RouteNo", Report.RouteNo);
                        command.Parameters.AddWithValue("@TableName", Report.TableName);
                        command.Parameters.AddWithValue("@ReportType", Report.ReportType);
                        command.Parameters.AddWithValue("@Condition", Report.Condition);
                        command.Parameters.AddWithValue("@PortalID", Report.PortalID);
                        command.Parameters.AddWithValue("@JsonString", Report.JsonString);
                        object id = command.ExecuteScalar();
                        command.Parameters.Clear();
                    }

                    Sql = "INSERT INTO dw1_setting(RouteNo,MappingName,SortingNo,DisplayName,FieldType,PortalName,Field,sys_CreateDate,sys_CreateUser,sys_UpdateDate,sys_UpdateUser,ReportName) VALUES ( @RouteNo,@MappingName,@SortingNo,@DisplayName,@FieldType,@PortalName,@Field,GetDate(),@sys_CreateUser,GetDate(),@sys_UpdateUser,@ReportName)";
                    foreach (var o in Dw1_Setting)
                    {

                        command.Parameters.AddWithValue("@sys_createuser", o.sysCreateUser);
                        command.Parameters.AddWithValue("@sys_updateuser", o.sysUpdateUser);

                        command.Parameters.AddWithValue("@RouteNo", o.RouteNo);
                        command.Parameters.AddWithValue("@MappingName", o.MappingName);
                        command.Parameters.AddWithValue("@SortingNo", o.SortingNo);
                        command.Parameters.AddWithValue("@DisplayName", o.DisplayName);
                        command.Parameters.AddWithValue("@FieldType", o.FieldType);
                        command.Parameters.AddWithValue("@PortalName", o.PortalName);
                        command.Parameters.AddWithValue("@Field", o.Field);
                        command.Parameters.AddWithValue("@ReportName", Report.reportName);
                        command.CommandText = Sql;
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    Sql = @"
insert into DW_ROUTE_ST_FIELD (MappingName,TableName,Field,RouteNo,FieldProperty,StationNo,PK,CreateDate,CreateUser,Deleted,SortingNo) 
VALUES ( @MappingName,@TableName,@Field,@RouteNo,@FieldProperty,@StationNo,@PK,GetDate(),@CreateUser,@Deleted,@SortingNo)";
                    if (Mapping.Count == 0)
                    {
                        foreach (var o in Route_ST)
                        {
                            command.Parameters.AddWithValue("@RouteNo", o.RouteNo);
                            command.Parameters.AddWithValue("@MappingName", o.MappingName);
                            command.Parameters.AddWithValue("@TableName", o.TableName);
                            command.Parameters.AddWithValue("@Field", o.Field);
                            command.Parameters.AddWithValue("@FieldProperty", o.FieldProperty);
                            command.Parameters.AddWithValue("@StationNo", o.StationNo);
                            command.Parameters.AddWithValue("@PK", o.PK);
                            command.Parameters.AddWithValue("@CreateUser", o.CreateUser);
                            command.Parameters.AddWithValue("@Deleted", o.Deleted);
                            command.Parameters.AddWithValue("@SortingNo", o.SortingNo);

                            command.CommandText = Sql;
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Mapping.Count; i++)
                        {
                            foreach (var o in Route_ST)
                            {
                                command.Parameters.AddWithValue("@RouteNo", o.RouteNo);
                                command.Parameters.AddWithValue("@MappingName", Mapping[i]);
                                command.Parameters.AddWithValue("@TableName", o.TableName);
                                command.Parameters.AddWithValue("@Field", o.Field);
                                command.Parameters.AddWithValue("@FieldProperty", o.FieldProperty);
                                command.Parameters.AddWithValue("@StationNo", o.StationNo);
                                command.Parameters.AddWithValue("@PK", o.PK);
                                command.Parameters.AddWithValue("@CreateUser", o.CreateUser);
                                command.Parameters.AddWithValue("@Deleted", o.Deleted);
                                command.Parameters.AddWithValue("@SortingNo", o.SortingNo);

                                command.CommandText = Sql;
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
                        }
                    }


                    transaction.Commit();
                    NewId = 0;
                    command.Dispose();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    NewId = -1;
                    throw (ex);
                }
            }
            return NewId;
        }
        public void createdw_1_report(Dw_1_report Dw1_report)
        {
            try
            {
                string Sql_Insert = @"INSERT INTO [DW_1_Report]
           ([sys_createdate],[sys_createuser],[sys_updatedate],[sys_updateuser],[report_name],[report_desc],[show_search],[RouteNo],[MappingName],[TableName],[ReportType],[Condition],[JsonString],[PortalID])
     VALUES
           (getdate(),'host',getdate(),'host',@report_name,@report_desc,@show_search,@RouteNo,@MappingName,@TableName,@ReportType,@Condition,@JsonString,@PortalID)";
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

                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Sql_Insert, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        

        public List<string> GetDw1_SettingMapping(string TableName)
        {
            List<string> Mapping = new List<string>();

            string SQL = "select mappingName from DW_ROUTE_ST_FIELD where TableName = @TableName group by mappingname";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
            sp1.Value = TableName;
            lstP.Add(sp1);


            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Mapping.Add(ds.Tables[0].Rows[i]["mappingName"].ToString());
            }

            return Mapping;
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

        public List<Dw1_Setting> GetDW1_setting(string RouteNo, string MappingName)
        {
            List<Dw1_Setting> list = new List<Dw1_Setting>();
            string SQL = "select * from dw1_setting where RouteNo = @RouteNo AND MappingName = @MappingName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = RouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = MappingName;
            lstP.Add(sp1);

            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            Dw1_Setting setting = null;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                setting = new Dw1_Setting()
                {
                    MappingName = dr["MappingName"] == System.DBNull.Value ? "" : dr["MappingName"].ToString(),
                    RouteNo = dr["RouteNo"] == System.DBNull.Value ? "" : dr["RouteNo"].ToString(),
                    SortingNo = dr["SortingNo"] == System.DBNull.Value ? "" : dr["SortingNo"].ToString(),
                    DisplayName = dr["DisplayName"] == System.DBNull.Value ? "" : dr["DisplayName"].ToString(),
                    FieldType = dr["FieldType"] == System.DBNull.Value ? "" : dr["FieldType"].ToString(),
                    Field = dr["Field"] == System.DBNull.Value ? "" : dr["Field"].ToString(),
                    PortalName = dr["PortalName"] == System.DBNull.Value ? "" : dr["PortalName"].ToString()
                };

                list.Add(setting);
            }

            return list;
        }
    }
 

}
