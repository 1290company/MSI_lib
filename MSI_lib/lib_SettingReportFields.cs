using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace MSI_lib
{
    public class lib_SettingReportFields : System.Web.UI.WebControls.WebControl, INamingContainer
    {
        public enum ControlType : int { TextBox, DropDownList }

        public class ControlObject
        {
            public string ID { get; set; }
            public ControlType Type { get; set; }
        }

        public ControlObject RouteControl { get; set; }
        public string SaveCompleteJSCallbackFunc { get; set; }


        public lib_SettingReportFields(ControlObject _RouteControl, string _SaveCompleteJSCallbackFunc)
        {
            this.RouteControl = _RouteControl;
            this.SaveCompleteJSCallbackFunc = _SaveCompleteJSCallbackFunc;
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            Controls.Add(new LiteralControl("<a herf='javascript:void(0);' id='lnkSettingReoprtFields' style='cursor: pointer;'><i class='fa fa-cog fa-fw'></i></a>"));
            Controls.Add(new LiteralControl(ModalHtml()));
            Controls.Add(new LiteralControl(JavascriptString()));
        }

        private string JavascriptString()
        {
            string js = @"<script>
                            $(function () {
                                //setting report fields
                                $('#lnkSettingReoprtFields').click(function () {
                                    settingReoprtFields('設定報表欄位', '', true);
                                });
        
                                $('#ddlSR_Station').change(function () {
                                    var stationNo = $('#ddlSR_Station option:selected').val();
                                    getReportFields(stationNo);
                                });

                                $('#btnSaveReportFields').click(function () {";
            js += "                var routeNo = $('#" + RouteControl.ID + "" + (RouteControl.Type == ControlType.DropDownList ? " option:selected" : "") + "').val();";
            js += @"                var fields = {
                                        RouteNo: routeNo,
                                        StationNo: $('#ddlSR_Station option:selected').val(),
                                        InDate: {
                                            Name: $('#ddlSR_InDate option:selected').val(),
                                            FieldType: typeof $('#ddlSR_InDate option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_InDate option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_InDate option:selected').text().trim() == '--' ? '' : $('#ddlSR_InDate option:selected').text().trim(),
                                        },
                                        OutDate: {
                                            Name: $('#ddlSR_OutDate option:selected').val(),
                                            FieldType: typeof $('#ddlSR_OutDate option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_OutDate option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_OutDate option:selected').text().trim() == '--' ? '' : $('#ddlSR_OutDate option:selected').text().trim(),
                                        },
                                        Price: {
                                            Name: $('#ddlSR_Price option:selected').val(),
                                            FieldType: typeof $('#ddlSR_Price option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_Price option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_Price option:selected').text().trim() == '--' ? '' : $('#ddlSR_Price option:selected').text().trim(),
                                        },
                                        Quantity: {
                                            Name: $('#ddlSR_Qty option:selected').val(),
                                            FieldType: typeof $('#ddlSR_Qty option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_Qty option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_Qty option:selected').text().trim() == '--' ? '' : $('#ddlSR_Qty option:selected').text().trim(),
                                        },
                                        Amount: {
                                            Name: $('#ddlSR_Amount option:selected').val(),
                                            FieldType: typeof $('#ddlSR_Amount option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_Amount option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_Amount option:selected').text().trim() == '--' ? '' : $('#ddlSR_Amount option:selected').text().trim(),
                                        },
                                        RYG: {
                                            Name: $('#ddlSR_RYG option:selected').val(),
                                            FieldType: typeof $('#ddlSR_RYG option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_RYG option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_RYG option:selected').text().trim() == '--' ? '' : $('#ddlSR_RYG option:selected').text().trim(),
                                        },
                                        StandardTAT: {
                                            Name: $('#ddlSR_STAT option:selected').val(),
                                            FieldType: typeof $('#ddlSR_STAT option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_STAT option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_STAT option:selected').text().trim() == '--' ? '' : $('#ddlSR_STAT option:selected').text().trim(),
                                        },
                                        ActualTAT: {
                                            Name: $('#ddlSR_NTAT option:selected').val(),
                                            FieldType: typeof $('#ddlSR_NTAT option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_NTAT option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_NTAT option:selected').text().trim() == '--' ? '' : $('#ddlSR_NTAT option:selected').text().trim(),
                                        },
                                        TotalRYG: {
                                            Name: $('#ddlSR_TalRYG option:selected').val(),
                                            FieldType: typeof $('#ddlSR_TalRYG option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_TalRYG option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_TalRYG option:selected').text().trim() == '--' ? '' : $('#ddlSR_TalRYG option:selected').text().trim(),
                                        },
                                        TotalStandardTAT: {
                                            Name: $('#ddlSR_TalSTAT option:selected').val(),
                                            FieldType: typeof $('#ddlSR_TalSTAT option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_TalSTAT option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_TalSTAT option:selected').text().trim() == '--' ? '' : $('#ddlSR_TalSTAT option:selected').text().trim(),
                                        },
                                        TotalActualTAT: {
                                            Name: $('#ddlSR_TalNTAT option:selected').val(),
                                            FieldType: typeof $('#ddlSR_TalNTAT option:selected').data('fieldtype') == 'undefined' ? '' : $('#ddlSR_TalNTAT option:selected').data('fieldtype'),
                                            DisplayName: $('#ddlSR_TalNTAT option:selected').text().trim() == '--' ? '' : $('#ddlSR_TalNTAT option:selected').text().trim(),
                                        }
                                    };
                                    console.log(fields);

                                    if (fields.StationNo.length == 0 || fields.InDate.Field == '' || fields.OutDate.Field == '' || fields.Price.Field == '' || fields.Quantity.Field == '' || fields.Amount.Field == '') {
                                        alert('站點、時間起、時間迄、單價、數量、金額，欄位不可空白');
                                    }
                                    else {
                                        showLoading();
                                        var data = { Fields: JSON.stringify(fields) };
                                        $.ajax({
                                            type: 'POST',
                                            url: '/DeskTopModules/XYInOutStock_MultiFlow/Service/WebService1.asmx/SetReportFields',
                                            data: JSON.stringify(data),
                                            contentType: 'application/json; charset=utf-8',
                                            dataType: 'json',
                                            success: function (resp) {
                                                if (resp != null && resp.d.length > 0) {
                                                    var d = JSON.parse(resp.d);
                                                    if (d != null) {
                                                        if (d.Result) {
                                                            alert('已更新');
                                                            $('#modalSettingReport').modal('hide');";
            js += "                                        " + SaveCompleteJSCallbackFunc;
            js += @"
                                                        }
                                                        else if (d.ErrorMessage.length > 0) {
                                                            alert(d.ErrorMessage);
                                                        }
                                                        else {
                                                            alert('更新失敗');
                                                        }
                                                    }
                                                }
                                            },
                                            complete: function () {
                                                hideLoading();
                                            }
                                        });
                                    }
                                });
                            });

                            var rf_loading = false;
                            function settingReoprtFields(showModal) {";
            js += "            var routeNo = $('#" + RouteControl.ID + "" + (RouteControl.Type == ControlType.DropDownList ? " option:selected" : "") + "').val();";
            js += @"            if (routeNo.replace('--', '').length == 0) {
                                    alert('請選擇流程');
                                }
                                else if (!rf_loading) {
                                    rf_loading = true;
                                    $('#lnkSettingReoprtFields').html($('<i>').addClass('fa fa-spinner fa-spin fa-fw'));

                                    var data = {";
            js += "                    RouteNo: $('#" + RouteControl.ID + "" + (RouteControl.Type == ControlType.DropDownList ? " option:selected" : "") + "').val()";
            js += @"                }
                                    $.ajax({
                                        type: 'POST',
                                        url: '/DeskTopModules/XYInOutStock_MultiFlow/Service/WebService1.asmx/GetStation',
                                        data: JSON.stringify(data),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        success: function (resp) {
                                            if (resp != null && resp.d.length > 0) {
                                                var d = JSON.parse(resp.d);
                                                if (d != null) {
                                                    console.log(d);
                                                    var station = d.Station.split(',');
                                                    //append station
                                                    var slt = $('<select>');
                                                    slt.append($('<option>').val('').text('--'));
                                                    for (var i = 0; i < station.length; i++) {
                                                        slt.append($('<option>').val(station[i]).text(station[i]));
                                                    }
                                                    $('#ddlSR_Station').html(slt.html());

                                                    var data2 = {
                                                        MappingName: routeNo
                                                    }
                                                    $.ajax({
                                                        type: 'POST',
                                                        url: '/DeskTopModules/XYInOutStock_MultiFlow/Service/WebService1.asmx/GetFields',
                                                        data: JSON.stringify(data2),
                                                        contentType: 'application/json; charset=utf-8',
                                                        dataType: 'json',
                                                        success: function (resp) {
                                                            if (resp != null && resp.d.length > 0) {
                                                                var d = JSON.parse(resp.d);
                                                                if (d != null) {
                                                                    //append fields
                                                                    var numSlt = $('<select>');
                                                                    var strSlt = $('<select>');
                                                                    var dateSlt = $('<select>');
                                                                    for (var i = 0; i < d.Items.length; i++) {
                                                                        switch (d.Items[i].FieldType) {
                                                                            case 'int':
                                                                            case 'bigint':
                                                                            case 'float':
                                                                                numSlt.append($('<option>').attr({ 'data-fieldtype': d.Items[i].FieldType }).val(d.Items[i].Field).text(d.Items[i].DisplayName));
                                                                                break;
                                                                            case 'varchar':
                                                                            case 'nvarchar':
                                                                                strSlt.append($('<option>').attr({ 'data-fieldtype': d.Items[i].FieldType }).val(d.Items[i].Field).text(d.Items[i].DisplayName));
                                                                                break;
                                                                            case 'date':
                                                                            case 'datetime':
                                                                                dateSlt.append($('<option>').attr({ 'data-fieldtype': d.Items[i].FieldType }).val(d.Items[i].Field).text(d.Items[i].DisplayName));
                                                                                break;
                                                                        }
                                                                    }

                                                                    $('#ddlSR_InDate').html($('<option>').data({ 'field': '' }).val('').text('--')).append(dateSlt.html());
                                                                    $('#ddlSR_OutDate').html($('<option>').data({ 'field': '' }).val('').text('--')).append(dateSlt.html());
                                                                    $('#ddlSR_Price').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html());
                                                                    $('#ddlSR_Qty').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html());
                                                                    $('#ddlSR_Amount').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html());

                                                                    $('#ddlSR_RYG').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());
                                                                    $('#ddlSR_STAT').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());
                                                                    $('#ddlSR_NTAT').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());
                                                                    $('#ddlSR_TalRYG').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());
                                                                    $('#ddlSR_TalSTAT').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());
                                                                    $('#ddlSR_TalNTAT').html($('<option>').data({ 'field': '' }).val('').text('--')).append(numSlt.html()).append(strSlt.html()).append(dateSlt.html());

                                                                    if (showModal) {
                                                                        $('#modalSettingReport h1').text('設定報表欄位');
                                                                        //$('#modalSettingReport h3').text(subtitle);
                                                                        $('#ddlSR_Station').val('');
                                                                        $('#modalSettingReport').modal('show');
                                                                    }
                                                                }
                                                            }
                                                        },
                                                        complete: function () {
                                                            $('#lnkSettingReoprtFields').html($('<i>').addClass('fa fa-cog fa-fw'));
                                                            rf_loading = false;
                                                        }
                                                    });
                                                }
                                            }
                                            else {
                                            }
                                        }
                                    });
                                }
                            }

                            function getReportFields(stationNo) {";
            js += "            var routeNo = $('#" + RouteControl.ID + "" + (RouteControl.Type == ControlType.DropDownList ? " option:selected" : "") + "').val();";
            js += @"            var data = { MappingName: routeNo, StationNo: stationNo };
                                $.ajax({
                                    type: 'POST',
                                    url: '/DeskTopModules/XYInOutStock_MultiFlow/Service/WebService1.asmx/GetReportFields',
                                    data: JSON.stringify(data),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (resp) {
                                        if (resp != null && resp.d.length > 0) {
                                            var d = JSON.parse(resp.d);
                                            if (d != null) {
                                                console.log(d);
                                                $('#ddlSR_InDate').val(d.InDate == null ? '' : d.InDate.Name);
                                                $('#ddlSR_OutDate').val(d.OutDate == null ? '' : d.OutDate.Name);
                                                $('#ddlSR_Price').val(d.Price == null ? '' : d.Price.Name);
                                                $('#ddlSR_Qty').val(d.Quantity == null ? '' : d.Quantity.Name);
                                                $('#ddlSR_Amount').val(d.Amount == null ? '' : d.Amount.Name);
                                                $('#ddlSR_RYG').val(d.RYG == null ? '' : d.RYG.Name);
                                                $('#ddlSR_STAT').val(d.StandardTAT == null ? '' : d.StandardTAT.Name);
                                                $('#ddlSR_NTAT').val(d.ActualTAT == null ? '' : d.ActualTAT.Name);
                                                $('#ddlSR_TalRYG').val(d.TotalRYG == null ? '' : d.TotalRYG.Name);
                                                $('#ddlSR_TalSTAT').val(d.TotalStandardTAT == null ? '' : d.TotalStandardTAT.Name);
                                                $('#ddlSR_TalNTAT').val(d.TotalActualTAT == null ? '' : d.TotalActualTAT.Name);
                                            }
                                        }
                                    },
                                    complete: function () {
                                        hideLoading();
                                    }
                                });
                            }
                        </script>";
            return js;
        }

        private string ModalHtml()
        {
            string htmlStr = @"
                <div class='modal fade bs-example-modal-lg' data-backdrop='static' id='modalSettingReport'>
                    <div class='modal-dialog modal-lg'>
                        <div class='modal-content'>
                            <div class='modal-header'>
                                <h1 class='modal-title'></h1>
                                <h3></h3>
                            </div>
                            <div class='modal-body'>
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>站點</label>
                                            <div class='controls'>
                                                <select id='ddlSR_Station' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                    <option value='ST1'>ST1</option>
                                                    <option value='ST2'>ST2</option>
                                                    <option value='ST3'>ST3</option>
                                                    <option value='ST4'>ST4</option>
                                                    <option value='ST5'>ST5</option>
                                                    <option value='ST6'>ST6</option>
                                                    <option value='ST7'>ST7</option>
                                                    <option value='ST8'>ST8</option>
                                                    <option value='ST9'>ST9</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>RYG</label>
                                            <div class='controls'>
                                                <select id='ddlSR_RYG' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>時間起</label>
                                            <div class='controls'>
                                                <select id='ddlSR_InDate' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>標準 TAT</label>
                                            <div class='controls'>
                                                <select id='ddlSR_STAT' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>時間迄</label>
                                            <div class='controls'>
                                                <select id='ddlSR_OutDate' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>實際 TAT</label>
                                            <div class='controls'>
                                                <select id='ddlSR_NTAT' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>單價</label>
                                            <div class='controls'>
                                                <select id='ddlSR_Price' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>Total RYG</label>
                                            <div class='controls'>
                                                <select id='ddlSR_TalRYG' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>數量</label>
                                            <div class='controls'>
                                                <select id='ddlSR_Qty' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>Total 標準 TAT</label>
                                            <div class='controls'>
                                                <select id='ddlSR_TalSTAT' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                
                                <div class='row-fluid'>
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>金額</label>
                                            <div class='controls'>
                                                <select id='ddlSR_Amount' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                    <div class='span6'>
                                        <div class='form-horizontal control-group'>
                                            <label class='control-label'>Total 實際 TAT</label>
                                            <div class='controls'>
                                                <select id='ddlSR_TalNTAT' class='m-wrap dnnFormInput span12'>
                                                    <option value=''>--</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/span-->
                                </div>
                                <!--/row-->
                            </div>
                            <div class='modal-footer'>
                                <button type='button' class='btn blue btn-primary' id='btnSaveReportFields'>Save</button>
                                <button type='button' class='btn btn-default' data-dismiss='modal'>Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /modal-end -->
                <style>
                    #modalSettingReport {
                        display: none;
                    }
                    #modalSettingReport .modal-header h1 {
                        color: red;
                    }
                </style>";
            return htmlStr;
        }
    }
}
