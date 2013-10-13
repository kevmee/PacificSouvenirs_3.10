using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.PaymentExpress.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Tax;
using Nop.Plugin.Payments.PaymentExpress.Models;
using System.Web.Mvc;
using Nop.Services.Logging;

namespace Nop.Plugin.Payments.PaymentExpress
{
    /// <summary>
    /// PaymentExpress payment processor
    /// </summary>
    public class PaymentExpressPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly PaymentExpressPaymentSettings _PaymentExpressPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IWebHelper _webHelper;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly ITaxService _taxService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly HttpContextBase _httpContext;
        private readonly ILogger _logger;
        #endregion

        #region Ctor

        public PaymentExpressPaymentProcessor(PaymentExpressPaymentSettings PaymentExpressPaymentSettings,
            ISettingService settingService, ICurrencyService currencyService,
            CurrencySettings currencySettings, ILogger logger, IWebHelper webHelper,
            ICheckoutAttributeParser checkoutAttributeParser, ITaxService taxService,
            IOrderTotalCalculationService orderTotalCalculationService, HttpContextBase httpContext)
        {
            this._PaymentExpressPaymentSettings = PaymentExpressPaymentSettings;
            this._settingService = settingService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._webHelper = webHelper;
            this._checkoutAttributeParser = checkoutAttributeParser;
            this._taxService = taxService;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._httpContext = httpContext;
            this._logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.NewPaymentStatus = PaymentStatus.Pending;
            return result;
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var builder = new StringBuilder();

            var helper = new PaymentExpressHelper(_PaymentExpressPaymentSettings.PxUrl, _PaymentExpressPaymentSettings.PxUserId, _PaymentExpressPaymentSettings.PxPassword);

            RequestInput input = new RequestInput();


            input.AmountInput = postProcessPaymentRequest.Order.OrderTotal.ToString("F");
            input.CurrencyInput = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            input.MerchantReference = postProcessPaymentRequest.Order.Id.ToString();
            input.TxnType = "Purchase";
            input.UrlFail = _webHelper.GetStoreLocation(false) + "Plugins/PaymentPaymentExpress/CancelOrder";
            input.UrlSuccess = _webHelper.GetStoreLocation(false) + "Plugins/PaymentPaymentExpress/PaymentComplete";
            input.TxnId = postProcessPaymentRequest.Order.Id.ToString();

            RequestOutput output = helper.GenerateRequest(input);

            if (output.valid == "1")
            {
                // Redirect user to payment page
                _httpContext.Response.Redirect(output.Url);
            }
            else
            {
                string errorStr = string.Format("Failed to process Payment Express request. Message {0}", output.URI);
                _logger.Error(errorStr);

                throw new Exception("There was an issue connecting with our Payment Provider.");
            }
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {

            //var result = cart this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
            //    _PaymentExpressPaymentSettings.AdditionalFee, _PaymentExpressPaymentSettings.AdditionalFeePercentage);
            return 0M;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //let's ensure that at least 1 minute passed after order is placed
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes < 1)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "PaymentPaymentExpress";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.PaymentExpress.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaymentPaymentExpress";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.PaymentExpress.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(PaymentPaymentExpressController);
        }

        public override void Install()
        {
            //settings
            var settings = new PaymentExpressPaymentSettings()
            {
                PxUrl = "https://sec.paymentexpress.com/pxaccess/pxpay.aspx"
            };
            _settingService.SaveSetting(settings);

            //locales
            //this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.RedirectionTip", "You will be redirected to PayPal site to complete the order.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUrl", "Payment Epxress Url");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUrl.Hint", "This is the Url the user will be redirected too to complete payment");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUserId", "User Id");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUserId.Hint", "Specify your Payment Express User Id.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxPassword", "Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxPassword.Hint", "Specify your Payment Express Password.");
           base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<PaymentExpressPaymentSettings>();

            //locales
            //this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUrl");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUrl.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUserId");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxUserId.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxPassword");
            this.DeletePluginLocaleResource("Plugins.Payments.PaymentExpress.Fields.PxPassword.Hint");
            
            base.Uninstall();
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        #endregion
    }
}
