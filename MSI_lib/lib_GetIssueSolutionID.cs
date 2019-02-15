using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class lib_GetIssueSolutionID : lib_ChatBase
    {
        string Connection = System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"] ?? "";
      
        public override string FunctionName()
        {
            return "";
            // throw new NotImplementedException();
        }


        //string tab = "\\t";
        //string NewLine = "\\r\\n";
        string tab = "     ";
        string NewLine = "<br>";

        public override string Init(string O)
        {

            string PushmailID = O;
            String AutoMail = "SELECT * FROM AUTOMAIL WHERE SYS_ID=@SYS_ID";            
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@SYS_ID", PushmailID));
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset
                (Connection, System.Data.CommandType.Text, AutoMail, parameters.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
                return "找不到流程";

            string RouteNo = ds.Tables[0].Rows[0]["RouteNo"].ToString();            
            string ISSUE_SOLUTOIN_ROUTENO = ds.Tables[0].Rows[0]["ISFlow"].ToString() ;
            string ISSUE_SOLUTOIN_ROUTENO_TABLE = ds.Tables[0].Rows[0]["ISFlow"].ToString() + "_ST1";
            string Content = ds.Tables[0].Rows[0]["Context"].ToString();
            String GetIssueSolution = "SELECT * FROM " + ISSUE_SOLUTOIN_ROUTENO_TABLE + " WHERE sys_updateUser IS NOT NULL ORDER BY sys_CreateDate DESC ";
            DataSet dsIssue = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset
                (Connection, System.Data.CommandType.Text, GetIssueSolution, null);
            if (dsIssue.Tables[0].Rows.Count == 0)
                return "找不到ISSUE SOLUTION 資料表";

            string IssueSolutionField = "SELECT * FROM DW_FIELD WHERE TableName=@TableName";
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TableName", ISSUE_SOLUTOIN_ROUTENO_TABLE));
            DataSet dsField = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset
             (Connection, System.Data.CommandType.Text, IssueSolutionField, parameters.ToArray());
            string Header = "";
            string Data = "";
            Dictionary<string, string> di = new Dictionary<string, string>();

            foreach (DataRow r in dsField.Tables[0].Rows)
            {
                Header += r["DisplayName"].ToString() + tab;
                if (!di.ContainsKey(r["Field"].ToString()))
                {
                    di.Add(r["Field"].ToString(), r["Field"].ToString());
                }
            }
            Header += "========================================" + NewLine;
            foreach (DataRow r in dsIssue.Tables[0].Rows)
            {
                foreach (KeyValuePair<string,string> o in di)
                {                    
                    Data += r[o.Key].ToString() + tab;
                }
                Data += NewLine;                
            }
            return Header + Data;


            /*
            string TableName = RouteNo + "_ST1";

            string SQL = @"SELECT * FROM rcpm_AGW_st1 WHERE sys_UpdateDate IS NULL ";
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Connection, System.Data.CommandType.Text, SQL, null);

            string PushField = @"SELECT * FROM rcpm_AGW_st1 WHERE sys_UpdateDate IS NULL ";
            DataSet dsPush = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Connection, System.Data.CommandType.Text, PushField, null);






            String Header = "編號\\t\\t流程名稱\\t\\tPushmail名字\\r\\n";
            Header += "================================\\r\\n";
            string Footer = "================================\\r\\n";
            string Message = "@ID={請填寫}\\r\\n";
            String Data = "";
            int i = 0;
            foreach (DataRow o in ds.Tables[0].Rows)
            {
                Data += o["sys_id"].ToString() + "\\t\\t" + o["ISFlow"].ToString() + "\\t\\t" + o["Subject"].ToString();
                Data += "\\r\\n";
            }
            return Header + Data + Footer + Message;            

            */
        }



    }
}
