using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Hosting;
using Wil50n.Tool;
using System.Configuration;
using MSI_lib.SRLTService;
using System.Data;

namespace MSI_lib
{
    public class SRLTServicePortal
    {

        [ServiceContract]
        public interface IEcho
        {
            [OperationContract]
            string Echo(string s);
        }
        public class MyService : IEcho
        {
            public string Echo(string s)
            {
                return s;
            }
        }
        public class SRLTFormat
        {
            [Required]
            public String CustomerNo { get; set; }
            [Required]
            public String CustomerName { get; set; }
            [Required]
            public String CustomerPhone { get; set; }
            [Required]
            public String LoginUser { get; set; }
            [Required]
            public RepairCenters RepairCenter { get; set; }
            [Required]
            public int BarcodeQTY { get; set; }
            [Required]
            public RMATypes RMAType { get; set; }
            [MaxLength(3), MinLength(3)]
            public String Currency { get; set; }
            public String RMAReason { get; set; }
            public String BarcodeReason { get; set; }
            [Required]
            public Boolean InWarranty { get; set; }
            [Required, MaxLength(2), MinLength(2)]
            public String Country { get; set; }
            [Required]
            public ReasonCodes ReasonCode { get; set; }
            [Required]
            public String Model { get; set; }
            [Required]
            public String Barcode { get; set; }
            public Int64 sys_id { get; set; }
            public String RMANO { get; set; }
            [Required]
            public String LOB { get; set; }
            [Required]
            public float UnitPrice { get; set; }
            [Required]
            public String SalesID { get; set; }
            [Required]
            public String SalesName { get; set; }
            [Required]
            public String AccountID { get; set; }
            [Required]
            public String AccountName { get; set; }
            [Required]
            public TransferToRCs TransferToRC { get; set; }
            [Required]
            public String CustomerPN { get; set; }

        }
        public enum ReasonCodes
        {
            CID, DOA, OOW, RMA, FA, EX
        }
        public enum RMATypes
        {
            NormalRMA, NormalCredit
        }
        public enum RepairCenters
        {
            MSITP, MSICN3, MSITM3, MSITM5, AINDLINK, MSI52S
        }
        public enum TransferToRCs
        {
            MSITP, MSICN3, MSITM3, MSITM5, AINDLINK, MSI52S
        }
        public enum CRMInterface
        {
            TP, CN, UK, JP, KR
        }


        bool IsTesting
        {
            get
            {
                bool YN = true;
                if (ConfigurationManager.AppSettings["CRMIsTest"] != null)
                    bool.TryParse(ConfigurationManager.AppSettings["CRMIsTest"].ToString(), out YN);

                return YN;
            }
        }

        SRLTService.ASPUploadProcessSoapClient CreateSoap()
        {
            SecurityBindingElement securityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
            httpsTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Ntlm;
            CustomBinding binding = new CustomBinding(securityElement, httpsTransport);
            binding.Name = "ASPUploadProcessSoap";
            // Replace <machinename> with the name of the development server.
            EndpointAddress endPoint = new EndpointAddress("http://rma3.msi.com/partnerws/cs_eu_aspuploadprocess.asmx");
            SRLTService.ASPUploadProcessSoapClient SRLTClient = new SRLTService.ASPUploadProcessSoapClient(binding, endPoint);
            SRLTClient.ClientCredentials.UserName.UserName = "wilsonhuang";
            using (HostingEnvironment.Impersonate())
            {
                SRLTClient.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            }
            return SRLTClient;
        }

        public List<SRLT_StatusEntity> GetSRLTStatusBySingle(String RMA_NO)
        {
            SRLTService.ASPUploadProcessSoapClient SRLTSoap = new SRLTService.ASPUploadProcessSoapClient();
            //SRLTService.ASPUploadProcessSoapClient SRLTSoap = CreateSoap();
            List<SRLT_StatusEntity> _Obj = new List<SRLT_StatusEntity>();
            try
            {
                SRLTService.QueryFilter QF = new SRLTService.QueryFilter() { p_s_rmano = RMA_NO };
                _Obj = SRLTSoap.getSRLTStatusForWeb(QF).ToList<SRLT_StatusEntity>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }



            return _Obj;
        }

