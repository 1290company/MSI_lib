using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSI_lib.TableWidget;

namespace MSI_lib
{

    public class TableWidgetDS
    {
        private string _SqlString { set; get; }
        private string _JasonField { set; get; }
        private string _PortalName { set; get; }
        private string _TargetTableName { set; get; }
        tw_Tables t = new tw_Tables();

        public TableWidgetDS() { }
        public TableWidgetDS(string SqlString, string JsonString, string PortalName, string TargetTableName)
        {

            //  SqlString = "select [Calibration_CBA_ST1].[ST1_Model] as [Calibration_CBA_ST1_ST1_Model], [Calibration_CBA_ST1].[ST1_LevelID] as [Calibration_CBA_ST1_ST1_LevelID], [Calibration_CBA_ST1].[ST1_Qty] as [Calibration_CBA_ST1_ST1_Qty], [Calibration_CBA_ST1].[ST1_Amount] as [Calibration_CBA_ST1_ST1_Amount], [Calibration_CBA_ST1].[ST1_StartDate] as [Calibration_CBA_ST1_ST1_StartDate] from Calibration_CBA_ST1";
            // JsonString = "{\"Table\":\"tbwView_A\",\"Fields\":[{\"Table\":\"Calibration_CBA_ST1\",\"Name\":\"ST1_Model\",\"DisplayName\":\"Calibration_CBA_ST1_ST1_Model\",\"Type\":\"nvarchar\",\"Value\":\"\"},{\"Table\":\"Calibration_CBA_ST1\",\"Name\":\"ST1_LevelID\",\"DisplayName\":\"Calibration_CBA_ST1_ST1_LevelID\",\"Type\":\"nvarchar\",\"Value\":\"\"},{\"Table\":\"Calibration_CBA_ST1\",\"Name\":\"ST1_Qty\",\"DisplayName\":\"Calibration_CBA_ST1_ST1_Qty\",\"Type\":\"float\",\"Value\":\"\"},{\"Table\":\"Calibration_CBA_ST1\",\"Name\":\"ST1_Amount\",\"DisplayName\":\"Calibration_CBA_ST1_ST1_Amount\",\"Type\":\"float\",\"Value\":\"\"},{\"Table\":\"Calibration_CBA_ST1\",\"Name\":\"ST1_StartDate\",\"DisplayName\":\"Calibration_CBA_ST1_ST1_StartDate\",\"Type\":\"datetime\",\"Value\":\"\"}],\"MainTable\":\"Calibration_CBA_ST1\",\"JoinTable\":[],\"Where\":[],\"Group\":[],\"Action\":\"view\",\"Stringify\":null}";
            _PortalName = PortalName;
            _SqlString = SqlString;
            _TargetTableName = TargetTableName;
            t = (tw_Tables)JsonConvert.DeserializeObject(JsonString, typeof(tw_Tables));

        }
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;

        public void DropTable(string tab)
        {
            string sqltable = " drop TABLE [" + tab + "]";
            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sqltable, null);
        }

        public int GetSqlData(out string tab, out string _sql)
        {
            tab = "";
            _sql = "";
            DataTable dt = new DataTable();

            try
            {
                string sql = string.Format(_SqlString);
                _sql = sql;
                SqlDataReader dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, null);
                dt.Load(dr);
                dr.Close();
            }
            catch (Exception ex)
            {
                return -1;
            }

            tab = _PortalName + "_" + _TargetTableName + "_DS";
            try
            {
                string sqltable = " CREATE TABLE [" + tab + "] (\n";
                foreach (DataColumn column in dt.Columns)
                {
                    string FieldName = column.ColumnName.ToString();
                    //var Field = from f in t.Fields where f.DisplayName == DisplayName select f.Name;
                    //string sField = (type.Any()) ? type.FirstOrDefault() : "";
                    string _FieldName = "";
                    foreach(var f in t.Fields)
                    {
                        if (FieldName.IndexOf(f.Name) == 2 && f.Name.Length + 2 == FieldName.Length)
                        {
                            _FieldName = f.Name;
                            break;
                        }
                    }
                    var type = from tp in t.Fields where tp.Name == _FieldName select tp.Type;
                    string stype = (type.Any()) ? type.FirstOrDefault() : "";
                    string dbtype = GetSqlType(stype).ToLower();
                    sqltable += "[" + _FieldName + "] " + dbtype + ",\n";
                }

                sqltable = sqltable.TrimEnd(new char[] { ',', '\n' }) + "\n";
                sqltable += ")";
                _sql = sqltable;
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sqltable, null);
            }
            catch (Exception ex)
            {
                return -2;
            }

            try
            {
                var f = from c in t.Fields select c.Name;
                string joinField = string.Join("],[", f.ToArray());
                string ins = "Insert into " + tab + "([" + joinField + "]) ";
                ins += _SqlString;
                _sql = ins;
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, ins, null);
            }
            catch (Exception ex)
            {
                DropTable(tab);
                return -3;
            }

            try
            {
                var f1 = from c in t.Fields select c;
                foreach (var dwf in f1)
                {
                    string insDwfield = "Insert into dw_field (TableName,Field,DisplayName,FieldType,FieldSize,Status,CreateDate,CreateUser,UpdateDate,UpdateUser) ";
                    insDwfield += " Values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},'{9}') ";
                    insDwfield = string.Format(insDwfield, tab, dwf.Name, dwf.DisplayName, dwf.Type, "255", "1", "getdate()", "TableWidgeDs", "getdate()", "TableWidgeDs");
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, insDwfield, null);
                }
            }
            catch (Exception ex)
            {
                DropTable(tab);
                return -4;
            }


            return 1;

        }

        public string GetSqlType(string type)
        {
            switch (type.ToLower())
            {
                case "float":
                    return "FLOAT";
                case "nvarchar":
                    return "NVARCHAR(" + "255" + ")";
                case "int":
                    return "INT";
                case "datetime":
                    return "DATETIME";
                case "decimal":
                    return "DECIMAL(" + 8 + "," + 2 + ")";
                default:
                    return "NVARCHAR(255)";
            }
        }


    }
}
