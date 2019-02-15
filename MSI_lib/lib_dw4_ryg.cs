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
    public class lib_dw4_ryg
    {
        public class Dw4_Format
        {

            public string RouteNo { set; get; }
            public string MappingName { set; get; }
            public string TableName { set; get; }
            public string Field_Display { set; get; }
            public string Field_STD { set; get; }
            public string Field_Symbol { set; get; }
            public string Field_Compare { set; get; }
            public string Field_Compare_Symbol { set; get; }
            public string Field_Value { set; get; }
            public string Field_Format { set; get; }
            public string Field_Group { set; get; }
            public string Field_ReplaceValue { set; get; }
        }
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

            public string ReportName { set; get; }
            public string RowWidth { set; get; }
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


        public class HeaderFilter
        {
            public bool allowSearch { get; set; }
        }

        public class InitProperties
        {
        }

        public class InitProperties2
        {
        }

        public class Level
        {
            public string dataField { get; set; }
            public string dataType { get; set; }
            public string groupName { get; set; }
            public string displayFolder { get; set; }
            public string groupInterval { get; set; }
            public int groupIndex { get; set; }
            public int index { get; set; }
            public InitProperties2 _initProperties { get; set; }
            public string caption { get; set; }
            public bool allowSorting { get; set; }
            public bool allowSortingBySummary { get; set; }
            public bool allowFiltering { get; set; }
            public bool allowExpandAll { get; set; }
        }

        public class SettingField
        {
            public string caption { get; set; }
            public string dataField { get; set; }
            public HeaderFilter headerFilter { get; set; }
            public string area { get; set; }
            public string dataType { get; set; }
            public string groupInterval { get; set; }
            public bool expanded { get; set; }
            public int index { get; set; }
            public InitProperties _initProperties { get; set; }
            public bool allowSorting { get; set; }
            public bool allowSortingBySummary { get; set; }
            public bool allowFiltering { get; set; }
            public bool allowExpandAll { get; set; }
            public int areaIndex { get; set; }
            public string displayFolder { get; set; }
            public string summaryType { get; set; }
            public string summaryDisplayMode { get; set; }
            public string format { get; set; }
            public string groupName { get; set; }
            public List<Level> levels { get; set; }
            public int? groupIndex { get; set; }
        }

        public class Dw4ReportSetting
        {
            public string width { get; set; }
            public string height { get; set; }
            public bool detailcheck { get; set; }
            public bool Row_Sub_Total { get; set; }
            public bool Column_Sub_Total { get; set; }
            public bool Row_Grand_Total { get; set; }
            public bool Column_Grand_Total { get; set; }
            public string byFieldArea { get; set; }
            public bool expanded { get; set; }
            public string Point { get; set; }
        }

        public class RootObject
        {
            public List<SettingField> settingFields { get; set; }
            public string TableName { get; set; }
            public string Field { get; set; }
            public string Condition { get; set; }
            public string Title { get; set; }
            public string width { get; set; }
            public string height { get; set; }
            public string Mathsetting { get; set; }
            public List<object> GropAppend { get; set; }
            public Dw4ReportSetting dw4_reportSetting { get; set; }
        }
        public class Fields
        {
            public string name { set; get; }
            public string caption { set; get; }
            public string aggregateFunc { set; get; }
            public string types { set; get; }
        }
        public class Mathsetting
        {
            public string caption { set; get; }
            public string dataField { set; get; }
            public bool expanded { set; get; }
            public HeaderFilter headerFilter { set; get; }
            public string dataType { set; get; }
            public string groupInterval { set; get; }

        }

        public class BannerObject
        {
            public string FlowName { get; set; }
            public string MappingName { get; set; }
            public string TableName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string Icon { get; set; }
            public string Title { get; set; }
            public string YFieldMath { get; set; }
            public string YFields { get; set; }
            public string Sorting { get; set; }
            public string Point { get; set; }
            public string Color { get; set; }
            public string Condition { get; set; }
            public string Url { get; set; }
        }

        public class BarChartObject
        {
            public string FlowName { get; set; }
            public string MappingName { get; set; }
            public string TableName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string XTitle { get; set; }
            public string YTitle { get; set; }
            public string YColor { get; set; }
            public string YFieldModel { get; set; }
            public bool IndexLabel { get; set; }
            public bool IndexValue { get; set; }
            public bool ShowDetail { get; set; }
            public bool ShowSecondary { get; set; }
            public string XField { get; set; }
            public string YFields { get; set; }
            public string TimeField { get; set; }
            public string GroupField { get; set; }
            public string XFiledtype { get; set; }
            public string Condition { get; set; }
            public string Pic { get; set; }
        }

        public class CombinationObject
        {
            public string FlowName { get; set; }
            public string MappingName { get; set; }
            public string TableName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string XTitle { get; set; }
            public string YTitle { get; set; }
            public string YColor { get; set; }
            public string YFieldModel { get; set; }
            public bool IndexLabel { get; set; }
            public bool IndexValue { get; set; }
            public bool ShowDetail { get; set; }
            public bool ShowSecondary { get; set; }
            public string XField { get; set; }
            public string YFields { get; set; }
            public string TimeField { get; set; }
            public string GroupField { get; set; }
            public string XFiledtype { get; set; }
            public string Condition { get; set; }
            public string Pic { get; set; }
            public string YFieldMath { get; set; }
            public string Point { get; set; }
            public string unit { get; set; }
            public string Format { get; set; }
            public string More { get; set; }
        }
        public class LineChartObject_TimeField
        {
            public string YField { get; set; }
            public string strtime { get; set; }
            public string endtime { get; set; }
            public string TimeAppear { get; set; }
            public string groupfield { get; set; }
            public bool XFiledtype { get; set; }
        }

        public class LineChartObject
        {
            public string FlowName { get; set; }
            public string MappingName { get; set; }
            public string TableName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string XTitle { get; set; }
            public string YTitle { get; set; }
            public string YColor { get; set; }
            public string YFieldModel { get; set; }
            public bool IndexLabel { get; set; }
            public bool IndexValue { get; set; }
            public bool ShowDetail { get; set; }
            public bool Showmore { get; set; }
            public string XField { get; set; }
            public string XFieldType { get; set; }
            public string YFields { get; set; }
            public string TimeField { get; set; }
            public string Condition { get; set; }
            public string Point { get; set; }
            public string unit { get; set; }
            public string Format { get; set; }
        }
        public class PieChartObject
        {
            public string FlowName { get; set; }
            public string MappingName { get; set; }
            public string TableName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool IndexLabel { get; set; }
            public bool IndexValue { get; set; }
            public bool ShowDetail { get; set; }
            public string XFields { get; set; }
            public string YFields { get; set; }
            public string YFieldModel { get; set; }
            public string LabelFields { get; set; }
            public string Condition { get; set; }
        }

        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        public lib_dw4_ryg()
        {


        }

        public void CreateNewDw4(List<DataField> lstField, List<Dw4_Format> dw4Format, Dw_1_report Dw1report)
        {
            try
            {
                DeleteAllSetting(lstField[0].RouteNo, lstField[0].MappingName, lstField[0].tablename);
                createdw4_setting(lstField);
                createdw4_format(dw4Format);
                createdw_1_report(Dw1report);
            }
            catch (Exception ex)
            {

                DeleteAllSetting(lstField[0].RouteNo, lstField[0].MappingName, lstField[0].tablename);
                throw new Exception(ex.ToString());
            }
        }

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
                sp1.Value = Dw1_report.reportName ;
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

        public Dw_1_report GetDW_1_report(string RouteNo, string MappingName, string TableName)
        {

            string SQL = "select * from DW_1_Report where RouteNo = @RouteNo AND MappingName = @MappingName AND TableName = @TableName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = RouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = MappingName;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
            sp1.Value = TableName;
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

        public void createdw4_format(List<Dw4_Format> dw4Format)
        {
            try
            {
                foreach (var o in dw4Format)
                {

                    string INS = @"INSERT INTO dw_ryg_format (sys_createuser,sys_createdate,sys_updateuser,sys_updatedate,
                                   RouteNo,MappingName,TableName,Field_Display,Field_STD,Field_Symbol,Field_Compare,Field_Compare_Symbol,Field_Value,
                                   Field_Format,Field_Group,Field_ReplaceValue)  VALUES ('host',getdate(),'host',getdate(),
                                   @RouteNo,@MappingName,@TableName,@Field_Display,@Field_STD,@Field_Symbol,@Field_Compare,@Field_Compare_Symbol,@Field_Value,
                                   @Field_Format,@Field_Group,@Field_ReplaceValue)";


                    List<SqlParameter> lstP = new List<SqlParameter>();
                    SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp1.Value = o.RouteNo;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                    sp1.Value = o.MappingName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                    sp1.Value = o.TableName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Display", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Display;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_STD", SqlDbType.NVarChar);
                    sp1.Value = o.Field_STD;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Symbol", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Symbol;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Compare", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Compare;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Compare_Symbol", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Compare_Symbol;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Value", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Value;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Format", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Format;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_Group", SqlDbType.NVarChar);
                    sp1.Value = o.Field_Group;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@Field_ReplaceValue", SqlDbType.NVarChar);
                    sp1.Value = o.Field_ReplaceValue;
                    lstP.Add(sp1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, INS, lstP.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<Dw4_Format> GetRyg_Format(string OldRouteNo, string OldMappingName, string OldTableName)
        {
            string SQL = "SELECT * FROM DW_RYG_FORMAT WHERE RouteNo=@RouteNo AND MappingName=@MappingName AND TableName=@TableName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = OldRouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = OldMappingName;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
            sp1.Value = OldTableName;
            lstP.Add(sp1);

            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            List<Dw4_Format> lstdw4format = new List<Dw4_Format>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Dw4_Format o = new Dw4_Format()
                {
                    Field_Compare = dr["Field_Compare"] == System.DBNull.Value ? "" : dr["Field_Compare"].ToString(),
                    Field_Compare_Symbol = dr["Field_Compare_Symbol"] == System.DBNull.Value ? "" : dr["Field_Compare_Symbol"].ToString(),
                    Field_Display = dr["Field_Display"] == System.DBNull.Value ? "" : dr["Field_Display"].ToString(),
                    Field_Format = dr["Field_Format"] == System.DBNull.Value ? "" : dr["Field_Format"].ToString(),
                    Field_Group = dr["Field_Group"] == System.DBNull.Value ? "" : dr["Field_Group"].ToString(),
                    Field_ReplaceValue = dr["Field_ReplaceValue"] == System.DBNull.Value ? "" : dr["Field_ReplaceValue"].ToString(),
                    Field_STD = dr["Field_STD"] == System.DBNull.Value ? "" : dr["Field_STD"].ToString(),
                    Field_Symbol = dr["Field_Symbol"] == System.DBNull.Value ? "" : dr["Field_Symbol"].ToString(),
                    Field_Value = dr["Field_Value"] == System.DBNull.Value ? "" : dr["Field_Value"].ToString(),
                    MappingName = dr["MappingName"] == System.DBNull.Value ? "" : dr["MappingName"].ToString(),
                    RouteNo = dr["RouteNo"] == System.DBNull.Value ? "" : dr["RouteNo"].ToString(),
                    TableName = dr["TableName"] == System.DBNull.Value ? "" : dr["TableName"].ToString(),
                };

                lstdw4format.Add(o);
            }
            return lstdw4format;
        }

        public class BaseDw4
        {
            public string TableName { set; get; }
            public string MappingName { set; get; }
            public string RouteNo { set; get; }
        }
        public BaseDw4 GetRouteTableMapping(string fid)
        {
            string SQL = "SELECT * FROM dw_ryg_setting Where sys_id =@fid";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@fid", SqlDbType.NVarChar);
            sp1.Value = fid;
            lstP.Add(sp1);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
            {
                return new BaseDw4();
            }

            BaseDw4 o = new BaseDw4();
            o.MappingName = ds.Tables[0].Rows[0]["MappingName"].ToString();
            o.RouteNo = ds.Tables[0].Rows[0]["RouteNo"].ToString();
            o.TableName = "v_" + ds.Tables[0].Rows[0]["RouteNo"].ToString() + "_dw";
            return o;
        }


        public List<DataField> GetDW4(string RouteNo, string MappingName)
        {

            string SQL = "SELECT * FROM dw_ryg_setting Where RouteNo =@RouteNo AND MappingName =@MappingName";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
            sp1.Value = RouteNo;
            lstP.Add(sp1);
            sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
            sp1.Value = MappingName;
            lstP.Add(sp1);
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SQL, lstP.ToArray());
            List<DataField> lst = new List<DataField>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataField o = new DataField()
                {
                    DisplayName = dr["DisplayName"].ToString(),
                    FieldName = dr["ColumnName"].ToString(),
                    //FieldSize = dr["FieldSize"].ToString(),
                    FieldType = dr["FieldType"].ToString(),
                    FloatNum = (dr["FloatNum"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["FloatNum"].ToString()),
                    GroupingType = Convert.ToInt32(dr["GroupingType"].ToString()),
                    MappingName = dr["MappingName"].ToString(),
                    OptionDateColumn = dr["OptionDateColumn"].ToString(),
                    OptionTotalColumn = dr["OptionTotalColumn"].ToString(),
                    PivotColumn = dr["PivotColumn"].ToString(),
                    ReportFormat = dr["ReportFormat"].ToString(),
                    ReportName = dr["ReportName"].ToString(),
                    RouteNo = dr["RouteNo"].ToString(),
                    RowWidth = dr["RowWidth"].ToString(),
                    SettingType = dr["SettingType"].ToString(),
                    sortingno = dr["sorting"].ToString(),
                    // tablename = dr["tablename"].ToString(),
                };

                lst.Add(o);

            }

            return lst;
            //   return null;

        }

        public void createdw4_setting(List<DataField> lstField)
        {
            try
            {
                foreach (DataField o in lstField)
                {
                    string sql = " insert into dw_ryg_setting(RouteNo, MappingName, ColumnName, PivotColumn, Sorting, FieldType ";
                    sql += ", DisplayName, sys_CreateDate, sys_CreateUser, sys_UpdateDate , sys_UpdateUser  , SettingType ";
                    sql += " ,GroupingType, DSS_ID , WhereItem, OptionDateColumn , OptionTotalColumn, ReportFormat  , FloatNum,ReportName,RowWidth) ";
                    sql += " values(@RouteNo, @MappingName, @ColumnName, @PivotColumn, @Sorting, @FieldType,";
                    sql += "@DisplayName, getdate(), @sys_CreateUser, getdate() , @sys_UpdateUser ,  @SettingType, ";
                    sql += "@GroupingType,0,@WhereItem,@OptionDateColumn,@OptionTotalColumn,@ReportFormat,@FloatNum,@ReportName,@RowWidth) ";
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
                    sp1 = new SqlParameter("@ReportName", SqlDbType.NVarChar);
                    sp1.Value = o.ReportName;
                    lstP.Add(sp1);
                    sp1 = new SqlParameter("@RowWidth", SqlDbType.NVarChar);
                    sp1.Value = o.RowWidth;
                    lstP.Add(sp1);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql, lstP.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }



        }


        public void DeleteAllSetting(string RouteNo, string MappingName, string TableName)
        {
            try
            {
                //RouteNo MappingName  TableName
                string del = "delete from dw_ryg_format where RouteNo =@RouteNo AND MappingName=@MappingName AND TableName=@TableName ;";
                del += " delete from DW_1_Report Where RouteNo=@RouteNo and MappingName=@MappingName and TableName=@TableName ;";
                del += " delete from dw_ryg_setting where MappingName=@MappingName and RouteNo=@RouteNo and SettingType='DW4'  ";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@TableName", SqlDbType.NVarChar);
                sp1.Value = TableName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@MappingName", SqlDbType.NVarChar);
                sp1.Value = MappingName;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = RouteNo;
                lstP.Add(sp1);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, del, lstP.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

    }
}
