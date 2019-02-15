using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MSI_lib
{
    public class lib_DataConvert
    {
        DataTable _dt = new DataTable();
        String Url { get; set; }
        String FilePath
        {
            get
            {
                String Val = System.Configuration.ConfigurationManager.AppSettings["libDataSnapshotPath"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["libDataSnapshotPath"].ToString();
                if (!Directory.Exists(Val))
                    Directory.CreateDirectory(Val);
                return Val;
            }
        }
        ReturnType _RT { get; set; }
        String[] PassColumn = new string[] { "sys_id", "sys_ID", "sys_CreateDate", "sys_UpdateDate", "sys_CreateUser", "sys_UpdateUser", "S1_LEVEL_ID", "b_id", "r_sys_id", "ST1_sys_Qty", "ST1_sys_UnitPrice", "ST1_sys_Amount" };
        static String SnapshotFileName { get; set; }

        public enum ReturnType
        {
            Url, Bas64, Bytes, Text, FileName, Html
        }

        public lib_DataConvert(DataTable dt, ReturnType RT)
        {
            _dt = dt.Copy();
            _RT = RT;
            //foreach (String c in PassColumn)
            //{
            //    _dt.Columns.Remove(c);
            //}


            var removeColumns = dt.Columns.Cast<DataColumn>()
               .Where(c => PassColumn.Contains(c.ColumnName));

            foreach (DataColumn colToRemove in removeColumns)
                _dt.Columns.Remove(colToRemove.ColumnName);


            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                if (_dt.Columns[i].ColumnName.IndexOf("sys_id") > 0)
                {
                    _dt.Columns.RemoveAt(i);
                }
            }

        }

        public String GetConvertResult()
        {
            String ReutrnVal = null;
            switch (_RT)
            {
                case ReturnType.Url:
                    break;
                case ReturnType.Bas64:
                    break;
                case ReturnType.Bytes:
                    break;
                case ReturnType.Text:
                    ReutrnVal = BindDataText(_dt);
                    break;
                case ReturnType.Html:
                    ReutrnVal = BindHtmlTable(_dt);
                    break;
                case ReturnType.FileName:
                    ReutrnVal = SnapshotFileName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                    ConvertToPicture();
                    break;
                default:
                    break;
            }
            return ReutrnVal;
        }

        void ConvertToPicture()
        {

            Thread thread = new Thread(delegate ()
            {
                using (WebBrowser browser = new WebBrowser())
                {
                    String html = BindHtmlTable(_dt);
                    browser.ScrollBarsEnabled = false;
                    browser.AllowNavigation = true;
                    browser.DocumentText = html;
                    browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            System.Drawing.Rectangle rectangle = browser.Document.Body.ScrollRectangle;
            int width = rectangle.Width;
            int height = rectangle.Height;

            ////設置解析後HTML的可視區域
            browser.Width = width;
            browser.Height = height;

            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            {
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                //設置圖片文件保存路徑和圖片格式，格式可以自定義
                string filePath = $@"{FilePath}\{SnapshotFileName}";
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        String BindHtmlTable(DataTable _dt)
        {
            String HtmlTable = "", Tag_th = "", Tag_row = "";
            foreach (DataColumn dc in _dt.Columns)
            {
                //if (PassColumn.Contains(dc.ColumnName.ToLower()))
                //    continue;
                Tag_th += $"<th style='border: 1px solid #666; white-space:nowrap; padding: 5px 10px; min-width: 50px;'>{dc.ColumnName}</th>";
            }

            Tag_row += $"<tr style='background-color: #4699ca; color: #fff;'>{Tag_th}</tr>";

            bool IsAlt = false;
            double isNumber = 0;
            foreach (DataRow dr in _dt.Rows)
            {
                String Tag_td = "", textAlign = "left";
                for (int i = 0; i < _dt.Columns.Count; i++)
                {
                    //if (PassColumn.Contains(_dt.Columns[i].ColumnName.ToLower()))
                    //    continue;

                    //switch (_dt.Columns[i].DataType.ToString())
                    //{
                    //    case "Decimal":
                    //    case "Double":
                    //    case "Int16":
                    //    case "Int32":
                    //    case "Int64":
                    //        textAlign = "right";
                    //        break;
                    //}
                    textAlign = "left";
                    if (dr[i] != null && !Convert.IsDBNull(dr[i]) && !string.IsNullOrEmpty(dr[i].ToString()) && double.TryParse(dr[i].ToString(), out isNumber))
                        textAlign = "right";

                    Tag_td += $"<td style='border: 1px solid #666; white-space:nowrap; padding: 5px 10px; min-width: 50px; text-align: { textAlign };'>{dr[i].ToString()}</td>";
                }
                //#f2f2f2
                Tag_row += $"<tr style='background-color: #{ (IsAlt ? "e0f0f9" : "fff") };'>{Tag_td}</tr>";
                IsAlt = !IsAlt;
            }

            HtmlTable = $"<table style='border-collapse: collapse; border: 1px solid black;'>{Tag_row}</table>";

            return HtmlTable;
        }

        String BindDataText(DataTable _dt)
        {
            //先計算每欄最長字數
            Dictionary<int, int> dicColumnLength = new Dictionary<int, int>();
            int columnLenght = 0;
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                columnLenght = Encoding.Default.GetBytes(_dt.Columns[i].ColumnName).Length + 8;

                if (!dicColumnLength.ContainsKey(i))
                    dicColumnLength.Add(i, columnLenght);
                else if (dicColumnLength[i] < columnLenght)
                    dicColumnLength[i] = columnLenght;
            }
            foreach (DataRow dr in _dt.Rows)
            {
                for (int j = 0; j < _dt.Columns.Count; j++)
                {
                    columnLenght = Encoding.Default.GetBytes(dr[j].ToString()).Length + 8;

                    if (!dicColumnLength.ContainsKey(j))
                        dicColumnLength.Add(j, columnLenght);
                    else if (dicColumnLength[j] < columnLenght)
                        dicColumnLength[j] = columnLenght;
                }
            }

            //開始產生表格字串
            string tabStr = "";
            String DataText = "", Tag_th = "", Tag_row = "";
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                //取得要補的空白字串
                tabStr = GetTabString(dicColumnLength[i], Encoding.Default.GetBytes(_dt.Columns[i].ColumnName).Length);

                //if (PassColumn.Contains(_dt.Columns[i].ColumnName.ToLower()))
                //    continue;
                if (i.Equals(_dt.Columns.Count - 1))
                    Tag_th += $"{_dt.Columns[i].ColumnName}";
                else
                    Tag_th += $"{_dt.Columns[i].ColumnName}" + "\t";
            }
            Tag_row += $"{Tag_th}\r\n";

            foreach (DataRow dr in _dt.Rows)
            {
                String Tag_td = "", val = "";
                for (int j = 0; j < _dt.Columns.Count; j++)
                {
                    val = dr[j].ToString();
                    //取得要補的空白字串
                    tabStr = GetTabString(dicColumnLength[j], Encoding.Default.GetBytes(val).Length);

                    //if (PassColumn.Contains(_dt.Columns[j].ColumnName.ToLower()))
                    //    continue;
                    if (j.Equals(_dt.Columns.Count - 1))
                        Tag_td += $"{val}";
                    else
                        Tag_td += $"{val}" + "\t";
                }
                Tag_row += $"{Tag_td}\r\n";
            }

            DataText = $"{Tag_row}";
            return DataText;
        }

        string GetTabString(int ColumnLength, int StringLength)
        {
            //int tabNum = ColumnLength - StringLength;
            double tabNum = Math.Round((ColumnLength - StringLength) / 4.0, 2);
            int _tabNum = Convert.ToInt32(tabNum);
            //if (_tabNum % 2 == 0 && tabNum - _tabNum >= 0.5)
            //    _tabNum += 1;

            //int _tabNum = Convert.ToInt16(Math.Ceiling((ColumnLength - StringLength) / 4.0));
            //int _tabNum = ColumnLength - StringLength;

            string tabStr = "";
            if (_tabNum > 0)
            {
                for (int i = 0; i < _tabNum; i++)
                    tabStr += " ";
            }

            return tabStr;
        }
    }
}