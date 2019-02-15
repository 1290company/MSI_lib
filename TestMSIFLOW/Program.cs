using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMSIFLOW
{
    class Program
    {
        private static void CheckUpd()
        {
            //2.測試修改流程 (第二站與第一站交換) ,*FLOW_NO 不能替換 , 
            MSI_lib.lib_Flow.FlowCode objFlowCode = new MSI_lib.lib_Flow.FlowCode();
            objFlowCode.FLOW_CODE = "RRR"; //流程代碼
            objFlowCode.FLOW_DESC = "rainny testing Flow";  //流程說明
            objFlowCode.FLOW_SEQNO = ""; //這個會幫算不用填
            objFlowCode.FLOW_TABLE = "v_CB_RRR_DW";
            objFlowCode.RouteNo = "CB_RRR_DW";
            List<MSI_lib.lib_Flow.SUB_FLOW> lstSubFlow = new List<MSI_lib.lib_Flow.SUB_FLOW>();
            MSI_lib.lib_Flow.SUB_FLOW objSubFlow = new MSI_lib.lib_Flow.SUB_FLOW();  //站點
            objSubFlow.FLOW_CODE = "RRR"; //流程代碼
            objSubFlow.FLOW_DESC = "第二站";
            objSubFlow.FLOW_DISABLE = "";//  填空白
            objSubFlow.FLOW_NO = "10";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objSubFlow.FLOW_ROUT_USE = "";//填空白
            objSubFlow.FLOW_USE = "1";//1是起用,0是不使用;
            objSubFlow.RouteNo = "CB_RRR_DW";
            objSubFlow.SREF_FIELD5 = ""; //填空白
            objSubFlow.SREF_FIELD6 = ""; //填空白
            objSubFlow.SREF_VAILD = "1=1"; // 固定 第一站
            objSubFlow.SREF_VAILD6 = ""; //填空白
            objSubFlow.SREF_VAILD7 = "";//填空白
            lstSubFlow.Add(objSubFlow);
            objSubFlow = new MSI_lib.lib_Flow.SUB_FLOW();  //站點
            objSubFlow.FLOW_CODE = "RRR"; //流程代碼
            objSubFlow.FLOW_DESC = "第一站";
            objSubFlow.FLOW_DISABLE = "";//  填空白
            objSubFlow.FLOW_NO = "20";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objSubFlow.FLOW_ROUT_USE = "";//填空白
            objSubFlow.FLOW_USE = "1";//1是起用,0是不使用;
            objSubFlow.RouteNo = "CB_RRR_DW";
            objSubFlow.SREF_FIELD5 = ""; //填空白
            objSubFlow.SREF_FIELD6 = ""; //填空白
            objSubFlow.SREF_VAILD = "1=2"; //固定 第二站 之後
            objSubFlow.SREF_VAILD6 = ""; //填空白
            objSubFlow.SREF_VAILD7 = "";//填空白
            lstSubFlow.Add(objSubFlow);

            //只能修正SEQNO ****
            List<MSI_lib.lib_Flow.TypeFlow> lstTypeFlow = new List<MSI_lib.lib_Flow.TypeFlow>();
            MSI_lib.lib_Flow.TypeFlow objTypeFlow = new MSI_lib.lib_Flow.TypeFlow();
            objTypeFlow.FlowCode = "RRR";
            objTypeFlow.FLOW_NO = "10";// * 不能改
            objTypeFlow.RouteNo = "CB_RRR_DW";
            objTypeFlow.SEQNO = "0002";// 第一站是0001,第二站是0002 以此類推 
            lstTypeFlow.Add(objTypeFlow);
            objTypeFlow = new MSI_lib.lib_Flow.TypeFlow();
            objTypeFlow.FlowCode = "RRR";
            objTypeFlow.FLOW_NO = "20";// * 不能改
            objTypeFlow.RouteNo = "CB_RRR_DW";
            objTypeFlow.SEQNO = "0001";// 第一站是0001,第二站是0002 以此類推
            lstTypeFlow.Add(objTypeFlow);

            MSI_lib.lib_Flow.lstFlow clsFlow = new MSI_lib.lib_Flow.lstFlow();
            clsFlow.lstSubFlow = lstSubFlow;
            clsFlow.objFlowCode = objFlowCode;
            clsFlow.lstTypeFlow = lstTypeFlow;
            MSI_lib.lib_Flow flow = new MSI_lib.lib_Flow(clsFlow);
        }

        private static void CheckIns()
        {
            //1.測試新增流程
            MSI_lib.lib_Flow.FlowCode objFlowCode = new MSI_lib.lib_Flow.FlowCode();
            objFlowCode.FLOW_CODE = "RRR"; //流程代碼
            objFlowCode.FLOW_DESC = "rainny testing New Flow";  //流程說明
            objFlowCode.FLOW_SEQNO = ""; //這個會幫算不用填
            objFlowCode.FLOW_TABLE = "v_CB_RRR_DW";
            objFlowCode.RouteNo = "CB_RRR_DW";
            List<MSI_lib.lib_Flow.SUB_FLOW> lstSubFlow = new List<MSI_lib.lib_Flow.SUB_FLOW>();
            MSI_lib.lib_Flow.SUB_FLOW objSubFlow = new MSI_lib.lib_Flow.SUB_FLOW();  //站點
            objSubFlow.FLOW_CODE = "RRR"; //流程代碼
            objSubFlow.FLOW_DESC = "第一站";
            objSubFlow.FLOW_DISABLE = "";//  填空白
            objSubFlow.FLOW_NO = "10";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objSubFlow.FLOW_ROUT_USE = "";//填空白
            objSubFlow.FLOW_USE = "1";//1是起用,0是不使用;
            objSubFlow.RouteNo = "CB_RRR_DW";
            objSubFlow.SREF_FIELD5 = ""; //填空白
            objSubFlow.SREF_FIELD6 = ""; //填空白
            objSubFlow.SREF_VAILD = "1=2"; // 固定 第一站
            objSubFlow.SREF_VAILD6 = ""; //填空白
            objSubFlow.SREF_VAILD7 = "";//填空白
            lstSubFlow.Add(objSubFlow);
            objSubFlow = new MSI_lib.lib_Flow.SUB_FLOW();  //站點
            objSubFlow.FLOW_CODE = "RRR"; //流程代碼
            objSubFlow.FLOW_DESC = "第二站";
            objSubFlow.FLOW_DISABLE = "";//  填空白
            objSubFlow.FLOW_NO = "20";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objSubFlow.FLOW_ROUT_USE = "";//填空白
            objSubFlow.FLOW_USE = "1";//1是起用,0是不使用;
            objSubFlow.RouteNo = "CB_RRR_DW";
            objSubFlow.SREF_FIELD5 = ""; //填空白
            objSubFlow.SREF_FIELD6 = ""; //填空白
            objSubFlow.SREF_VAILD = "1=1"; //固定 第二站 之後
            objSubFlow.SREF_VAILD6 = ""; //填空白
            objSubFlow.SREF_VAILD7 = "";//填空白
            lstSubFlow.Add(objSubFlow);
            List<MSI_lib.lib_Flow.TypeFlow> lstTypeFlow = new List<MSI_lib.lib_Flow.TypeFlow>();
            MSI_lib.lib_Flow.TypeFlow objTypeFlow = new MSI_lib.lib_Flow.TypeFlow();
            objTypeFlow.FlowCode = "RRR";
            objTypeFlow.FLOW_NO = "10";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objTypeFlow.RouteNo = "CB_RRR_DW";
            objTypeFlow.SEQNO = "0001";// 第一站是0001,第二站是0002 以此類推
            lstTypeFlow.Add(objTypeFlow);
            objTypeFlow = new MSI_lib.lib_Flow.TypeFlow();
            objTypeFlow.FlowCode = "RRR";
            objTypeFlow.FLOW_NO = "20";// *10 起跳, 如果是新增第一站就是 1 * 10 , 如果第2站 2*10 
            objTypeFlow.RouteNo = "CB_RRR_DW";
            objTypeFlow.SEQNO = "0002";// 第一站是0001,第二站是0002 以此類推
            lstTypeFlow.Add(objTypeFlow);

            MSI_lib.lib_Flow.lstFlow clsFlow = new MSI_lib.lib_Flow.lstFlow();
            clsFlow.lstSubFlow = lstSubFlow;
            clsFlow.objFlowCode = objFlowCode;
            clsFlow.lstTypeFlow = lstTypeFlow;
            MSI_lib.lib_Flow flow = new MSI_lib.lib_Flow(clsFlow);

        }

        static void Main(string[] args)
        {
            //CheckIns();
            //  CheckUpd();


            MSI_lib.lib_Jibea lj = new MSI_lib.lib_Jibea();
            lj.JiebaKey("台北2018年第一週NB管理報表");
          //  lj.JiebaKey("我要讀取2017年與2018年維修單號開立未收貨管理報表");


            //object[] constructorParameters = new object[1];
            //constructorParameters[0] = "28"; // First parameter.

            //   MSI_lib.lib_ChatManager libchat = new MSI_lib.lib_ChatManager();
            //    libchat.lib_JustDoIt("GetIssueSolutionRouteNo", constructorParameters);


            //MSI_lib.lib_ChatManager libchat = new MSI_lib.lib_ChatManager();
            //libchat.lib_JustDoIt("GetIssueSolutionID", constructorParameters);




            //  lj.Delete("維修單號開立未收貨管理");
            //  lj.AddKey("維修單號開立未收貨管理");


        }
    }
}
