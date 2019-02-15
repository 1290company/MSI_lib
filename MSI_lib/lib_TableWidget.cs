using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MSI_lib
{
    public class lib_TableWidget
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
            public string Operator { get; set; }
            public string Formula { get; set; }
        }

        public class tw_Group
        {
            public string Table { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
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
            public string Stringify { get; set; }
        }

        public class AllItems
        {
            public List<tw_Tables> Items { get; set; }
        }
        #endregion

        public int PortalID { get; set; }

        /// <summary>
        /// TableWidget
        /// </summary>
        /// <param name="_PortalID">PortalID</param>
        public lib_TableWidget(int _PortalID)
        {
            PortalID = _PortalID;
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
    }
}