        /// <summary>
        /// Evan給我的Package已經是GroupBy好的
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public String CreateRMANO(List<SRLTFormat> Obj)
        {
            String TransferToRCStr = "";
            String RepairCenterStr = "";
            String RMATypeStr = "";
            String ReasonCodeStr = "";
            String RMANO = null;
            #region GroupDate ByModel去算Qty
            SRLTService.ASPUploadProcessSoapClient SRLTSoap = new SRLTService.ASPUploadProcessSoapClient();
            //SRLTService.ASPUploadProcessSoapClient SRLTSoap = CreateSoap();
            SRLTService.rmaRequest rq = new SRLTService.rmaRequest();

            #region
            switch (Obj[0].TransferToRC)
            {
                case TransferToRCs.MSITP:
                    TransferToRCStr = "MSI-TP";
                    break;
                case TransferToRCs.MSICN3:
                    TransferToRCStr = "MSI-CN3";
                    break;
                case TransferToRCs.MSITM3:
                    TransferToRCStr = "MSI-TM3";
                    break;
                case TransferToRCs.MSITM5:
                    TransferToRCStr = "MSI-TM5";
                    break;
                case TransferToRCs.AINDLINK:
                    TransferToRCStr = "AIN-DLINK";
                    break;
                case TransferToRCs.MSI52S:
                    TransferToRCStr = "MSI-52S";
                    break;
            }
            switch (Obj[0].RepairCenter)
            {
                case RepairCenters.MSITP:
                    RepairCenterStr = "MSI-TP";
                    break;
                case RepairCenters.MSICN3:
                    RepairCenterStr = "MSI-CN3";
                    break;
                case RepairCenters.MSITM3:
                    RepairCenterStr = "MSI-TM3";
                    break;
                case RepairCenters.MSITM5:
                    RepairCenterStr = "MSI-TM5";
                    break;
                case RepairCenters.AINDLINK:
                    RepairCenterStr = "AIN-DLINK";
                    break;
                case RepairCenters.MSI52S:
                    RepairCenterStr = "MSI-52S";
                    break;
            }
            switch (Obj[0].RMAType)
            {
                case RMATypes.NormalRMA:
                    RMATypeStr = "Normal RMA";
                    break;
                case RMATypes.NormalCredit:
                    RMATypeStr = "Normal Credit";
                    break;
            }
            switch (Obj[0].ReasonCode)
            {
                case ReasonCodes.CID:
                    ReasonCodeStr = "CID";
                    break;
                case ReasonCodes.DOA:
                    ReasonCodeStr = "DOA";
                    break;
                case ReasonCodes.OOW:
                    ReasonCodeStr = "OOW";
                    break;
                case ReasonCodes.RMA:
                    ReasonCodeStr = "RMA";
                    break;
                case ReasonCodes.FA:
                    ReasonCodeStr = "FA";
                    break;
                case ReasonCodes.EX:
                    ReasonCodeStr = "EX";
                    break;
            }
            #endregion

            var query = from rma in Obj
                        group rma by new { rma.Model }; //分Line
            rq.s_newRMA_H = new SRLTService.S_rmaHeader()
            {
                customerno = Obj[0].CustomerNo,
                customername = Obj[0].CustomerName,
                cm_phone = Obj[0].CustomerPhone,
                requesteruid = Obj[0].LoginUser ?? "gracehung", //目前預設綁定Grace
                repaircenter = RepairCenterStr,
                totalqty = Obj.Count().ToString(),
                rmatype = RMATypeStr,
                currencycode = Obj[0].Currency ?? "USD",
                notes = Obj[0].RMAReason ?? "system auto notes",
                country = Obj[0].Country ?? "US",
                reasoncode = ReasonCodeStr,
                salesuid = Obj[0].SalesID,
                salesname = Obj[0].SalesName,
                account_id = Obj[0].AccountID,
                account_name = Obj[0].AccountName,
                transfertorc = RepairCenterStr,
                cm_add = ""
            };
            SRLTService.S_rmaLines[] rqLines = new SRLTService.S_rmaLines[query.Count()];
            SRLTService.S_rmaLine_details[] rqLineD = new SRLTService.S_rmaLine_details[Obj.Count()];
            int Line = 1;
            int ldCounts = 1;

            foreach (var l in query)
            {
                rqLines[Line - 1] = new SRLTService.S_rmaLines();
                rqLines[Line - 1].lineno = Line.ToString();
                rqLines[Line - 1].model = l.Key.Model;
                rqLines[Line - 1].requestedqty = l.Count().ToString();
                rqLines[Line - 1].currencycode = Obj[0].Currency ?? "USD";
                rqLines[Line - 1].itemid = "";
                rqLines[Line - 1].repaircenter = RepairCenterStr;
                rqLines[Line - 1].unitprice = l.Where(x => x.Model.Trim().Equals(l.Key.Model.Trim())).Select(y => y.UnitPrice).Distinct().FirstOrDefault().ToString();

                if (l.Where(x => x.Model.Trim().Equals(l.Key.Model.Trim())).Count() > 0 && l.Where(x => x.Model.Trim().Equals(l.Key.Model.Trim())).Select(z => z.CustomerPN) != null)
                {
                    rqLines[Line - 1].customerpartno = l.Where(x => x.Model.Trim().Equals(l.Key.Model.Trim())).Select(z => z.CustomerPN).ToList()[0];
                }

                foreach (var ld in l)
                {
                    rqLineD[ldCounts - 1] = new SRLTService.S_rmaLine_details();
                    rqLineD[ldCounts - 1].lineno = Line.ToString();
                    rqLineD[ldCounts - 1].barcode = ld.Barcode;
                    rqLineD[ldCounts - 1].warranty = ld.InWarranty ? "Y" : "N";
                    rqLineD[ldCounts - 1].repaircenter = RepairCenterStr;
                    rqLineD[ldCounts - 1].fault_description = ld.BarcodeReason ?? "system auto descript";

                    ldCounts++;
                }
                Line++;
            }
            rq.s_newRMA_L = rqLines;
            rq.s_newRMA_L_D = rqLineD;
            #endregion

            try
            {
                if (IsTesting)
                    RMANO = GetRMANo(rq, SRLTSoap);
                else
                    RMANO = GenBatchNo.GetBatchNo();
            }
            catch (Exception ex)
            {
                RMANO = ex.Message;
            }
            return RMANO;
        }

