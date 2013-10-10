using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PaymentExpress.Models
{
    /// <summary>
    /// Class containing properties describing transaction details
    /// </summary>
    public class RequestInput
    {
        private string _AmountInput;
        private string _BillingId;
        private string _CurrencyInput;
        private string _DpsBillingId;
        private string _DpsTxnRef;
        private string _EmailAddress;
        private string _EnableAddBillCard;
        private string _MerchantReference;
        private string _TxnData1;
        private string _TxnData2;
        private string _TxnData3;
        private string _TxnType;
        private string _TxnId;
        private string _UrlFail;
        private string _UrlSuccess;
        private string _Opt;


        public RequestInput()
        {
        }


        public string AmountInput
        {
            get
            {
                return _AmountInput;
            }
            set
            {
                _AmountInput = value;
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

        public string EnableAddBillCard
        {
            get
            {
                return _EnableAddBillCard;
            }
            set
            {
                _EnableAddBillCard = value;
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

        public string UrlFail
        {
            get
            {
                return _UrlFail;
            }
            set
            {
                _UrlFail = value;
            }
        }

        public string UrlSuccess
        {
            get
            {
                return _UrlSuccess;
            }
            set
            {
                _UrlSuccess = value;
            }
        }

        public string Opt
        {
            get
            {
                return _Opt;
            }
            set
            {
                _Opt = value;
            }
        }

        // If there are any additional input parameters simply add a new read/write property

    }
}
