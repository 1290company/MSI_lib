using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class lib_GetIssueSolutionRouteNo : lib_ChatBase
    {
        string Connection = System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"] ?? "";
     
        public override string FunctionName()
        {
            return "";
           // throw new NotImplementedException();
        }




        public override string Init(string O)
        {            
            string SQL = @"SELECT SYS_ID,ISFlow,Subject FROM AutoMail where ISFlow IS NOT NULL AND Rtrim(ISFlow) <> ''";
            DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(Connection, System.Data.CommandType.Text, SQL, null);
            String Header = "編號\\t\\t流程名稱\\t\\tPushmail名字\\r\\n";
            Header += "================================\\r\\n";
            string Footer = "================================\\r\\n";
            string Message = "請填寫ID後回傳\\r\\n";
            String Data = "";
            int i = 0;
            foreach (DataRow o in ds.Tables[0].Rows)
            {
                Data += o["sys_id"].ToString() + "\\t\\t" + o["ISFlow"].ToString() + "\\t\\t" + o["Subject"].ToString();
                Data += "\\r\\n";
            }
            return Header + Data + Footer + Message;            
        }



    }
}
