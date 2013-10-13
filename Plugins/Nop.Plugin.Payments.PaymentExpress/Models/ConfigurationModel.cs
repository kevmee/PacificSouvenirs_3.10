using Nop.Web.Framework;
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

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.AdditionalFee")]
        //public decimal AdditionalFee { get; set; }
        //public bool AdditionalFee_OverrideForStore { get; set; }

        //[NopResourceDisplayName("Plugins.Payments.PayPalStandard.Fields.AdditionalFeePercentage")]
        //public bool AdditionalFeePercentage { get; set; }
        //public bool AdditionalFeePercentage_OverrideForStore { get; set; }
    }
}