        String GetRMANo(SRLTService.rmaRequest rq, SRLTService.ASPUploadProcessSoapClient SRLTSoap)
        {
            String RMANO = null;
            SRLTSoap.createRMArequest(rq, out RMANO);
            return RMANO;
        }

        String GetReasonCode(ReasonCodes r_Code)
        {
            String returnVal = null;
            switch (r_Code)
            {
                case ReasonCodes.CID:
                    returnVal = "CID";
                    break;
                case ReasonCodes.DOA:
                    returnVal = "DOA";
                    break;
                case ReasonCodes.OOW:
                    returnVal = "OOW";
                    break;
                case ReasonCodes.RMA:
                    returnVal = "RMA";
                    break;
                case ReasonCodes.FA:
                    returnVal = "FA";
                    break;
                case ReasonCodes.EX:
                    returnVal = "EX";
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Flag, X=查無保固,Y=保固內,N=保固外
        /// StartDate, 保固起始日
        /// EndDate, 保固截止日
        /// Standard,標準保固長度
        /// Add_Month,寬限保固月數
        /// Extend,延長保固月數
        /// onsite_startdate,保固啟用起始日
        /// onsite_month,保固月數
        /// 保固到期 = onsite_startdate + onsite_month
        /// lob_no, LOB分類
        /// </summary>
        /// <param name="CRMType"></param>
        /// <param name="Barcode"></param>
        /// <param name="Msg"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public DataTable GetRLTWarrantyInfo(CRMInterface CRMType, String Barcode, out string Msg, out string Description)
        {
            String ItemNo = null;
            Msg = null; Description = null;
            DataTable dt = null;
            try
            {
                SRLTService.ASPUploadProcessSoapClient SRLTSoap = new SRLTService.ASPUploadProcessSoapClient();
                QueryFilterH qf_conf = new QueryFilterH();
                qf_conf.p_s_barcode = Barcode;
                DataSet ds1 = SRLTSoap.getSRLTWarrantyInfo(qf_conf);
                dt = ds1.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // 如果输入的条码不存在
                    if (dt.Rows[i]["Flag"].ToString().Equals("X"))
                    {
                        Msg = "Undifind this Barcode Infomation.";
                    }
                    else
                    {
                        Msg = "Successful";
                        ItemNo = dt.Rows[i]["finished_item_no"].ToString();
                        // 开始信息查询
                        if (ItemNo.Trim().Length > 0)
                        {
                            string result = "";
                            if (CRMType == CRMInterface.CN)
                            {
                                DataTable dtx = SRLTSoap.GetMSICNModelPrice("", ItemNo, "");
                                for (int k = 0; k < dtx.Rows.Count; k++)
                                {
                                    //把结果输入到界面上
                                    result = "";
                                    result = result + dtx.Rows[k]["cpu"].ToString() + ",";
                                    result = result + dtx.Rows[k]["ram"].ToString() + ",";
                                    result = result + dtx.Rows[k]["hdd"].ToString() + ",";
                                    result = result + dtx.Rows[k]["size"].ToString() + ",";
                                    result = result + dtx.Rows[k]["lcdtype"].ToString() + ",";
                                    result = result + dtx.Rows[k]["lcd"].ToString() + ",";
                                    result = result + dtx.Rows[k]["odd"].ToString() + ",";
                                    result = result + dtx.Rows[k]["vga"].ToString() + ",";
                                    result = result + dtx.Rows[k]["vram"].ToString() + ",";
                                    result = result + dtx.Rows[k]["wireless"].ToString() + ",";
                                    result = result + dtx.Rows[k]["modem"].ToString() + ",";
                                    result = result + dtx.Rows[k]["bt"].ToString() + ",";
                                    result = result + dtx.Rows[k]["ccd"].ToString() + ",";
                                    result = result + dtx.Rows[k]["battery"].ToString() + ",";
                                    result = result + dtx.Rows[k]["osdesc"].ToString() + ",";
                                    result = result + dtx.Rows[k]["os"].ToString() + ",";
                                    result = result + dtx.Rows[k]["misc"].ToString() + ",";
                                    result = result + dtx.Rows[k]["price"].ToString();
                                    Description = result;
                                }
                            }
                            else
                            {
                                DataSet ds = SRLTSoap.GetMSIModel(ItemNo, "");
                                DataTable dtx = ds.Tables[0];

                                for (int j = 0; j < dtx.Rows.Count; j++)
                                {
                                    //把结果输入到界面上
                                    Description = dtx.Rows[i]["description"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
            }
            return dt;
        }

        [Serializable]
        public class RMAHeader
        {
            public String CustomerName { get; set; }
            public String ContactEmail { get; set; }
            public String Requester_Uid { get; set; }
            public int ServiceType { get; set; }
            public String PickupCountry { get; set; }
            public String PickupCity { get; set; }
            public String PickupPostalCode { get; set; }
            public String PickupCmAdd { get; set; }
            public String CmAdd { get; set; }
            public String CmPhone { get; set; }
            public String CmPostalCode { get; set; }
            public String HeaderNotes { get; set; }
            public String RepairCenter { get; set; }
            public String TotalQty { get; set; }
            public String RMAType { get; set; }
            public String CurrencyCode { get; set; }
            public String ReasonCode { get; set; }
        }

        [Serializable]
        public class RMALines
        {
            public String Model { get; set; }
            public String RequestedQty { get; set; }
            public String CurrencyCode { get; set; }
            public String RepairCenter { get; set; }
        }
                
        [Serializable]
        public class RMALineDetails
        {
            public String Model { get; set; }
            public String RequestedQty { get; set; }
            public int WarrantyJudge { get; set; } //保固內外 0=非保固 1=保固
            public String Barcode { get; set; }
            public String FaultDescription { get; set; } 

        }

        public void CreateRMANo(RMAHeader RmaHeader, RMALines RmaLine, RMALineDetails RmaLineDetails, out String Msg, out String RMANo)
        {
            String RMA_No = null;
            RMANo = null;
            Msg = null;
            rmaRequest rmaR = new rmaRequest();
            S_rmaHeader rmaH = new S_rmaHeader();
            S_rmaLines[] rmaL = new S_rmaLines[1];
            S_rmaLine_details[] rmaLD = new S_rmaLine_details[1];
            S_rmaAttachment rmaAtta = new S_rmaAttachment();

            #region rmaHeader
            /*測試*/
            rmaH.isbooking = RmaHeader.ServiceType.Equals(2) ? "Y" : "N"; //是否預約取件
            rmaH.isonsite = RmaHeader.ServiceType.Equals(1) ? "Y" : "N"; //是否OnSiteService
            rmaH.ispickup = RmaHeader.ServiceType.Equals(2) ? "Y" : "N"; //是否到府取件
            rmaH.pickup_country = RmaHeader.PickupCountry; //取件國家
            rmaH.pickup_city = RmaHeader.PickupCity; //取件城市
            rmaH.pickup_postal_code = RmaHeader.PickupPostalCode; //取件地區郵遞區號
            rmaH.pickup_cm_add = RmaHeader.PickupCmAdd; //取件客戶地址
            rmaH.customername = RmaHeader.CustomerName; //客戶名字
            rmaH.contactemail = RmaHeader.ContactEmail; //客戶聯繫電子郵件
            rmaH.requesteruid = RmaHeader.Requester_Uid; //Call Center人員帳號(SRLT帳號) 測試用MSIRIGHTNOW
            rmaH.cm_add = RmaHeader.CmAdd; //客戶地址
            rmaH.cm_phone = RmaHeader.CmPhone; //客戶電話
            rmaH.postal_code = RmaHeader.CmPostalCode; //客戶地址郵遞區號
            rmaH.repaircenter = RmaHeader.RepairCenter; //維修中心 MSI-TP
            rmaH.totalqty = RmaHeader.TotalQty; //總數量
            rmaH.rmatype = RmaHeader.RMAType; //Normal RMA, On Site Service
            rmaH.currencycode = RmaHeader.CurrencyCode; //幣別代碼(三碼) TWD
            rmaH.notes = RmaHeader.HeaderNotes; //問題描述
            rmaH.country = RmaHeader.RepairCenter; //服務人員國家碼(兩碼) TW
            rmaH.reasoncode = RmaHeader.ReasonCode; //維修類型 RMA DOA OOW etc....
            #endregion

            #region rmaLines
            rmaL[0] = new S_rmaLines();
            rmaL[0].lineno = "1";
            rmaL[0].model = RmaLine.Model; //型號 911-7641-074
            rmaL[0].requestedqty = "1";
            rmaL[0].currencycode = RmaLine.CurrencyCode; //幣別(三碼) TWD
            rmaL[0].itemid = "";
            rmaL[0].repaircenter = RmaLine.RepairCenter; //維修中心 MSI-TP
            #endregion

            #region rmaLine_details
            rmaLD[0] = new S_rmaLine_details();
            rmaLD[0].lineno = "1";
            rmaLD[0].barcode = RmaLineDetails.Barcode; //Barcode 601-7641-220B1403033787
            if (RmaLineDetails.WarrantyJudge == 0) //保固內外
                rmaLD[0].warranty = "N";
            else
                rmaLD[0].warranty = "Y";
            rmaLD[0].repaircenter = RmaHeader.RepairCenter; //維修中心 MSI-TP
            rmaLD[0].fault_description = RmaLineDetails.FaultDescription; //問題描述
            #endregion

            rmaR.s_newRMA_H = rmaH;
            rmaR.s_newRMA_L = rmaL;
            rmaR.s_newRMA_L_D = rmaLD;
            rmaR.s_newRMA_A = rmaAtta;

            SRLTService.ASPUploadProcessSoapClient SRLTSoap = new SRLTService.ASPUploadProcessSoapClient();

            // 如果是申请RMA的情况
            if (RMA_No == null || RMA_No.Length == 0)
            {
                bool vPass = false;

                if (IsTesting)
                {
                    vPass = SRLTSoap.createRMArequest(rmaR, out RMA_No);
                }
                else
                {
                    RMA_No = GenBatchNo.GetBatchNo();
                    //RMA_No = "測試取回單號";
                    vPass = true;
                }

                if (!vPass)
                {
                    //if (RMA_No.IndexOf("此條碼已在申請於此維修單:維修單號:") > -1)
                    Msg = RMA_No;
                }
                else
                    RMANo = RMA_No;
            }
            // 如果是修改RMA的情况下
            else
            {
                // 设置需要修改RMA的no
                rmaR.s_newRMA_H.rmano = RMA_No;
                string vPass = SRLTSoap.updateRMArequest(rmaR);
                if (vPass != null && vPass.Equals("PASS"))
                    Msg = "Update Successful";
                else
                    Msg = "Update Failed";
            }
        }
    }
}