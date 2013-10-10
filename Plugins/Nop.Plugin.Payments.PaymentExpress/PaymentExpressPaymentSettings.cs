using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.PaymentExpress
{
    public class PaymentExpressPaymentSettings : ISettings
    {
        public string PxUrl { get; set; }
        public string PxUserId { get; set; }
        public string PxPassword { get; set; }
    }
}
