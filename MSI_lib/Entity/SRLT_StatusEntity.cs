using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class SRLT_StatusEntity
    {
        String _Status;
        String _ReceiveDate;
        String _ShipDate;
        String _Barcode;
        String _Rmano;
        String _Model;
        String _Marketname;
        String _Message;
        public String Status
        {
            set
            {
                this._Status = value;
            }
            get
            {
                return _Status;
            }
        }
        public String ReceiveDate
        {
            set
            {
                this._ReceiveDate = value;
            }
            get
            {
                return _ReceiveDate;
            }
        }
        public String ShipDate
        {
            set
            {
                this._ShipDate = value;
            }
            get
            {
                return _ShipDate;
            }
        }
        public String Barcode
        {
            set
            {
                this._Barcode = value;
            }
            get
            {
                return _Barcode;
            }
        }
        public String Rmano
        {
            set
            {
                this._Rmano = value;
            }
            get
            {
                return _Rmano;
            }
        }
        public String Model
        {
            set
            {
                this._Model = value;
            }
            get
            {
                return _Model;
            }
        }
        public String Marketname
        {
            set
            {
                this._Marketname = value;
            }
            get
            {
                return _Marketname;
            }
        }
        public String Message
        {
            set
            {
                this._Message = value;
            }
            get
            {
                return _Message;
            }
        }
    }
}
