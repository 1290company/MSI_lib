using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace MSI_lib
{
    public class TableWidget
    {
        public enum ActionType : int
        {
            view = 1, insert = 2, update = 3
        }

        #region class
        public class tw_Field
        {
            public string Table { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public string Fx { get; set; }
            public string Model { get; set; }
        }

        public class tw_On
        {
            public string Where { get; set; }
            public string Target_Table { get; set; }
            public string Join_Table { get; set; }
            public string Join_Type { get; set; }
        }

        public class tw_JoinTable
        {
            public string Table { get; set; }
            public List<tw_On> On { get; set; }
            public List<tw_Tables> Items { get; set; }
        }

        public class tw_Where
        {
            public string Table { get; set; }
            public string Field { get; set; }
            public string FieldType { get; set; }
            public string Operator { get; set; }
            public string Formula { get; set; }
            public string Fx { get; set; }
        }

        public class tw_Group
        {
            public string Table { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Fx { get; set; }
        }

        public class tw_Tables
        {
            public string Table { get; set; }
            public List<tw_Field> Fields { get; set; }
            public string MainTable { get; set; }
            public List<tw_JoinTable> JoinTable { get; set; }
            public List<tw_Where> Where { get; set; }
            public List<tw_Group> Group { get; set; }
            public string Action { get; set; }
            public tw_Setting Setting { get; set; }
            public string Stringify { get; set; }
        }

        public class tw_Setting
        {
            public bool WithRollup { get; set; }
        }

        public class AllItems
        {
            public List<tw_Tables> Items { get; set; }
        }

        public class CopyData_Param
        {
            public string SourceActionName { set; get; }
            public string TargetActionName { set; get; }
            public string SourceRouteNo { set; get; }
            public string TargetRouteNo { set; get; }
            public string LoginUser { set; get; }
        }
        #endregion

        public int PortalID { get; set; }
        public int TargetPortalID { get; set; }
        public bool UseDefaultFieldName { get; set; } = false;
        public bool UseOldPrefixOrder { get; set; } = false;
        public static string[] FxArr = { "tablefield", "getdate", "sysid", "bid", "username", "dateformat", "datediff", "dateadd", "count", "sum", "max", "min", "multimaxmin", "casewhen", "calculate", "top_10p", "last_10p", "top_10p_nth", "last_10p_nth", "toplast_np", "stdevp" };
        public static string[] OperatorArr = { ">=", ">", "<", "<=", "=", "<>", "like", "in", "not in" };

        /// <summary>
        /// TableWidget
        /// </summary>
        /// <param name="_PortalID">PortalID</param>
        public TableWidget(int _PortalID)
        {
            PortalID = _PortalID;
        }

        /// <summary>
        /// TableWidget
        /// </summary>
        /// <param name="_PortalID"></param>
        /// <param name="_TargetPortalID"></param>
        public TableWidget(int _PortalID, int _TargetPortalID)
        {
            PortalID = _PortalID;
            TargetPortalID = _TargetPortalID;
        }

        public List<string> GetList(ActionType actionType)
        {
            List<string> list = new List<string>();

            string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            string sql = string.Format("select * from DW_TableWidget where ActionType = '{0}' and PortalID = {1}", Enum.GetName(typeof(ActionType), actionType), PortalID);

            SqlDataReader dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, null);
            DataTable dt = new DataTable();
            dt.Load(dr);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    list.Add(dt.Rows[i]["ActionName"].ToString());
            }

            return list;
        }

        public List<tw_Field> GetFields(string ActionName)
        {
            List<tw_Field> list = new List<tw_Field>();

            string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            string sql = string.Format("select top 1 * from DW_TableWidget where ActionName = @ActionName and PortalID = {0}", PortalID);

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@ActionName", SqlDbType.NVarChar);
            arParms[0].Value = ActionName;
            SqlDataReader dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, arParms);
            DataTable dt = new DataTable();
            dt.Load(dr);

            if (dt != null && dt.Rows.Count > 0)
            {
                tw_Tables t = (tw_Tables)JsonConvert.DeserializeObject(dt.Rows[0]["JSONString"].ToString(), typeof(tw_Tables));
                return t.Fields;
            }

            return list;
        }

        public string GetSql(string ActionName)
        {
            string SqlString = "";
            List<tw_Field> list = new List<tw_Field>();

            string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
            string sql = string.Format("select top 1 * from DW_TableWidget where ActionName = @ActionName and PortalID = {0}", PortalID);

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@ActionName", SqlDbType.NVarChar);
            arParms[0].Value = ActionName;
            SqlDataReader dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql, arParms);
            DataTable dt = new DataTable();
            dt.Load(dr);

            if (dt != null && dt.Rows.Count > 0)
                SqlString = dt.Rows[0]["SQLString"].ToString();

            return SqlString;
        }

        public tw_Tables ConvertDeserialize(string jsonStr)
        {
            try
            {
                tw_Tables t = (tw_Tables)JsonConvert.DeserializeObject(jsonStr, typeof(tw_Tables));
                return t;
            }
            catch
            {
                return null;
            }
        }
        public string ConvertSerialize(tw_Tables json)
        {
            try
            {
                string s = JsonConvert.SerializeObject(json);
                return s;
            }
            catch
            {
                return "";
            }
        }

        //逐一檢查 SQL JSON 是否有問題
        public static bool VerifySqlJson(tw_Tables t, out string ErrorMessage)
        {
            ErrorMessage = "";

            try
            {
                if (t == null) return false;
                
                #region 檢查欄位
                foreach (var f in t.Fields)
                {
                    //Table
                    if (!string.IsNullOrEmpty(f.Table) && !IsNumericOrLetterOrUnderline(f.Table))
                    {
                        ErrorMessage = string.Concat("欄位資料表[", f.Table, "]有誤");
                        return false;
                    }

                    //Name
                    if (!string.IsNullOrEmpty(f.Name) && (f.Name.Trim().IndexOf(" ") > -1 || f.Name.Trim().IndexOf("-") > -1 || f.Name.Trim().IndexOf("'") > -1))
                    {
                        ErrorMessage = string.Concat(f.DisplayName, " 欄位[", f.Name, "]有誤");
                        return false;
                    }

                    if (t.Action == "view" || t.Action == "dataset")
                    {
                        //DisplayName
                        if (!string.IsNullOrEmpty(f.DisplayName) && (f.DisplayName.Trim().IndexOf(" ") > -1 || f.DisplayName.Trim().IndexOf("-") > -1 || f.DisplayName.Trim().IndexOf("'") > -1))
                        {
                            ErrorMessage = string.Concat("顯示名稱[", f.DisplayName, "]有誤");
                            return false;
                        }
                    }
                    else
                    {
                        //DisplayName
                        if (!string.IsNullOrEmpty(f.DisplayName) && !CheckBadStr4Field(f.DisplayName))
                        {
                            ErrorMessage = string.Concat("欄位對應值[", f.DisplayName, "]有誤");
                            return false;
                        }
                    }

                    //Type
                    string[] datatype = { "varchar", "nvarchar", "float", "decimal", "int", "datetime" };
                    if (!string.IsNullOrEmpty(f.Type) && Array.IndexOf(datatype, f.Type) == -1)
                    {
                        ErrorMessage = string.Concat(f.DisplayName, " 欄位格式[", f.Type, "]有誤");
                        return false;
                    }

                    //Value
                    if (!string.IsNullOrEmpty(f.Value) && !CheckBadStr4Field(f.Value))
                    {
                        ErrorMessage = string.Concat(f.DisplayName, " 欄位值[", f.Value, "]有誤");
                        return false;
                    }

                    //Fx                            
                    if (!string.IsNullOrEmpty(f.Fx) && Array.IndexOf(FxArr, f.Fx) == -1)
                    {
                        ErrorMessage = string.Concat(f.DisplayName, " 函數[", f.Fx, "]有誤");
                        return false;
                    }

                    //Model
                    if (!string.IsNullOrEmpty(f.Model) && (f.Model.Trim().IndexOf(" ") > -1 || f.Model.Trim().IndexOf("-") > -1 || f.Model.Trim().IndexOf("'") > -1))
                    {
                        ErrorMessage = string.Concat(f.DisplayName, " Model[", f.Model, "]有誤");
                        return false;
                    }
                }
                #endregion

                //Table
                if (!string.IsNullOrEmpty(t.Table) && !IsNumericOrLetterOrUnderline(t.Table))
                {
                    ErrorMessage = string.Concat("資料表[", t.Table, "]有誤");
                    return false;
                }

                //MainTable
                if (!string.IsNullOrEmpty(t.MainTable) && !IsNumericOrLetterOrUnderline(t.MainTable))
                {
                    ErrorMessage = string.Concat("主表[", t.MainTable, "]有誤");
                    return false;
                }

                //JoinTable
                foreach (var f in t.JoinTable)
                {
                    //public string Table { get; set; }
                    //public List<tw_On> On { get; set; }
                    //public List<tw_Tables> Items { get; set; }

                    //Table
                    if (!string.IsNullOrEmpty(f.Table) && !IsNumericOrLetterOrUnderline(f.Table))
                    {
                        ErrorMessage = string.Concat("Join表[", f.Table, "]有誤");
                        return false;
                    }

                    if (f.On != null && f.On.Count > 0)
                    {
                        foreach (var o in f.On)
                        {
                            //Target_Table
                            if (!string.IsNullOrEmpty(o.Target_Table) && !IsNumericOrLetterOrUnderline(o.Target_Table))
                            {
                                ErrorMessage = string.Concat("target table[", o.Target_Table, "]有誤");
                                return false;
                            }
                            //Join_Table
                            if (!string.IsNullOrEmpty(o.Join_Table) && !IsNumericOrLetterOrUnderline(o.Join_Table))
                            {
                                ErrorMessage = string.Concat("join table[", o.Join_Table, "]有誤");
                                return false;
                            }
                            //Where
                            if (!string.IsNullOrEmpty(o.Where) && !CheckBadStr4Join(o.Where))
                            {
                                ErrorMessage = string.Concat(f.Table, " 條件[", o.Where, "]有誤");
                                return false;
                            }
                            //Join Type
                            string[] jointype = { "inner join", "left join", "right join", "full outer join" };
                            if (!string.IsNullOrEmpty(o.Join_Type) && Array.IndexOf(jointype, o.Join_Type) == -1)
                            {
                                ErrorMessage = string.Concat(f.Table, " join type[", o.Join_Type, "]有誤");
                                return false;
                            }
                        }

                        //Items
                        if (f.Items != null && f.Items.Count > 0)
                        {
                            foreach (var item in f.Items)
                            {
                                if (!VerifySqlJson(item, out ErrorMessage))
                                    return false;
                            }
                        }
                    }
                }

                //public string Table { get; set; }
                //public string Field { get; set; }
                //public string FieldType { get; set; }
                //public string Operator { get; set; }
                //public string Formula { get; set; }
                //public string Fx { get; set; }
                //檢查 Where
                foreach (var w in t.Where)
                {
                    //Table
                    if (!string.IsNullOrEmpty(w.Table) && !IsNumericOrLetterOrUnderline(w.Table))
                    {
                        ErrorMessage = string.Concat("條件資料表[", w.Table, "]有誤");
                        return false;
                    }

                    //Name
                    if (!string.IsNullOrEmpty(w.Field) && (w.Field.Trim().IndexOf(" ") > -1 || w.Field.Trim().IndexOf("-") > -1 || w.Field.Trim().IndexOf("'") > -1))
                    {
                        ErrorMessage = string.Concat("條件欄位[", w.Field, "]有誤");
                        return false;
                    }

                    //Type
                    string[] datatype = { "varchar", "nvarchar", "float", "decimal", "int", "datetime" };
                    if (!string.IsNullOrEmpty(w.FieldType) && Array.IndexOf(datatype, w.FieldType) > -1)
                    {
                        ErrorMessage = string.Concat("條件欄位格式[", w.FieldType, "]有誤");
                        return false;
                    }

                    //Operator
                    if (!string.IsNullOrEmpty(w.Operator) && Array.IndexOf(OperatorArr, w.Operator) > -1)
                    {
                        ErrorMessage = string.Concat("條件運算子[", w.FieldType, "]有誤");
                        return false;
                    }

                    //Formula
                    if (!string.IsNullOrEmpty(w.Operator) && CheckBadStr4Where(w.Operator))
                    {
                        ErrorMessage = string.Concat("條件值[", w.FieldType, "]有誤");
                        return false;
                    }

                    //Fx                            
                    if (!string.IsNullOrEmpty(w.Fx) && Array.IndexOf(FxArr, w.Fx) == -1)
                    {
                        ErrorMessage = string.Concat("條件函數[", w.Fx, "]有誤");
                        return false;
                    }
                }


                //Group
                foreach (var g in t.Group)
                {
                    //public string Table { get; set; }
                    //public string Name { get; set; }
                    //public string DisplayName { get; set; }
                    //public string Fx { get; set; }

                    //Table
                    if (!string.IsNullOrEmpty(g.Table) && !IsNumericOrLetterOrUnderline(g.Table))
                    {
                        ErrorMessage = string.Concat("group table[", g.Table, "]有誤");
                        return false;
                    }
                    //Name
                    if (!string.IsNullOrEmpty(g.Name) && !CheckBadStr4Field(g.Name))
                    {
                        ErrorMessage = string.Concat("group field[", g.Table, "]有誤");
                        return false;
                    }
                    //DisplayName
                    if (!string.IsNullOrEmpty(g.DisplayName) && (g.DisplayName.Trim().IndexOf(" ") > -1 || g.DisplayName.Trim().IndexOf("-") > -1 || g.DisplayName.Trim().IndexOf("'") > -1))
                    {
                        ErrorMessage = string.Concat("group 顯示名稱[", g.DisplayName, "]有誤");
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public string ConvertSql(tw_Tables t, string SourceAction = "", bool IsInit = true)
        {
            string sql = "";

            try
            {
                if (t == null) return "";

                string s_fields = "", _s_fields = "";
                string s_values = "";
                string s_tables = "";
                string s_source_tables = "";
                string s_where = "";
                string s_group = "";

                switch (t.Action)
                {
                    case "dataset":
                    case "view":
                        #region view
                        //TableFieldPrefix = new Dictionary<string, string>();
                        //TableFieldPrefixIdx = 0;

                        //先分配資料表前贅字
                        if (IsInit)
                        {
                            InitTablePrefix(t);
                            InitTableFields(t, SourceAction);
                        }

                        #region 組欄位
                        bool existedSysID = false, isTBW = false;
                        string __table = "", __field = "", __displayName = "", funcValue = "";
                        Dictionary<string, tw_Field> dic10pFields = new Dictionary<string, tw_Field>(StringComparer.OrdinalIgnoreCase);
                        Dictionary<string, tw_Field> dic10pNthFields = new Dictionary<string, tw_Field>(StringComparer.OrdinalIgnoreCase);
                        Dictionary<string, tw_Field> dicNPFields = new Dictionary<string, tw_Field>(StringComparer.OrdinalIgnoreCase);
                        Dictionary<string, tw_Field> stdevpFields = new Dictionary<string, tw_Field>(StringComparer.OrdinalIgnoreCase);
                        Dictionary<string, tw_Field> returnFields = new Dictionary<string, tw_Field>(StringComparer.OrdinalIgnoreCase);
                        Dictionary<string, string> dicGroupDateFields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        for (var i = 0; i < t.Fields.Count; i++)
                        {
                            if (t.Fields[i].Fx == "top_10p" || t.Fields[i].Fx == "last_10p")
                            {
                                dic10pFields.Add(t.Fields[i].DisplayName, t.Fields[i]);
                                continue;
                            }
                            else if (t.Fields[i].Fx == "top_10p_nth" || t.Fields[i].Fx == "last_10p_nth")
                            {
                                dic10pNthFields.Add(t.Fields[i].DisplayName, t.Fields[i]);
                                continue;
                            }
                            else if (t.Fields[i].Fx == "toplast_np")
                            {
                                dicNPFields.Add(t.Fields[i].DisplayName, t.Fields[i]);
                                continue;
                            }
                            else if (t.Fields[i].Fx == "stdevp")
                            {
                                stdevpFields.Add(t.Fields[i].DisplayName, t.Fields[i]);
                                continue;
                            }
                            else if (t.Fields[i].Fx == "multimaxmin")
                            {
                                var arrFields = t.Fields[i].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                string multimaxminStr = "";
                                for (int j = 1; j < arrFields.Length; j++)
                                    multimaxminStr += string.Concat(j > 1 ? ", ([" : "([", arrFields[j].Trim().Split('.')[0], "].[", arrFields[j].Trim().Split('.')[1], "])");
                                s_fields += string.Format("{0}(SELECT {3}(maxNum) FROM (VALUES {1}) AS cValues(maxNum)) as [{2}]", (i > 0 ? ", " : ""), multimaxminStr, t.Fields[i].DisplayName, arrFields[0]);

                                continue;
                            }

                            isTBW = false;
                            if (!string.IsNullOrEmpty(t.Fields[i].Name) && !string.IsNullOrEmpty(t.Fields[i].Model) && t.Fields[i].Model == "custom")
                            {
                                if (!dicTableFields[t.Table].ContainsKey(t.Fields[i].Name))
                                    dicTableFields[t.Table].Add(t.Fields[i].Name, t.Fields[i].DisplayName);

                                foreach (var f in dicTableFields)
                                {
                                    if (f.Value.Count == 0) continue;

                                    foreach (var f2 in f.Value)
                                        t.Fields[i].Value = Regex.Replace(t.Fields[i].Value, string.Concat(f.Key, ".", f2.Key), string.Concat(f.Key, ".", f2.Value), RegexOptions.IgnoreCase);
                                }

                                funcValue = setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx);
                                if (!returnFields.ContainsKey(t.Fields[i].DisplayName))
                                    returnFields.Add(t.Fields[i].DisplayName, new tw_Field() { Table = "", Name = funcValue, Type = t.Fields[i].Type, DisplayName = t.Fields[i].DisplayName });

                                s_fields += string.Format("{0}{1} as [{2}]", (i > 0 ? ", " : ""), funcValue, t.Fields[i].Name);
                                //s_fields += (i > 0 ? ", " : "") + setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx) + " as [" + t.Fields[i].DisplayName + "]";
                                //s_fields += (i > 0 ? ", " : "") + setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx) + " as [" + t.Fields[i].Table + "_" + t.Fields[i].Name + "]";
                            }
                            else if (string.IsNullOrEmpty(t.Fields[i].Name))
                            {
                                if (!dicTableFields[t.Table].ContainsKey(t.Fields[i].DisplayName))
                                    dicTableFields[t.Table].Add(t.Fields[i].DisplayName, t.Fields[i].DisplayName);

                                foreach (var f in dicTableFields)
                                {
                                    if (f.Value.Count == 0) continue;

                                    foreach (var f2 in f.Value)
                                        t.Fields[i].Value = Regex.Replace(t.Fields[i].Value, string.Concat(f.Key, ".", f2.Key), string.Concat(f.Key, ".", f2.Value), RegexOptions.IgnoreCase);
                                }

                                funcValue = setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx);
                                if (!returnFields.ContainsKey(t.Fields[i].DisplayName))
                                    returnFields.Add(t.Fields[i].DisplayName, new tw_Field() { Table = "", Name = funcValue, Type = t.Fields[i].Type, DisplayName = t.Fields[i].DisplayName });

                                s_fields += string.Format("{0}{1} as [{2}]", (i > 0 ? ", " : ""), funcValue, t.Fields[i].DisplayName);
                                //s_fields += (i > 0 ? ", " : "") + setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx) + " as [" + t.Fields[i].DisplayName + "]";
                                //s_fields += (i > 0 ? ", " : "") + setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx) + " as [" + t.Fields[i].Table + "_" + t.Fields[i].Name + "]";
                            }
                            else
                            {
                                __table = t.Fields[i].Table;
                                if (__table.EndsWith("_tbw_dw"))
                                    isTBW = true;

                                //if (!TableFieldPrefix.ContainsKey(__table))
                                //    TableFieldPrefix.Add(__table, GetFieldPrefix());

                                __field = t.Fields[i].Name;

                                if (SourceAction == "insert" || SourceAction == "update")
                                    __displayName = t.Fields[i].Name;
                                else if (UseDefaultFieldName)
                                    __displayName = t.Fields[i].DisplayName;
                                else
                                    __displayName = TableFieldPrefix[__table] + "_" + t.Fields[i].Name;

                                if (t.Action == "view" && !existedSysID && t.Fields[i].DisplayName.ToLower().Equals("sys_id"))
                                {
                                    existedSysID = true;
                                    __displayName = "sys_ID";

                                    if (isTBW)
                                        __field = "sys_ID";
                                }

                                if (!dicTableFields[t.Table].ContainsKey(__field))
                                    dicTableFields[t.Table].Add(__field, __displayName);

                                if (!returnFields.ContainsKey(t.Fields[i].DisplayName))
                                    returnFields.Add(t.Fields[i].DisplayName, new tw_Field() { Table = __table, Name = __field, Type = t.Fields[i].Type, DisplayName = __displayName });

                                if (t.Setting.WithRollup)
                                {
                                    string lastGroupName = t.Group[t.Group.Count - 1].Name;
                                    if (lastGroupName == __field)
                                    {
                                        if (t.Fields[i].Type == "varchar" || t.Fields[i].Type == "nvarchar")
                                            s_fields += string.Format("{0} ISNULL([{1}].[{2}], 'Total') as [{3}]", (i > 0 ? ", " : ""), __table, __field, __displayName);
                                        else
                                            s_fields += string.Format("{0} ISNULL(cast([{1}].[{2}] as nvarchar), 'Total') as [{3}]", (i > 0 ? ", " : ""), __table, __field, __displayName);
                                    }
                                    else
                                        s_fields += string.Format("{0}[{1}].[{2}] as [{3}]", (i > 0 ? ", " : ""), __table, __field, __displayName);
                                }
                                else
                                {
                                    var g = t.Group.Where(p => p.Name == t.Fields[i].Name && p.Table == t.Fields[i].Table && p.Fx == "convert_date111").SingleOrDefault();
                                    if (g != null && !string.IsNullOrEmpty(g.Name) && t.Fields[i].Type == "datetime")
                                    {
                                        _s_fields = string.Format("{0}cast(convert(varchar, [{1}].[{2}], 111) as datetime) as [{3}]", (i > 0 ? ", " : ""), __table, __field, __displayName);
                                        s_fields += _s_fields;

                                        if (!dicGroupDateFields.ContainsKey(t.Fields[i].DisplayName))
                                            dicGroupDateFields.Add(t.Fields[i].DisplayName, _s_fields);
                                    }
                                    else
                                        s_fields += string.Format("{0}[{1}].[{2}] as [{3}]", (i > 0 ? ", " : ""), __table, __field, __displayName);
                                }

                                //s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Table + "].[" + t.Fields[i].Name + "]" + " as [" + (t.Action == "view" && !existed && t.Fields[i].Name.ToLower().Equals("sys_id") ? t.Fields[i].Name : t.Fields[i].Table + "_" + t.Fields[i].Name) + "]";
                                //s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Table + "].[" + t.Fields[i].Name + "]" + " as [" + t.Fields[i].DisplayName + "]";
                            }
                        }
                        #endregion

                        //組 Table
                        s_tables = string.Concat("[", t.MainTable, "]");

                        for (var i = 0; i < t.JoinTable.Count; i++)
                        {
                            if (t.JoinTable[i].Items != null && t.JoinTable[i].Items.Count > 0)
                            {
                                var _s_tables = "";
                                for (var j = 0; j < t.JoinTable[i].Items.Count; j++)
                                {
                                    var _table = ConvertSql(t.JoinTable[i].Items[j], "", false);
                                    _s_tables += string.Format(" {0} ({1}) {2}", t.JoinTable[i].On[0].Join_Type, _table, t.JoinTable[i].Table);
                                }
                                s_tables += _s_tables;
                            }
                            else
                            {
                                s_tables += string.Format(" {0} [{1}]", t.JoinTable[i].On[0].Join_Type, t.JoinTable[i].Table);
                            }

                            for (var j = 0; j < t.JoinTable[i].On.Count; j++)
                            {

                                foreach (var f in dicTableFields)
                                {
                                    if (f.Value.Count == 0) continue;

                                    foreach (var f2 in f.Value)
                                        t.JoinTable[i].On[j].Where = t.JoinTable[i].On[j].Where.Replace(string.Concat("[", f.Key, "].[", f2.Key, "]"), string.Concat("[", f.Key, "].[", f2.Value, "]"));
                                }

                                s_tables += (j == 0 ? " on " : " and ") + t.JoinTable[i].On[j].Where;
                            }
                        }

                        //組條件
                        s_where = GetWhere(t.Where);

                        //組Group
                        bool isFunc = false;
                        for (var i = 0; i < t.Group.Count; i++)
                        {
                            tw_Field f = new tw_Field();
                            isFunc = string.IsNullOrEmpty(t.Group[i].Name);
                            if (isFunc)
                            {
                                f = t.Fields.Where(p => p.DisplayName == t.Group[i].DisplayName).SingleOrDefault();
                                s_group += string.Concat((i > 0 ? ", " : ""), "(", f.Value, ")");
                            }
                            else
                            {
                                var _f = t.Fields.Where(p => p.Name == t.Group[i].Name);
                                if (_f == null || _f.Count() == 0)
                                {
                                    int tryInt = 0;
                                    if (int.TryParse(t.Group[i].Name, out tryInt) && !string.IsNullOrEmpty(t.Group[i].DisplayName))
                                        s_group += string.Concat((i > 0 ? ", [" : "["), t.Group[i].DisplayName, "]");
                                    else
                                        s_group += string.Concat((i > 0 ? ", " : ""), t.Group[i].Name);
                                }
                                else
                                {
                                    f = _f.FirstOrDefault();
                                    if (f != null && string.IsNullOrEmpty(f.Table))
                                        s_group += string.Concat((i > 0 ? ", " : ""), (isFunc ? "(" + f.Value + ")" : f.Name));
                                    else if (f != null && f.Type == "datetime" && t.Group[i].Fx == "convert_date111")
                                        s_group += (i > 0 ? ", " : "") + "cast(convert(varchar, [" + t.Group[i].Table + "].[" + t.Group[i].Name + "], 111) as datetime)";
                                    else
                                        s_group += (i > 0 ? ", " : "") + "[" + t.Group[i].Table + "].[" + t.Group[i].Name + "]";
                                }
                            }
                        }

                        //組標準差語法
                        string stdevpFieldStr = "";
                        if (stdevpFields.Count > 0)
                        {
                            string[] arr = new string[0], arr2 = new string[0];
                            foreach (var f in stdevpFields)
                            {
                                arr = f.Value.Value.Split(';');
                                if (arr.Length != 2) continue;

                                stdevpFieldStr += string.Concat("STDEVP(", (arr[1] == "1" ? "DISTINCT " : ""), arr[0], ") [", f.Value.DisplayName, "], ");
                            }
                        }

                        //組前10%, 後10%
                        int idx = 1;
                        string sql10p = "";
                        if (dic10pFields.Count > 0)
                        {
                            foreach (var f in dic10pFields)
                                sql10p += GetTopLastString(idx++, f.Value, returnFields, s_tables, s_where);
                        }

                        //組第10%前後
                        string sql10pNth = "";
                        if (dic10pNthFields.Count > 0)
                        {
                            foreach (var f in dic10pNthFields)
                                sql10pNth += GetTopLastString(idx++, f.Value, returnFields, s_tables, s_where);
                        }

                        //組前後幾%
                        string sqlnp = "";
                        if (dicNPFields.Count > 0)
                        {
                            foreach (var f in dicNPFields)
                                sqlnp += GetTopLastString(idx++, f.Value, returnFields, s_tables, s_where);
                        }

                        sql = string.Concat(@"
                                select ",
                                    stdevpFieldStr, s_fields,
                                " from ", s_tables,
                                (s_where.Length > 0 ? " where " + s_where : ""),
                                (s_group.Length > 0 ? " group by " + s_group : ""),
                                (t.Setting.WithRollup ? " with rollup" : "")
                            );

                        //取得 前10%, 後10% 欄位
                        string fields_10p = dic10pFields.Count > 0 ? (string.Join(", ", dic10pFields.Keys.ToArray()) + ", ") : "";
                        //取得 第10%前後 欄位
                        string fields_10pNth = dic10pNthFields.Count > 0 ? (string.Join(", ", dic10pNthFields.Keys.ToArray()) + ", ") : "";
                        //取得 前後幾% 欄位
                        string fields_np = dicNPFields.Count > 0 ? (string.Join(", ", dicNPFields.Keys.ToArray()) + ", ") : "";

                        sql = string.Concat("select ", fields_10p, fields_10pNth, fields_np, " tb.* from (", sql, ") tb", sql10p, sql10pNth, sqlnp);

                        ////組標準差語法
                        //if (stdevpFields.Count > 0)
                        //{
                        //    string stdevpSql = "", stdField = "";
                        //    string[] arr = new string[0], arr2 = new string[0];
                        //    foreach (var f in stdevpFields)
                        //    {
                        //        arr = f.Value.Value.Split(';');
                        //        if (arr.Length != 2) continue;

                        //        if (!returnFields.ContainsKey(arr[0])) continue;
                        //        stdField = returnFields[arr[0]].DisplayName;

                        //        stdevpSql += string.Concat("(select STDEVP(", (arr[1] == "1" ? "DISTINCT " : ""), stdField, ") from (", sql, ") t) [", f.Value.DisplayName, "], ");
                        //    }

                        //    sql = string.Concat("select ", stdevpSql, "tb.* from (", sql, ") tb");
                        //}

                        #endregion
                        break;
                    case "insert":
                        #region insert
                        //組欄位
                        for (var i = 0; i < t.Fields.Count; i++)
                        {
                            s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Name + "]";
                            s_values += (i > 0 ? ", " : "") + (t.Fields[i].Table.Length == 0 ? setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx) : ("[" + t.Fields[i].Table + "].[" + t.Fields[i].Value + "]"));
                        }
                        //組 Table
                        s_tables = t.MainTable;
                        s_source_tables = GetJoinTable(t.JoinTable, "insert");
                        //組條件
                        s_where = GetWhere(t.Where);

                        if (s_source_tables.Length == 0)
                        {
                            sql = "insert into [" + s_tables + "] (" + s_fields + ") values (" + s_values + ") " + (s_where.Length > 0 ? " where " + s_where : "");
                        }
                        else
                        {
                            sql = "insert into [" + s_tables + "] (" + s_fields + ") select " + s_values + " from " + s_source_tables + (s_where.Length > 0 ? " where " + s_where : "");
                        }
                        #endregion
                        break;
                    case "update":
                        #region update
                        //組欄位
                        for (var i = 0; i < t.Fields.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(t.Fields[i].Fx))
                            {
                                s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Name + "] = " + t.Fields[i].Value;
                            }
                            else if (t.Fields[i].Table.Length == 0)
                            {
                                s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Name + "] = " + setValueFormat(t.Fields[i].Type, "", t.Fields[i].Value, t.Fields[i].Fx);
                            }
                            else
                            {
                                s_fields += (i > 0 ? ", " : "") + "[" + t.Fields[i].Name + "] = [" + t.Fields[i].Table + "].[" + t.Fields[i].Value + "]";
                            }
                        }
                        //組 Table
                        s_tables = t.MainTable;
                        s_source_tables = GetJoinTable(t.JoinTable, "update");
                        //組條件
                        s_where = GetWhere(t.Where);

                        if (s_source_tables.Length == 0)
                        {
                            sql = "update [" + s_tables + "] set " + s_fields + (s_where.Length > 0 ? " where " + s_where : "");
                        }
                        else
                        {
                            sql = "update [" + s_tables + "] set " + s_fields + " from " + s_source_tables + (s_where.Length > 0 ? " where " + s_where : "");
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return sql;
        }

        //
        public void InitTablePrefix(tw_Tables t)
        {
            if (TableFieldPrefix.Count > 0) return;

            for (var i = 0; i < t.Fields.Count; i++)
            {
                if ((!string.IsNullOrEmpty(t.Fields[i].Table) || UseOldPrefixOrder) && !TableFieldPrefix.ContainsKey(t.Fields[i].Table))
                    TableFieldPrefix.Add(t.Fields[i].Table, GetFieldPrefix());
            }

            for (var i = 0; i < t.JoinTable.Count; i++)
            {
                if (t.JoinTable[i].Items != null && t.JoinTable[i].Items.Count > 0)
                {
                    for (var j = 0; j < t.JoinTable[i].Items.Count; j++)
                        InitTablePrefix(t.JoinTable[i].Items[j]);
                }
            }
        }

        //
        public void InitTableFields(tw_Tables t, string SourceAction = "")
        {
            //組欄位
            bool existedSysID = false, isTBW = false;
            string __table = "", __field = "", __displayName = "";
            for (var i = 0; i < t.Fields.Count; i++)
            {
                if (!dicTableFields.ContainsKey(t.Table))
                    dicTableFields.Add(t.Table, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

                isTBW = false;
                if (!string.IsNullOrEmpty(t.Fields[i].Model) && t.Fields[i].Model == "custom")
                {
                    if (!dicTableFields[t.Table].ContainsKey(t.Fields[i].Name))
                        dicTableFields[t.Table].Add(t.Fields[i].Name, t.Fields[i].DisplayName);
                }
                else if (string.IsNullOrEmpty(t.Fields[i].Name))
                {
                    if (!dicTableFields[t.Table].ContainsKey(t.Fields[i].DisplayName))
                        dicTableFields[t.Table].Add(t.Fields[i].DisplayName, t.Fields[i].DisplayName);
                }
                else
                {
                    __table = t.Fields[i].Table;
                    if (__table.EndsWith("_tbw_dw"))
                        isTBW = true;

                    __field = t.Fields[i].Name;

                    if (SourceAction == "insert" || SourceAction == "update")
                        __displayName = t.Fields[i].Name;
                    else
                        __displayName = TableFieldPrefix[__table] + "_" + t.Fields[i].Name;

                    if (t.Action == "view" && !existedSysID && t.Fields[i].DisplayName.ToLower().Equals("sys_id"))
                    {
                        existedSysID = true;
                        __displayName = "sys_ID";

                        if (isTBW)
                            __field = "sys_ID";
                    }

                    if (!dicTableFields[t.Table].ContainsKey(__field))
                        dicTableFields[t.Table].Add(__field, __displayName);
                }
            }

            for (var i = 0; i < t.JoinTable.Count; i++)
            {
                if (t.JoinTable[i].Items != null && t.JoinTable[i].Items.Count > 0)
                {
                    for (var j = 0; j < t.JoinTable[i].Items.Count; j++)
                        InitTableFields(t.JoinTable[i].Items[j]);
                }
            }
        }

        public string GetTopLastString(int Idx, tw_Field Field, Dictionary<string, tw_Field> ReturnFields, string FormStr, string WhereStr)
        {
            var arr = Field.Value.Split(';');
            if (arr.Length != 3 && arr.Length != 4) return "";

            var arrGroup = arr[2].Split(',');
            string tatField = ReturnFields.ContainsKey(arr[1]) ? ReturnFields[arr[1]].Name : arr[1];

            //跑 for 產生 fields, where, group 等
            List<string> tl1_group = new List<string>(), tl1_fields = new List<string>();
            List<string> tl2_where = new List<string>();
            List<string> toplast_on = new List<string>();
            tw_Field _field = new tw_Field();
            for (int i = 0; i < arrGroup.Length; i++)
            {
                if (!ReturnFields.ContainsKey(arrGroup[i])) return "";

                _field = ReturnFields[arrGroup[i]];
                if (string.IsNullOrEmpty(_field.Table))
                {
                    tl1_fields.Add(string.Concat(_field.Name, " as [", _field.DisplayName, "]"));
                    tl1_group.Add(_field.Name);
                    tl2_where.Add(string.Concat("tl_1.[", _field.DisplayName, "] = ", _field.Name));
                    toplast_on.Add(string.Concat("toplast_", Idx, ".[", _field.DisplayName, "] = tb.[", _field.DisplayName, "]"));
                }
                else
                {
                    tl1_fields.Add(string.Concat("[", _field.Table, "].[", _field.Name, "]"));
                    tl1_group.Add(string.Concat("[", _field.Table, "].[", _field.Name, "]"));
                    tl2_where.Add(string.Concat("tl_1.[", _field.Name, "] = [", _field.Table, "].[", _field.Name, "]"));
                    toplast_on.Add(string.Concat("toplast_", Idx, ".[", _field.Name, "] = tb.[", _field.DisplayName, "]"));
                }
            }

            //轉換 % 數
            int p = 10;
            if (arr.Length == 4)
                int.TryParse(arr[3], out p);
            if (p <= 0)
                p = 10;
            double p2 = Math.Round((double)p / 100, 2, MidpointRounding.AwayFromZero);

            //order by
            string orderby1 = string.Concat(tatField, (arr[0] == "top" ? " asc" : " desc"));
            string orderby2 = string.Concat(tatField, (arr[0] == "top" ? " desc" : " asc"));

            //組平均數或第幾筆語法
            string fieldFormatStr = "";
            switch (Field.Fx)
            {
                //取前後10筆(或n筆)平均數
                case "top_10p":
                case "last_10p":
                case "toplast_np":
                    fieldFormatStr = string.Concat("cast(round(sum(", tatField, ") / tl_1.GetRowNum, 2) as numeric(20, 2)) ", tatField);
                    break;
                //取第10筆
                case "top_10p_nth":
                case "last_10p_nth":
                    fieldFormatStr = tatField;
                    break;
            }

            string sql = string.Concat(@"
                join (
 		                select 
			                *,
			                (
				                select top 1 ", fieldFormatStr, @"
				                from (
					                select top(tl_1.GetRowNum) *
					                from ", FormStr, @"
					                where", (string.IsNullOrEmpty(WhereStr) ? "" : WhereStr + " and ") + @"
                                        ", (string.Join(" and ", tl2_where.ToArray())), @"
					                order by ", orderby1, @"
					                ) tl_2
				                order by ", orderby2, @"
			                ) [", Field.DisplayName, @"]
		                from (
			                select 
				                cast(ceiling(count(sys_ID) * ", p2, @") as int) GetRowNum, 
				                ", (string.Join(", ", tl1_fields.ToArray())), @"
			                from ", FormStr, @"
			                ", (string.IsNullOrEmpty(WhereStr) ? "" : "where " + WhereStr) + @"
			                group by ", (string.Join(", ", tl1_group.ToArray())), @"
			                ) tl_1
	                ) toplast_", Idx, @"
		                on ", (string.Join(" and ", toplast_on.ToArray())), @"
                ");

            return sql;
        }

        public Dictionary<string, string> TableFieldPrefix = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Dictionary<string, string>> dicTableFields = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        int TableFieldPrefixIdx = 0;
        private string GetFieldPrefix()
        {
            string[] arr = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            return arr[TableFieldPrefixIdx++];
        }

        private string setValueFormat(string dtype, string doperator, string val, string fx)
        {
            if (val == "null")
                return val;

            var _val = "";
            bool isFx = val.ToLower().IndexOf("dateadd(") == 0 || val.ToLower().IndexOf("count(") == 0 || val.ToLower().IndexOf("isnull(") == 0 ||
                val.ToLower().IndexOf("sum(") == 0 || val.ToLower().IndexOf("(sum(") == 0 || val.ToLower().IndexOf("(case ") == 0 || val.ToLower().IndexOf("convert(") == 0 ||
                val.ToLower().IndexOf("datediff(") == 0 || val.ToLower().IndexOf("datepart(") == 0 || val.ToLower().IndexOf("round(") == 0 || val.ToLower().IndexOf("datepart(") == 0 ||
                val.ToLower().IndexOf("max(") == 0 || val.ToLower().IndexOf("min(") == 0 || val.ToLower().IndexOf("avg(") == 0 ||
                val.ToLower().IndexOf("cast(") == 0 || val.ToLower() == "{sysid}" || val.ToLower() == "{bid}";

            switch (dtype)
            {
                case "varchar":
                case "nvarchar":
                    if (!string.IsNullOrEmpty(fx) || isFx)
                        _val = val;
                    else
                    {
                        switch (doperator)
                        {
                            case "in":
                            case "not in":
                                _val = "(" + val + ")";
                                break;
                            default:
                                if ((val.IndexOf("'") == 0 && val.Substring(val.Length - 1, 1) == "'") || val.IndexOf("N'") == 0)
                                    _val = val;
                                else
                                    _val = (val == "{sysid}" || val == "{bid}") ? val : "N'" + val + "'";
                                break;
                        }
                    }
                    break;
                case "float":
                case "decimal":
                    float _float = 0;
                    if (!string.IsNullOrEmpty(fx) || float.TryParse(val, out _float) || isFx)
                        _val = val;
                    else
                    {
                        switch (doperator)
                        {
                            case "in":
                            case "not in":
                                _val = "(" + val + ")";
                                break;
                            default:
                                _val = "0";
                                break;
                        }
                    }
                    break;
                case "int":
                    int _int = 0;
                    if (!string.IsNullOrEmpty(fx) || int.TryParse(val, out _int) || isFx)
                        _val = val;
                    else
                    {
                        switch (doperator)
                        {
                            case "in":
                            case "not in":
                                _val = "(" + val + ")";
                                break;
                            default:
                                _val = "0";
                                break;
                        }
                    }
                    break;
                case "datetime":
                    DateTime _datetime = DateTime.Now;
                    if (DateTime.TryParse(val, out _datetime))
                    {

                        switch (doperator)
                        {
                            case "in":
                            case "not in":
                                _val = "(" + val + ")";
                                break;
                            default:
                                _val = "'" + val + "'";
                                break;
                        }
                    }
                    else if (val == "getdate()")
                        _val = "getdate()";
                    else if (!string.IsNullOrEmpty(fx))
                        _val = val;
                    else if (isFx)
                        _val = val;
                    else
                        _val = "''";
                    break;
            }
            return _val;
        }
        private string GetJoinTable(List<tw_JoinTable> t, string SourceAction)
        {
            string s_source_tables = "";
            if (t == null || t.Count == 0) return "";

            for (var i = 0; i < t.Count; i++)
            {
                if (t[i].Items != null && t[i].Items.Count > 0)
                {
                    var _s_tables = "";
                    for (var j = 0; j < t[i].Items.Count; j++)
                    {
                        var _table = ConvertSql(t[i].Items[j], SourceAction);
                        _s_tables += "(" + _table + ") " + t[i].Table;
                    }
                    s_source_tables += _s_tables;
                }
                else
                {
                    s_source_tables += "[" + t[i].Table + "]";
                }
            }

            return s_source_tables;
        }
        private string GetWhere(List<tw_Where> t)
        {
            bool isTBW = false;
            string s_where = "", __table = "";
            if (t == null || t.Count == 0) return "";

            //組條件
            for (var i = 0; i < t.Count; i++)
            {
                isTBW = false;
                if (t[i].Formula == "null")
                {
                    switch (t[i].Operator)
                    {
                        case "=":
                            t[i].Operator = "is";
                            break;
                        case "<>":
                            t[i].Operator = "is not";
                            break;
                    }
                }

                __table = t[i].Table;
                if (__table.EndsWith("_tbw_dw"))
                    isTBW = true;


                if (dicTableFields.ContainsKey(__table) && dicTableFields[__table].ContainsKey(t[i].Field))
                    t[i].Field = dicTableFields[__table][t[i].Field];

                s_where += (i > 0 ? " and " : "") + "[" + __table + "].[" + t[i].Field + "] " + t[i].Operator + " " + setValueFormat(t[i].FieldType, t[i].Operator, t[i].Formula, t[i].Fx);
            }

            return s_where;
        }

        public virtual bool CopyData(CopyData_Param c)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                string sql = string.Format("select top 1 * from DW_TableWidget where ActionName = @ActionName and PortalID = {0}", PortalID);

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
                    sql = @"
                        INSERT INTO [dbo].[DW_TableWidget]
                            ([sys_CreateDate]
                            ,[sys_CreateUser]
                            ,[ActionName]
                            ,[ActionType]
                            ,[SQLString]
                            ,[JSONString]
                            ,[TableName]
                            ,[PortalID])
                        VALUES
                            (getdate()
                            ,@sys_CreateUser
                            ,@ActionName
                            ,@ActionType
                            ,@SQLString
                            ,@JSONString
                            ,@TableName
                            ,@PortalID)";
                    arParms = new SqlParameter[7];
                    arParms[0] = new SqlParameter("@sys_CreateUser", SqlDbType.NVarChar);
                    arParms[0].Value = c.LoginUser;
                    arParms[1] = new SqlParameter("@ActionName", SqlDbType.NVarChar);
                    arParms[1].Value = c.TargetActionName;
                    arParms[2] = new SqlParameter("@ActionType", SqlDbType.NVarChar);
                    arParms[2].Value = dt.Rows[0]["ActionType"].ToString();
                    arParms[3] = new SqlParameter("@SQLString", SqlDbType.NVarChar);
                    arParms[3].Value = sqlStr;
                    arParms[4] = new SqlParameter("@JSONString", SqlDbType.NVarChar);
                    arParms[4].Value = ConvertSerialize(_d);
                    arParms[5] = new SqlParameter("@TableName", SqlDbType.NVarChar);
                    arParms[5].Value = dt.Rows[0]["TableName"].ToString();
                    arParms[6] = new SqlParameter("@PortalID", SqlDbType.Int);
                    arParms[6].Value = TargetPortalID;
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
        public tw_Tables ReplaceRoute(tw_Tables d, CopyData_Param c)
        {
            //MainTable
            if (d.MainTable.Equals(c.SourceRouteNo) || d.MainTable.StartsWith(c.SourceRouteNo + "_ST"))
                d.MainTable = d.MainTable.Replace(c.SourceRouteNo, c.TargetRouteNo);

            //Fields
            for (int i = 0; i < d.Fields.Count; i++)
            {
                if (d.Fields[i].Table.Equals(c.SourceRouteNo) || d.Fields[i].Table.StartsWith(c.SourceRouteNo + "_ST"))
                    d.Fields[i].Table = d.Fields[i].Table.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Fields[i].Value.IndexOf(c.SourceRouteNo) > -1)
                    d.Fields[i].Value = d.Fields[i].Value.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Fields[i].DisplayName.IndexOf(c.SourceRouteNo) > -1)
                    d.Fields[i].DisplayName = d.Fields[i].DisplayName.Replace(c.SourceRouteNo, c.TargetRouteNo);
            }

            //Where
            for (int i = 0; i < d.Where.Count; i++)
            {
                if (d.Where[i].Table.Equals(c.SourceRouteNo) || d.Where[i].Table.StartsWith(c.SourceRouteNo + "_ST"))
                    d.Where[i].Table = d.Where[i].Table.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Where[i].Formula.IndexOf(c.SourceRouteNo) > -1)
                    d.Where[i].Formula = d.Where[i].Formula.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Where[i].Field.IndexOf(c.SourceRouteNo) > -1)
                    d.Where[i].Field = d.Where[i].Field.Replace(c.SourceRouteNo, c.TargetRouteNo);
            }

            //Group
            for (int i = 0; i < d.Group.Count; i++)
            {
                if (d.Group[i].Table.Equals(c.SourceRouteNo) || d.Group[i].Table.StartsWith(c.SourceRouteNo + "_ST"))
                    d.Group[i].Table = d.Group[i].Table.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Group[i].Name.IndexOf(c.SourceRouteNo) > -1)
                    d.Group[i].Name = d.Group[i].Name.Replace(c.SourceRouteNo, c.TargetRouteNo);
                if (d.Group[i].DisplayName.IndexOf(c.SourceRouteNo) > -1)
                    d.Group[i].DisplayName = d.Group[i].DisplayName.Replace(c.SourceRouteNo, c.TargetRouteNo);
            }

            //Join
            for (int i = 0; i < d.JoinTable.Count; i++)
            {
                if (d.JoinTable[i].Table.Equals(c.SourceRouteNo) || d.JoinTable[i].Table.StartsWith(c.SourceRouteNo + "_ST"))
                    d.JoinTable[i].Table = d.JoinTable[i].Table.Replace(c.SourceRouteNo, c.TargetRouteNo);

                //On
                for (int j = 0; j < d.JoinTable[i].On.Count; j++)
                {
                    //Join_Table
                    if (d.JoinTable[i].On[j].Join_Table.Equals(c.SourceRouteNo) || d.JoinTable[i].On[j].Join_Table.StartsWith(c.SourceRouteNo + "_ST"))
                        d.JoinTable[i].On[j].Join_Table = d.JoinTable[i].On[j].Join_Table.Replace(c.SourceRouteNo, c.TargetRouteNo);
                    //Target_Table
                    if (d.JoinTable[i].On[j].Target_Table.Equals(c.SourceRouteNo) || d.JoinTable[i].On[j].Target_Table.StartsWith(c.SourceRouteNo + "_ST"))
                        d.JoinTable[i].On[j].Target_Table = d.JoinTable[i].On[j].Target_Table.Replace(c.SourceRouteNo, c.TargetRouteNo);
                    //Where
                    if (d.JoinTable[i].On[j].Where.IndexOf(c.SourceRouteNo) > -1)
                        d.JoinTable[i].On[j].Where = d.JoinTable[i].On[j].Where.Replace(c.SourceRouteNo, c.TargetRouteNo);
                }

                //Items
                if (d.JoinTable[i].Items != null && d.JoinTable[i].Items.Count > 0)
                {
                    for (int j = 0; j < d.JoinTable[i].Items.Count; j++)
                        d.JoinTable[i].Items[j] = ReplaceRoute(d.JoinTable[i].Items[j], c);
                }
            }

            return d;
        }

        /// <summary>
        /// 驗證由數位、26個英文字母或者下劃線組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetterOrUnderline(string input)
        {
            return Regex.IsMatch(input, @"^\w+$");
        }

        /// <summary>
        /// /// 判斷是否有非法字符
        /// /// </summary>
        /// /// <param name="strString"></param>
        /// /// <returns>返回TRUE表示有非法字符，返回FALSE表示沒有非法字符。 </returns>
        public static bool CheckBadStr(string strString)
        {
            if (!string.IsNullOrEmpty(strString))
            {
                string tempStr = strString.ToLower();
                List<string> bidStrlist = new List<string>();
                bidStrlist.Add("'");
                bidStrlist.Add(");");
                bidStrlist.Add("-");
                bidStrlist.Add(":");
                bidStrlist.Add("%");
                bidStrlist.Add("@");
                bidStrlist.Add("&");
                bidStrlist.Add("#");
                bidStrlist.Add("\"");
                bidStrlist.Add("net user");
                bidStrlist.Add("exec");
                bidStrlist.Add("net localgroup");
                //bidStrlist.Add("select");
                //bidStrlist.Add("asc");
                //bidStrlist.Add("char");
                //bidStrlist.Add("mid");
                bidStrlist.Add("insert");
                bidStrlist.Add("update");
                bidStrlist.Add("order");
                bidStrlist.Add("delete");
                bidStrlist.Add("drop");
                bidStrlist.Add("truncate");
                bidStrlist.Add("xp_cmdshell");
                //bidStrlist.Add("<");
                //bidStrlist.Add(">");

                var arr = bidStrlist.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (tempStr.IndexOf(bidStrlist[i]) != -1) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// /// 判斷是否有非法字符
        /// /// </summary>
        /// /// <param name="strString"></param>
        /// /// <returns>返回TRUE表示有非法字符，返回FALSE表示沒有非法字符。 </returns>沒有非法字符。 </returns>
        public static bool CheckBadStr4Where(string strString)
        {
            if (!string.IsNullOrEmpty(strString))
            {
                string tempStr = strString.ToLower();
                List<string> bidStrlist = new List<string>();
                //bidStrlist.Add("'");
                bidStrlist.Add(");");
                bidStrlist.Add("--");
                //bidStrlist.Add(":");
                //bidStrlist.Add("%");
                //bidStrlist.Add("@");
                //bidStrlist.Add("&");
                //bidStrlist.Add("#");
                bidStrlist.Add("\"");
                bidStrlist.Add("net user");
                bidStrlist.Add("exec");
                bidStrlist.Add("net localgroup");
                bidStrlist.Add("select");
                //bidStrlist.Add("asc");
                //bidStrlist.Add("char");
                //bidStrlist.Add("mid");
                bidStrlist.Add("insert");
                bidStrlist.Add("update");
                //bidStrlist.Add("order");
                bidStrlist.Add("exec");
                bidStrlist.Add("delete");
                bidStrlist.Add("drop");
                bidStrlist.Add("truncate");
                bidStrlist.Add("xp_cmdshell");
                //bidStrlist.Add("<");
                //bidStrlist.Add(">");

                var arr = bidStrlist.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (tempStr.IndexOf(bidStrlist[i]) != -1) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// /// 判斷是否有非法字符
        /// /// </summary>
        /// /// <param name="strString"></param>
        /// /// <returns>返回TRUE表示有非法字符，返回FALSE表示沒有非法字符。 </returns>沒有非法字符。 </returns>
        public static bool CheckBadStr4Join(string strString)
        {
            if (!string.IsNullOrEmpty(strString))
            {
                string tempStr = strString.ToLower();
                List<string> bidStrlist = new List<string>();
                bidStrlist.Add("'");
                bidStrlist.Add(");");
                bidStrlist.Add("--");
                bidStrlist.Add(":");
                bidStrlist.Add("%");
                bidStrlist.Add("@");
                bidStrlist.Add("&");
                bidStrlist.Add("#");
                bidStrlist.Add("\"");
                bidStrlist.Add("net user");
                bidStrlist.Add("exec");
                bidStrlist.Add("net localgroup");
                bidStrlist.Add("select");
                //bidStrlist.Add("asc");
                //bidStrlist.Add("char");
                //bidStrlist.Add("mid");
                bidStrlist.Add("insert");
                bidStrlist.Add("update");
                //bidStrlist.Add("order");
                bidStrlist.Add("exec");
                bidStrlist.Add("delete");
                bidStrlist.Add("drop");
                bidStrlist.Add("truncate");
                bidStrlist.Add("xp_cmdshell");
                bidStrlist.Add("<");
                bidStrlist.Add(">");

                var arr = bidStrlist.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (tempStr.IndexOf(bidStrlist[i]) != -1) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// /// 判斷是否有非法字符
        /// /// </summary>
        /// /// <param name="strString"></param>
        /// /// <returns>返回TRUE表示有非法字符，返回FALSE表示沒有非法字符。 </returns>沒有非法字符。 </returns>
        public static bool CheckBadStr4Field(string strString)
        {
            if (!string.IsNullOrEmpty(strString))
            {
                string tempStr = strString.ToLower();
                List<string> bidStrlist = new List<string>();
                //bidStrlist.Add("'");
                bidStrlist.Add(");");
                bidStrlist.Add("--");
                //bidStrlist.Add(":");
                //bidStrlist.Add("%");
                //bidStrlist.Add("@");
                //bidStrlist.Add("&");
                //bidStrlist.Add("#");
                bidStrlist.Add("\"");
                bidStrlist.Add("net user");
                bidStrlist.Add("exec");
                bidStrlist.Add("net localgroup");
                bidStrlist.Add("select");
                //bidStrlist.Add("asc");
                //bidStrlist.Add("char");
                //bidStrlist.Add("mid");
                bidStrlist.Add("insert");
                bidStrlist.Add("update");
                //bidStrlist.Add("order");
                bidStrlist.Add("delete");
                bidStrlist.Add("drop");
                bidStrlist.Add("truncate");
                bidStrlist.Add("xp_cmdshell");
                //bidStrlist.Add("<");
                //bidStrlist.Add(">");

                var arr = bidStrlist.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (tempStr.IndexOf(bidStrlist[i]) != -1) return false;
                }
            }

            return true;
        }
    }
}
