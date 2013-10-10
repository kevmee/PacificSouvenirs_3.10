using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Payments.p
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //PxPay2.0
            routes.MapRoute("Plugin.Payments.PaymentExpress.PaymentComplete",
                 "Plugins/PaymentPaymentExpress/PaymentComplete",
                 new { controller = "PaymentPaymentExpress", action = "PaymentComplete" },
                 new[] { "Nop.Plugin.Payments.PaymentExpress.Controllers" }
            );
            ////IPN
            //routes.MapRoute("Plugin.Payments.PayPalStandard.IPNHandler",
            //     "Plugins/PaymentPayPalStandard/IPNHandler",
            //     new { controller = "PaymentPayPalStandard", action = "IPNHandler" },
            //     new[] { "Nop.Plugin.Payments.PayPalStandard.Controllers" }
            //);
            //Cancel
            routes.MapRoute("Plugin.Payments.PaymentExpress.CancelOrder",
                 "Plugins/PaymentPaymentExpress/CancelOrder",
                 new { controller = "PaymentPaymentExpress", action = "CancelOrder" },
                 new[] { "Nop.Plugin.Payments.PaymentExpress.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
