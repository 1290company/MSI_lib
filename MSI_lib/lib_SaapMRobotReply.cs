using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class ClsKey
    {
        public string sorting { set; get; }
        public string Field { set; get; }
        public List<ddlData> DropDown { set; get; }
        public string Level2ID { set; get; }

        public string Lever3ID { set; get; }
        public string Reply { set; get; }
    }


    public class ddlData
    {
        public string Text { set; get; }
        public string Value { set; get; }
    }
    public class FAQ {
        public string TemplateName { get; set; }
        public string RouteNo { get; set; }
    }
    public class FAQAns {
        public string ID { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string AfterID { get; set; }
    }


  
    public class lib_SaapMRobotReply
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        string TableName = "";
        string sys_id = "";
        

        string TreeTable = "RCPM_AO5_ST1";

        public void GetTableName(string chat_id)
        {
            string sql = @"select RouteNo,TableName,f_id from DW_CHAT_DATA Where sys_id =@chat_id";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@chat_id", chat_id));
            DataSet ds = SqlHelper.ExecuteDataset(
                                ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
                return;

            TableName = ds.Tables[0].Rows[0]["TableName"].ToString();
            sys_id = ds.Tables[0].Rows[0]["f_id"].ToString();
            TreeTable =(ds.Tables[0].Rows[0]["RouteNo"] == System.DBNull.Value || ds.Tables[0].Rows[0]["RouteNo"].ToString()=="") ? "RCPM_AO5_ST1" :  ds.Tables[0].Rows[0]["RouteNo"].ToString() + "_ST1";
        }


        public DataSet GetData(string Where)
        {
            string sql = "select * from " + TableName + " where sys_id=@sys_id  and " + Where;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@sys_id", sys_id));
            DataSet ds = SqlHelper.ExecuteDataset(
                          ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());
            return ds;
        }


        //public List<ClsKey> GetLevel2DropdownField(string ParentID)
        //{
        //    string sql = " select * from rcpm_AMS_ST1 where st1_field1 = " + ParentID + " order by sys_id ";//  => 第二層"
        //    DataSet ds = SqlHelper.ExecuteDataset(
        //                 ConnectionString, System.Data.CommandType.Text, sql, null);
        //    List<ClsKey> lstKey = new List<ClsKey>();
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        String result = Regex.Replace(ds.Tables[0].Rows[i]["ST1_FIELD2"].ToString(), @"<[^>]*>", String.Empty).Replace("&#39;", "'");
        //        ClsKey obj = new ClsKey()
        //        {
        //            sorting = i.ToString("00"),

        //            Field = result,
        //            Level2ID = ds.Tables[0].Rows[i]["sys_id"].ToString(),
        //        };

        //        lstKey.Add(obj);
        //    }
        //    return lstKey;
        //}

        public List<ClsKey>   GetLevel2DropdownField()
        {
            string sql = "select * from " + TreeTable + " where st1_field1 = (select sys_id from  " + TreeTable + " where st1_field1 = 0) order by sys_id ";//  => 第二層"
            DataSet ds = SqlHelper.ExecuteDataset(
                         ConnectionString, System.Data.CommandType.Text, sql, null);
            List<ClsKey> lstKey = new List<ClsKey>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                String result = Regex.Replace(ds.Tables[0].Rows[i]["ST1_FIELD2"].ToString(), @"<[^>]*>", String.Empty).Replace("&#39;", "'");
                ClsKey obj = new ClsKey()
                {
                    sorting = i.ToString("00"),
                  
                Field = result,
                    Level2ID = ds.Tables[0].Rows[i]["sys_id"].ToString(),
                };

                lstKey.Add(obj);
            }
            return lstKey;
        }

        public List<ClsKey> GetDropdownList(List<ClsKey> lstKey)
        {
            foreach (var o in lstKey)
            {
                if (o.Lever3ID == null)
                    continue;
                string Level3ID = o.Lever3ID;
                string sql = "select * from "+ TreeTable + " where st1_field1 =@sys_id";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@sys_id", Level3ID));
                DataSet ds = SqlHelper.ExecuteDataset(
                             ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());
                List<ddlData> lstddl = new List<ddlData>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlData ddl = new ddlData()
                    {
                        Text = ds.Tables[0].Rows[i]["ST1_FIELD2"].ToString(),
                        Value = ds.Tables[0].Rows[i]["sys_id"].ToString()
                    };
                    lstddl.Add(ddl);
                }

                o.DropDown = lstddl;
            }
            var q = from c in lstKey where c.Lever3ID != null select c;
            return q.ToList< ClsKey>();
        }

        public List<ClsKey> GetDropdownList(List<ClsKey> lstKey,string ParentID)
        {
            foreach (var o in lstKey)
            {
                string Level3ID = o.Lever3ID;
                string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@sys_id", ParentID));
                DataSet ds = SqlHelper.ExecuteDataset(
                             ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());
                List<ddlData> lstddl = new List<ddlData>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlData ddl = new ddlData()
                    {
                        Text = ds.Tables[0].Rows[i]["ST1_FIELD2"].ToString(),
                        Value = ds.Tables[0].Rows[i]["sys_id"].ToString()
                    };
                    lstddl.Add(ddl);
                }

                o.DropDown = lstddl;
            }

            return lstKey;
        }

        public List<FAQ> GetFAQ()
        {
            //string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
            string sql = @"select TemplateName,(select top 1 RouteNo RouteNo  from Autogen_TmplFlowMapping where TmplFormId = A.sys_ID ) as RouteNo from ( 
select * From Autogen_TmplForm where sys_id in ( 
select TmplFormId from Autogen_TmplFlowMapping where routeno IN( 
select RouteNo from SRLT_ROUTE_ST_FIELD where RouteNo like @RouteNo AND FormValue =@FormValue AND DefaultValue =@DefaultValue 
) 
) 
) A";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@RouteNo", "DEMOPOW_%"));
            parameters.Add(new SqlParameter("@FormValue", "IsTreeKey"));
            parameters.Add(new SqlParameter("@DefaultValue", "FAQ"));
            DataSet ds = SqlHelper.ExecuteDataset(
                         ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());

            List<FAQ> lstFAQ = new List<FAQ>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FAQ fam = new FAQ()
                {
                    TemplateName = ds.Tables[0].Rows[i]["TemplateName"].ToString(),
                    RouteNo = ds.Tables[0].Rows[i]["RouteNo"].ToString(),
                };
                lstFAQ.Add(fam);
            }
            return lstFAQ;
        }

        public List<FAQAns> GetFAQAns(string TableName,string AnsID)
        {
            //string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
            string sql = @"select * from " + TableName + @"_St1 where St1_FIELD1 =@St1_FIELD1 ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@St1_FIELD1", AnsID));
            DataSet ds = SqlHelper.ExecuteDataset(
                         ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());

            List<FAQAns> lstFAQAns = new List<FAQAns>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FAQAns fam = new FAQAns()
                {
                    ID = ds.Tables[0].Rows[i]["sys_id"].ToString(),
                    AfterID = ds.Tables[0].Rows[i]["St1_FIELD1"].ToString(),
                    Text = ds.Tables[0].Rows[i]["St1_FIELD2"].ToString(),
                    Value = ds.Tables[0].Rows[i]["St1_FIELD3"].ToString(),
                };
                lstFAQAns.Add(fam);
            }
            return lstFAQAns;
        }
        public List<FAQAns> GetFAQAnsReturn(string TableName, string sys_id)
        {
            //string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
            string sql = @"select * from " + TableName + @"_St1 where St1_FIELD1 in  ( select St1_FIELD1 from " + TableName + @"_St1 where sys_id =@sys_id) ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@sys_id", sys_id));
            DataSet ds = SqlHelper.ExecuteDataset(
                         ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());

            List<FAQAns> lstFAQAns = new List<FAQAns>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FAQAns fam = new FAQAns()
                {
                    ID = ds.Tables[0].Rows[i]["sys_id"].ToString(),
                    AfterID = ds.Tables[0].Rows[i]["St1_FIELD1"].ToString(),
                    Text = ds.Tables[0].Rows[i]["St1_FIELD2"].ToString(),
                    Value = ds.Tables[0].Rows[i]["St1_FIELD3"].ToString(),
                };
                lstFAQAns.Add(fam);
            }
            return lstFAQAns;
        }

        //public List<ClsKey> GetKeyDropdown(string ChatID,string ParentID)
        //{
        //    List<ClsKey> lstKey = GetLevel2DropdownField();
        //    GetTableName(ChatID);
        //    //  DataSet sourceds= GetData();
        //    foreach (var o in lstKey)
        //    {
        //        string sql = "select * from rcpm_AMS_ST1 where st1_field1 =@sys_id";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@sys_id", o.Level2ID));
        //        DataSet ds = SqlHelper.ExecuteDataset(
        //                     ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());

        //        if (ds.Tables[0].Rows.Count == 0)
        //            continue;
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            string Where = dr["ST1_FIELD2"].ToString();
        //            String result = Regex.Replace(Where, @"<[^>]*>", String.Empty).Replace("&#39;", "'");
        //            DataSet dsWhere = GetData(result);
        //            if (dsWhere.Tables[0].Rows.Count != 0)
        //            {
        //                o.Lever3ID = dr["sys_id"].ToString();
        //            }

        //        }
        //    }

        //    return GetDropdownList(lstKey);

        //}

        public List<ClsKey> GetKeyDropdown(string ChatID,string ParentID="0")
        {
            GetTableName(ChatID);
            List<ClsKey> lstKey = new List<ClsKey>();
            lstKey = GetLevel2DropdownField();
            //if (ParentID == "0")
            // lstKey = GetLevel2DropdownField();
            //else
            // lstKey = GetLevel2DropdownField(ParentID);
         
            //  DataSet sourceds= GetData();
            foreach (var o in lstKey)
            {
                string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@sys_id", o.Level2ID));
                DataSet ds = SqlHelper.ExecuteDataset(
                             ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());

                if (ds.Tables[0].Rows.Count == 0)
                    continue;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string Where = dr["ST1_FIELD2"].ToString();
                    String result = Regex.Replace(Where, @"<[^>]*>", String.Empty).Replace("&#39;", "'");
                    DataSet dsWhere = GetData(result);
                    if (dsWhere.Tables[0].Rows.Count != 0)
                    {
                        o.Lever3ID = dr["sys_id"].ToString();
                    }

                }
            }

            if (ParentID =="0"|| ParentID == "")
                return GetDropdownList(lstKey);
            else
                return GetDropdownList(lstKey, ParentID);

        }


        public string GetReply(string ParentID,string ChatID)
        {
            GetTableName(ChatID);
            string html = "";
            string str = "請點選我回覆";
            string sql = "select * from  " + TreeTable + " where st1_field1 =@sys_id";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@sys_id", ParentID));
            DataSet ds = SqlHelper.ExecuteDataset(
                         ConnectionString, System.Data.CommandType.Text, sql, parameters.ToArray());
            if (ds.Tables[0].Rows.Count != 0)
            {
                string content = ds.Tables[0].Rows[0]["ST1_FIELD2"].ToString();
                content = Regex.Replace(content, @"<[^>]*>", String.Empty);
                if (content.IndexOf("[") > -1 && content.IndexOf("]") > -1)
                {
                    string sql1 = "select f_id,TableName from DW_CHAT_DATA where sys_id =@ChatID";
                    List<SqlParameter> plist = new List<SqlParameter>();
                    plist.Add(new SqlParameter("@ChatID", ChatID));
                    DataSet dschat = SqlHelper.ExecuteDataset(
                                 ConnectionString, System.Data.CommandType.Text, sql1, plist.ToArray());
                    if (dschat.Tables[0].Rows.Count > 0)
                    {
                        string table = dschat.Tables[0].Rows[0]["TableName"].ToString();
                        string f_id = dschat.Tables[0].Rows[0]["f_id"].ToString();
                        string Field = "";
                        string[] field = content.Split('[');                        
                        string[] field1 = field[1].Split(']');
                        Field = field1[0].ToString();

                        string data = "select " + Field + " from " + table + " Where sys_id=" + f_id;
                        DataSet dsFieldData = SqlHelper.ExecuteDataset(
                             ConnectionString, System.Data.CommandType.Text, data, null);

                        if (dsFieldData.Tables[0].Rows.Count > 0)
                        {
                            string pid = ds.Tables[0].Rows[0]["sys_id"].ToString();
                            content = content.Replace("[" + Field + "]", dsFieldData.Tables[0].Rows[0][Field.Replace("[", "").Replace("]", "")].ToString());
                            string sql2 = "select sys_id from " + TreeTable + " Where st1_field1=@ParentID";
                            List<SqlParameter> plist2 = new List<SqlParameter>();
                            plist2.Add(new SqlParameter("@ParentID", pid));
                            DataSet dsNext = SqlHelper.ExecuteDataset(
                                 ConnectionString, System.Data.CommandType.Text, sql2, plist2.ToArray());
                            if (dsNext.Tables[0].Rows.Count != 0)
                            {
                                html = content + "<input type=\"button\" onclick=\"SetReview(" + ChatID + "," + pid + ")\" value=\"" + str + "\">";
                            }
                            else
                            {
                                html = content;
                            }
                           


                        }
                    }
                }
                else
                {
                     html = content + "<input type=\"button\" onclick=\"SetReview(" + ChatID + "," + ds.Tables[0].Rows[0]["sys_id"].ToString() + ")\" value=\"" + str + "\">";
                }
                



                return html;// + "|" + ds.Tables[0].Rows[0]["sys_id"].ToString();
            }
            

            return "";
        }


    }
}
