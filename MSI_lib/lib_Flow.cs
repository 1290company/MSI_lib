
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
    public class lib_Flow
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
        public lib_Flow(lstFlow lstobj)
        {
            if (lstobj == null)
                return;
            if (lstobj.objFlowCode == null)
                return;
            if (lstobj.lstSubFlow == null)
                return;
            if (lstobj.lstTypeFlow == null)
                return;
            InsFlow( lstobj);

        }


        public ErrorMessage ChkFlowExist(string FlowCode)
        {
            string sql = "SELECT count(*) FLOWCNT FROM WKF_FLOW_CODE WHERE FLOW_CODE =@FLOW_CODE";
            List<SqlParameter> lstP = new List<SqlParameter>();
            SqlParameter sp1 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
            sp1.Value = FlowCode;
            lstP.Add(sp1);
            DataSet ds =
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql, lstP.ToArray());
            if (ds.Tables[0].Rows[0]["FlowCode"].ToString() != "0")
            {
                return ErrorMessage.FlowExist;
            }

            return ErrorMessage.FlowOK;
        }

        public enum ErrorMessage
        {
            FlowExist = -1,
            FlowOK= 0,
            SystemError =-2

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstobj"></param>
        private ErrorMessage InsFlow(lstFlow lstobj)
        {
            try
            {

 


                //新增 FLOWCODE
                string ins = @"
                                    INSERT INTO WKF_FLOW_CODE(FLOW_CODE, FLOW_SEQNO, FLOW_DESC,FLOW_TABLE,RouteNo)
                                    SELECT @FLOW_CODE,@FLOW_SEQNO, @FLOW_DESC,@FLOW_TABLE,@RouteNo
                                    WHERE NOT EXISTS(SELECT * FROM WKF_FLOW_CODE
                                    WHERE FLOW_CODE = @FLOW_CODE)";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp1 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                sp1.Value = lstobj.objFlowCode.FLOW_CODE;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@FLOW_SEQNO", SqlDbType.NVarChar);
                sp1.Value = DateTime.Now.ToString("yyyyMMdd0000");
                lstP.Add(sp1);
                sp1 = new SqlParameter("@FLOW_DESC", SqlDbType.NVarChar);
                sp1.Value = lstobj.objFlowCode.FLOW_DESC;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@FLOW_TABLE", SqlDbType.NVarChar);
                sp1.Value = lstobj.objFlowCode.FLOW_TABLE;
                lstP.Add(sp1);
                sp1 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                sp1.Value = lstobj.objFlowCode.RouteNo;
                lstP.Add(sp1);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, ins, lstP.ToArray());


                //新增刪除修改TYPEFLOW
                //先新增沒有的
                foreach (var item in lstobj.lstSubFlow)
                {
                    ins = @"   INSERT INTO WKF_SUB_FLOW(RouteNo, FLOW_CODE, FLOW_NO,FLOW_DESC,FLOW_USE,SREF_VAILD,SREF_FIELD5,FLOW_ROUT_USE,SREF_VAILD6,
                                    FLOW_DISABLE,SREF_FIELD6,SREF_VAILD7)
                                    SELECT @RouteNo, @FLOW_CODE, @FLOW_NO,@FLOW_DESC,@FLOW_USE,@SREF_VAILD,@SREF_FIELD5,@FLOW_ROUT_USE,@SREF_VAILD6,
                                    @FLOW_DISABLE,@SREF_FIELD6,@SREF_VAILD7
                                    WHERE NOT EXISTS(SELECT * FROM WKF_SUB_FLOW
                                    WHERE FLOW_CODE = @FLOW_CODE AND FLOW_NO=@FLOW_NO) ";
                    List<SqlParameter> lstSubFlow = new List<SqlParameter>();
                    SqlParameter sp2 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp2.Value = item.RouteNo;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_CODE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_NO", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_NO;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_DESC", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_DESC;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_USE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_USE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_FIELD5", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_FIELD5;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_ROUT_USE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_ROUT_USE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD6", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD6;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_DISABLE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_DISABLE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_FIELD6", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_FIELD6;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD7", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD7;
                    lstSubFlow.Add(sp2);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, ins, lstSubFlow.ToArray());
                }

                //修改存在的
                foreach (var item in lstobj.lstSubFlow)
                {
                    string upd = @" UPDATE WKF_SUB_FLOW SET RouteNo =@RouteNo, FLOW_CODE=@FLOW_CODE, 
                                        FLOW_NO=@FLOW_NO,FLOW_DESC=@FLOW_DESC,FLOW_USE=@FLOW_USE,
                                        SREF_VAILD=@SREF_VAILD,SREF_FIELD5=@SREF_FIELD5,
                                        FLOW_ROUT_USE=@FLOW_ROUT_USE,SREF_VAILD6=@SREF_VAILD6,
                                        FLOW_DISABLE=@FLOW_DISABLE,SREF_FIELD6=@SREF_FIELD6,SREF_VAILD7=@SREF_VAILD7 
                                        Where FLOW_CODE = @FLOW_CODE AND FLOW_NO=@FLOW_NO;";

                    List<SqlParameter> lstSubFlow = new List<SqlParameter>();
                    SqlParameter sp2 = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp2.Value = item.RouteNo;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_CODE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_NO", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_NO;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_DESC", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_DESC;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_USE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_USE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_FIELD5", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_FIELD5;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_ROUT_USE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_ROUT_USE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD6", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD6;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@FLOW_DISABLE", SqlDbType.NVarChar);
                    sp2.Value = item.FLOW_DISABLE;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_FIELD6", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_FIELD6;
                    lstSubFlow.Add(sp2);
                    sp2 = new SqlParameter("@SREF_VAILD7", SqlDbType.NVarChar);
                    sp2.Value = item.SREF_VAILD7;
                    lstSubFlow.Add(sp2);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, upd, lstSubFlow.ToArray());
                }

                //update 不存在的
                string join = "'" + string.Join("','", lstobj.lstSubFlow.Select(x => x.FLOW_NO)) + "'";
                string del = @" UPDATE  WKF_SUB_FLOW SET  FLOW_USE =0 WHERE  FLOW_CODE  =@FLOW_CODE  AND  FLOW_NO NOT IN ("+ join + ")";
                List<SqlParameter> lstSubFlow1 = new List<SqlParameter>();
                SqlParameter sp3 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                sp3.Value = lstobj.objFlowCode.FLOW_CODE;
                lstSubFlow1.Add(sp3);          
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, del, lstSubFlow1.ToArray());

                //
                foreach (var item in lstobj.lstTypeFlow)
                {
                     ins = @" INSERT INTO WKF_TYPE_FLOW (FLOW_CODE,SEQNO,FLOW_NO,RouteNo) SELECT @FLOW_CODE,@SEQNO,@FLOW_NO,@RouteNo 
                                    WHERE NOT EXISTS(SELECT * FROM WKF_TYPE_FLOW
                                    WHERE FLOW_CODE = @FLOW_CODE AND FLOW_NO=@FLOW_NO) ";
                    List<SqlParameter> lstTypeFlow = new List<SqlParameter>();
                    SqlParameter sp = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                    sp.Value = item.FlowCode;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@SEQNO", SqlDbType.NVarChar);
                    sp.Value = item.SEQNO;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@FLOW_NO", SqlDbType.NVarChar);
                    sp.Value = item.FLOW_NO;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp.Value = item.RouteNo;
                    lstTypeFlow.Add(sp);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, ins, lstTypeFlow.ToArray());

                }

                foreach (var item in lstobj.lstTypeFlow)
                {
                    ins = @" UPDATE WKF_TYPE_FLOW SET FLOW_CODE=@FLOW_CODE,SEQNO=@SEQNO,FLOW_NO=@FLOW_NO,RouteNo =@RouteNo WHERE FLOW_CODE=@FLOW_CODE AND FLOW_NO=@FLOW_NO";
                    List<SqlParameter> lstTypeFlow = new List<SqlParameter>();
                    SqlParameter sp = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                    sp.Value = item.FlowCode;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@SEQNO", SqlDbType.NVarChar);
                    sp.Value = item.SEQNO;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@FLOW_NO", SqlDbType.NVarChar);
                    sp.Value = item.FLOW_NO;
                    lstTypeFlow.Add(sp);
                    sp = new SqlParameter("@RouteNo", SqlDbType.NVarChar);
                    sp.Value = item.RouteNo;
                    lstTypeFlow.Add(sp);
                    Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, ins, lstTypeFlow.ToArray());

                }
                 join = "'" + string.Join("','", lstobj.lstTypeFlow.Select(x => x.FLOW_NO)) + "'";
                //update 不存在的
                del = @" DELETE  WKF_TYPE_FLOW WHERE  FLOW_CODE  =@FLOW_CODE  AND  FLOW_NO NOT IN ("+ join + ")";
                List<SqlParameter> lstTypeFlow1 = new List<SqlParameter>();
                SqlParameter sp4 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                sp4.Value = lstobj.objFlowCode.FLOW_CODE;
                lstTypeFlow1.Add(sp4);
     
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, del, lstTypeFlow1.ToArray());
               return    ErrorMessage.FlowOK;
            }
            catch (Exception ex)
            {
                string del = @" DELETE  WKF_TYPE_FLOW WHERE  FLOW_CODE  =@FLOW_CODE;DELETE FROM WKF_FLOW_CODE WHERE FLOW_CODE =@FLOW_CODE ; DELETE FROM WKF_SUB_FLOW WHERE FLOW_CODE=@FLOW_CODE  ";
                List<SqlParameter> lstP = new List<SqlParameter>();
                SqlParameter sp4 = new SqlParameter("@FLOW_CODE", SqlDbType.NVarChar);
                sp4.Value = lstobj.objFlowCode.FLOW_CODE;
                lstP.Add(sp4);
                sp4 = new SqlParameter("@JOIN_FLOW_NO", SqlDbType.NVarChar);
                sp4.Value = "'" + string.Join("','", lstobj.lstTypeFlow.Select(x => x.FLOW_NO)) + "'";
                lstP.Add(sp4);
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, del, lstP.ToArray());
                return ErrorMessage.SystemError;
                //throw new Exception();
            }
        }


        public class lstFlow
        {
            public List<TypeFlow> lstTypeFlow;
            public FlowCode objFlowCode;
            public List<SUB_FLOW> lstSubFlow;
        }

        public class TypeFlow
        {
            public string FlowCode { set; get; }
            public string SEQNO { set; get; }
            public string FLOW_NO { set; get; }
            public string RouteNo { set; get; }
        }

        public class FlowCode
        {
            public string FLOW_CODE { set; get; }
            public string FLOW_SEQNO { set; get; }
            public string FLOW_DESC { set; get; }
            public string FLOW_TABLE { set; get; }
            public string RouteNo { set; get; }
        }

        public class SUB_FLOW
        {
            public string RouteNo { set; get; }
            public string FLOW_CODE { set; get; }
            public string FLOW_NO { set; get; }
            public string FLOW_DESC { set; get; }
            public string FLOW_USE { set; get; }      
            public string SREF_VAILD { set; get; }
            public string SREF_FIELD5 { set; get; }
            public string FLOW_ROUT_USE { set; get; }
            public string SREF_VAILD6 { set; get; }
            public string FLOW_DISABLE { set; get; }     
            public string SREF_FIELD6 { set; get; }
            public string SREF_VAILD7 { set; get; }


        }





    
    }
}
