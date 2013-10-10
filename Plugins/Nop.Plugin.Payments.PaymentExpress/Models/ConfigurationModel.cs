﻿using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.PaymentExpress.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PaymentExpress.Fields.PxUrl")]
        public string PxUrl { get; set; }
        public bool PxUrl_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PaymentExpress.Fields.PxUserId")]
        public string PxUserId { get; set; }
        public bool PxUserId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.PaymentExpress.Fields.PxPassword")]
        public string PxPassword { get; set; }
        public bool PxPassword_OverrideForStore { get; set; }


        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.UseSandbox")]
        //public bool UseSandbox { get; set; }
        //public bool UseSandbox_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.BusinessEmail")]
        //public string BusinessEmail { get; set; }
        //public bool BusinessEmail_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.PDTToken")]
        //public string PdtToken { get; set; }
        //public bool PdtToken_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.PDTValidateOrderTotal")]
        //public bool PdtValidateOrderTotal { get; set; }
        //public bool PdtValidateOrderTotal_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.AdditionalFee")]
        //public decimal AdditionalFee { get; set; }
        //public bool AdditionalFee_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.AdditionalFeePercentage")]
        //public bool AdditionalFeePercentage { get; set; }
        //public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.PassProductNamesAndTotals")]
        //public bool PassProductNamesAndTotals { get; set; }
        //public bool PassProductNamesAndTotals_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.EnableIpn")]
        //public bool EnableIpn { get; set; }
        //public bool EnableIpn_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.IpnUrl")]
        //public string IpnUrl { get; set; }
        //public bool IpnUrl_OverrideForStore { get; set; }
    }
}