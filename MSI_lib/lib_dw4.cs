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
    public class DataField
    {
        public string tablename { set; get; }
        public string MappingName { set; get; }
        public string RouteNo { set; get; }
        public string FieldName { set; get; }
        public string DisplayName { set; get; }
        public string FieldType { set; get; }
        public string FieldSize { set; get; }
        public string PivotColumn { set; get; }
        public string SettingType { set; get; }
        public int GroupingType { set; get; }
        public string OptionDateColumn { set; get; }
        public string OptionTotalColumn { set; get; }
        public string ReportFormat { set; get; }
        public int FloatNum { set; get; }
        public string sortingno { set; get; }
        public int HiddenCode { set; get; }

    }
    public class lib_dw4
    {

        public List<DataField> lstField = new List<DataField>();

        public lib_dw4(List<DataField> lstField)
        {
            try
            {
                DeleteAllSetting(lstField);
                create_dw_field(lstField);
                create_route_st_field(lstField);
                createdw4_setting(lstField);
            }
            catch (Exception ex)
            {

                DeleteAllSetting(lstField);
                throw new Exception(ex.ToString());
            }

        }

        public void DeleteAllSetting(List<DataField> lstField)
        {

            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                string del = "delete from dw_field where TableName = @TableName ; delete from dw_route_st_field where TableName=@TableName and MappingName=@MappingName; ";
                del += " delete from dw_ryg_setting where MappingName=@MappingName and RouteNo=@RouteNo and SettingType='DW4'  ";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                sp1.Value = (lstField.Any()) ? lstField[0].tablename : "";
                lstP.Add(sp1);
                sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                sp1.Value = (lstField.Any()) ? lstField[0].MappingName : "";
                lstP.Add(sp1);
                sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = (lstField.Any()) ? lstField[0].RouteNo : "";
                lstP.Add(sp1);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, del, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public void create_dw_field(List<DataField> lstField)
        {

            try
            {
                foreach (DataField o in lstField)
                {
                    string sql = " insert into dw_field( TableName,Field,DisplayName,FieldType,FieldSize,Status,CreateDate,CreateUser,UpdateDate,UpdateUser,FormType ,HiddenCode) ";
                    sql += " values(@TableName,@Field,@DisplayName,@FieldType,@FieldSize,'1',getdate(),'admin',getdate(),'admin','text' ,@HiddenCode) ";
                    string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                    List<SqlParameter> lstP = new List<SqlParameter>();
                    SqlParameter sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                    sp1.Value = o.tablename;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field", SqlDbType.NVarChar);
                    sp1.Value = o.FieldName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@DisplayName", SqlDbType.NVarChar);
                    sp1.Value = o.DisplayName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@FieldType", SqlDbType.NVarChar);
                    sp1.Value = o.FieldType;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@FieldSize", SqlDbType.Int);
                    sp1.Value = o.FieldSize;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@HiddenCode", SqlDbType.Int);
                    sp1.Value = o.HiddenCode;
                    lstP.Add(sp1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, lstP.ToArray());
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public void create_route_st_field(List<DataField> lstField)
        {
            try
            {
                int i = 1;
                foreach (DataField o in lstField)
                {
                    string sql = " insert into dw_route_st_field(MappingName, TableName, Field, RouteNo, StationNo, FieldProperty, SortingNo, PK, CreateDAte, CreateUser, UpdateDate, UpdateUser) ";
                    sql += " values(@MappingName, @TableName, @Field, @RouteNo, @StationNo, @FieldProperty, @SortingNo, @PK, getdate(), @CreateUser, getdate(), @UpdateUser) ";
                    string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                    List<SqlParameter> lstP = new List<SqlParameter>();
                    SqlParameter sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                    sp1.Value = o.MappingName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                    sp1.Value = o.tablename;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field", SqlDbType.NVarChar);
                    sp1.Value = o.FieldName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp1.Value = o.RouteNo;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@StationNo", SqlDbType.NVarChar);
                    sp1.Value = i.ToString("000");
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@FieldProperty", SqlDbType.Int);
                    sp1.Value = 0;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@PK", SqlDbType.Bit);
                    sp1.Value = 0;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
                    sp1.Value = "admin";
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@UpdateUser", SqlDbType.NVarChar);
                    sp1.Value = "admin";
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@SortingNo", SqlDbType.NVarChar);
                    sp1.Value = "0";
                    lstP.Add(sp1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, lstP.ToArray());
                    i++;
                    var objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    objEventLog.AddLog("Log Message", i + ";" + o.MappingName + ";" + o.tablename + ";" + o.FieldName, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                }
            }
            catch (Exception ex)
            {
                var objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("Log Message", ex.ToString(), DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                throw new Exception();
            }
        }

        public void createdw4_setting(List<DataField> lstField)
        {
            try
            {
                foreach (DataField o in lstField)
                {
                    string sql = " insert into dw_ryg_setting(RouteNo, MappingName, ColumnName, PivotColumn, Sorting, FieldType ";
                    sql += ", DisplayName, sys_CreateDate, sys_CreateUser, sys_UpdateDate , sys_UpdateUser  , SettingType ";
                    sql += " ,GroupingType, DSS_ID , WhereItem, OptionDateColumn , OptionTotalColumn, ReportFormat  , FloatNum) ";
                    sql += " values(@RouteNo, @MappingName, @ColumnName, @PivotColumn, @Sorting, @FieldType,";
                    sql += "@DisplayName, getdate(), @sys_CreateUser, getdate() , @sys_UpdateUser ,  @SettingType, ";
                    sql += "@GroupingType,0,@WhereItem,@OptionDateColumn,@OptionTotalColumn,@ReportFormat,@FloatNum) ";
                    string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                    List<SqlParameter> lstP = new List<SqlParameter>();
                    SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp1.Value = o.RouteNo;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                    sp1.Value = o.MappingName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@ColumnName", SqlDbType.NVarChar);
                    sp1.Value = o.FieldName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@PivotColumn", SqlDbType.NVarChar);
                    sp1.Value = o.PivotColumn;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Sorting", SqlDbType.NVarChar);
                    sp1.Value = o.sortingno;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@FieldType", SqlDbType.NVarChar);
                    sp1.Value = o.FieldType;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@DisplayName", SqlDbType.NVarChar);
                    sp1.Value = o.DisplayName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@sys_CreateUser", SqlDbType.NVarChar);
                    sp1.Value = "admin";
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@sys_UpdateUser", SqlDbType.NVarChar);
                    sp1.Value = "admin";
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@SettingType", SqlDbType.NVarChar);
                    sp1.Value = o.SettingType;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@GroupingType", SqlDbType.Int);
                    sp1.Value = o.GroupingType;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@WhereItem", SqlDbType.NVarChar);
                    sp1.Value = "";
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@OptionDateColumn", SqlDbType.NVarChar);
                    sp1.Value = o.OptionDateColumn;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@OptionTotalColumn", SqlDbType.NVarChar);
                    sp1.Value = o.OptionTotalColumn;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@ReportFormat", SqlDbType.NVarChar);
                    sp1.Value = o.ReportFormat;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@FloatNum", SqlDbType.Int);
                    sp1.Value = o.FloatNum;
                    lstP.Add(sp1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, lstP.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }


        }



    }
}
