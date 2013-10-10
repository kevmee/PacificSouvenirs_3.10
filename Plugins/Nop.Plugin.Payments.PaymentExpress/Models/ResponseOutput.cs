using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Payments.PaymentExpress.Models
{
    public class ResponseOutput
    {

        public ResponseOutput(string Xml)
        {
            _Xml = Xml;
            SetProperty();
        }

        private string _valid;
        private string _AmountSettlement;
        private string _AuthCode;
        private string _CardName;
        private string _CardNumber;
        private string _DateExpiry;
        private string _DpsTxnRef;
        private string _Success;
        private string _ResponseText;
        private string _DpsBillingId;
        private string _CardHolderName;
        private string _CurrencySettlement;
        private string _TxnData1;
        private string _TxnData2;
        private string _TxnData3;
        private string _TxnType;
        private string _CurrencyInput;
        private string _MerchantReference;
        private string _ClientInfo;
        private string _TxnId;
        private string _EmailAddress;
        private string _BillingId;
        private string _TxnMac;

        private string _Xml;

        public string valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }

        public string AmountSettlement
        {
            get
            {
                return _AmountSettlement;
            }
            set
            {
                _AmountSettlement = value;
            }
        }

        public string AuthCode
        {
            get
            {
                return _AuthCode;
            }
            set
            {
                _AuthCode = value;
            }
        }

        public string CardName
        {
            get
            {
                return _CardName;
            }
            set
            {
                _CardName = value;
            }
        }

        public string CardNumber
        {
            get
            {
                return _CardNumber;
            }
            set
            {
                _CardNumber = value;
            }
        }

        public string DateExpiry
        {
            get
            {
                return _DateExpiry;
            }
            set
            {
                _DateExpiry = value;
            }
        }

        public string DpsTxnRef
        {
            get
            {
                return _DpsTxnRef;
            }
            set
            {
                _DpsTxnRef = value;
            }
        }

        public string Success
        {
            get
            {
                return _Success;
            }
            set
            {
                _Success = value;
            }
        }

        public string ResponseText
        {
            get
            {
                return _ResponseText;
            }
            set
            {
                _ResponseText = value;
            }
        }

        public string DpsBillingId
        {
            get
            {
                return _DpsBillingId;
            }
            set
            {
                _DpsBillingId = value;
            }
        }

        public string CardHolderName
        {
            get
            {
                return _CardHolderName;
            }
            set
            {
                _CardHolderName = value;
            }
        }

        public string CurrencySettlement
        {
            get
            {
                return _CurrencySettlement;
            }
            set
            {
                _CurrencySettlement = value;
            }
        }

        public string TxnData1
        {
            get
            {
                return _TxnData1;
            }
            set
            {
                _TxnData1 = value;
            }
        }

        public string TxnData2
        {
            get
            {
                return _TxnData2;
            }
            set
            {
                _TxnData2 = value;
            }
        }

        public string TxnData3
        {
            get
            {
                return _TxnData3;
            }
            set
            {
                _TxnData3 = value;
            }
        }

        public string TxnType
        {
            get
            {
                return _TxnType;
            }
            set
            {
                _TxnType = value;
            }
        }

        public string CurrencyInput
        {
            get
            {
                return _CurrencyInput;
            }
            set
            {
                _CurrencyInput = value;
            }
        }


        public string MerchantReference
        {
            get
            {
                return _MerchantReference;
            }
            set
            {
                _MerchantReference = value;
            }
        }

        public string ClientInfo
        {
            get
            {
                return _ClientInfo;
            }
            set
            {
                _ClientInfo = value;
            }
        }

        public string TxnId
        {
            get
            {
                return _TxnId;
            }
            set
            {
                _TxnId = value;
            }
        }

        public string EmailAddress
        {
            get
            {
                return _EmailAddress;
            }
            set
            {
                _EmailAddress = value;
            }
        }

        public string BillingId
        {
            get
            {
                return _BillingId;
            }
            set
            {
                _BillingId = value;
            }
        }

        public string TxnMac
        {
            get
            {
                return _TxnMac;
            }
            set
            {
                _TxnMac = value;
            }
        }

        public override string ToString()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (PropertyInfo oPropertyInfo in properties)
            {

                if (oPropertyInfo.CanRead)
                {
                    sb.AppendLine(string.Format("{0} : {1}",oPropertyInfo.Name, (string)oPropertyInfo.GetValue(this, null)));
                }
            }
            return sb.ToString();
        }
        // If there are any additional elements or attributes added to the output XML simply add a property of the same name.

        private void SetProperty()
        {

            XmlReader reader = XmlReader.Create(new StringReader(_Xml));

            while (reader.Read())
            {
                PropertyInfo prop;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    prop = this.GetType().GetProperty(reader.Name);
                    if (prop != null)
                    {
                        this.GetType().GetProperty(reader.Name).SetValue(this, reader.ReadString(), System.Reflection.BindingFlags.Default, null, null, null);
                    }
                    if (reader.HasAttributes)
                    {

                        for (int count = 0; count < reader.AttributeCount; count++)
                        {
                            //Read the current attribute
                            reader.MoveToAttribute(count);
                            prop = this.GetType().GetProperty(reader.Name);
                            if (prop != null)
                            {
                                this.GetType().GetProperty(reader.Name).SetValue(this, reader.Value, System.Reflection.BindingFlags.Default, null, null, null);
                            }
                        }
                    }
                }
            }

        }


    }
}
