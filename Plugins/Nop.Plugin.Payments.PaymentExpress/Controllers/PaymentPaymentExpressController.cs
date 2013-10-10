using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.PaymentExpress.Models;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.PaymentExpress.Controllers
{
    public class PaymentPaymentExpressController : BaseNopPaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly PaymentSettings _paymentSettings;

        public PaymentPaymentExpressController(IWorkContext workContext,
            IStoreService storeService, 
            ISettingService settingService, 
            IPaymentService paymentService, 
            IOrderService orderService, 
            IOrderProcessingService orderProcessingService, 
            ILogger logger, IWebHelper webHelper,
            PaymentSettings paymentSettings)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._logger = logger;
            this._webHelper = webHelper;
            this._paymentSettings = paymentSettings;
        }
        
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var pxPaymentSettings = _settingService.LoadSetting<PaymentExpressPaymentSettings>(storeScope);

            var model = new ConfigurationModel();
            model.PxUrl = pxPaymentSettings.PxUrl;
            model.PxUserId = pxPaymentSettings.PxUserId;
            model.PxPassword = pxPaymentSettings.PxPassword;

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.PxUrl_OverrideForStore = _settingService.SettingExists(pxPaymentSettings, x => x.PxUrl, storeScope);
                model.PxUserId_OverrideForStore = _settingService.SettingExists(pxPaymentSettings, x => x.PxUserId, storeScope);
                model.PxPassword_OverrideForStore = _settingService.SettingExists(pxPaymentSettings, x => x.PxPassword, storeScope);
            }

            return View("Nop.Plugin.Payments.PaymentExpress.Views.PaymentExpress.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var pxPaymentSettings = _settingService.LoadSetting<PaymentExpressPaymentSettings>(storeScope);

            //save settings
            pxPaymentSettings.PxUrl = model.PxUrl;
            pxPaymentSettings.PxUserId = model.PxUserId;
            pxPaymentSettings.PxPassword = model.PxPassword;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.PxUrl_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pxPaymentSettings, x => x.PxUrl, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pxPaymentSettings, x => x.PxUrl, storeScope);

            if (model.PxUserId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pxPaymentSettings, x => x.PxUserId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pxPaymentSettings, x => x.PxUserId, storeScope);

            if (model.PxPassword_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pxPaymentSettings, x => x.PxPassword, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pxPaymentSettings, x => x.PxPassword, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            return View("Nop.Plugin.Payments.PaymentExpress.Views.PaymentExpress.PaymentInfo");
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }

        [ValidateInput(false)]
        public ActionResult PaymentComplete(FormCollection form)
        {
            string result = _webHelper.QueryString<string>("result");

            var processor = _paymentService.LoadPaymentMethodBySystemName("Payments.PaymentExpress") as PaymentExpressPaymentProcessor;
            if (processor == null ||
                !processor.IsPaymentMethodActive(_paymentSettings) || !processor.PluginDescriptor.Installed)
                throw new NopException("Payment Express module cannot be loaded");


            if (!string.IsNullOrEmpty(result))
            {
                //load settings for a chosen store scope
                var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
                var pxPaymentSettings = _settingService.LoadSetting<PaymentExpressPaymentSettings>(storeScope);

                // Obtain the transaction result
                PaymentExpressHelper helper = new PaymentExpressHelper(pxPaymentSettings.PxUrl, 
                                                                        pxPaymentSettings.PxUserId, 
                                                                        pxPaymentSettings.PxPassword);

                ResponseOutput output = helper.ProcessResponse(result);
                int orderId;
                
                if(!int.TryParse(output.MerchantReference, out orderId))
                {
                    string errorStr = string.Format("Payment Express Payment Complete. Invalid order Id returned from DPS: {0}", output.MerchantReference);
                    _logger.Error(errorStr);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                if (output.valid == "1" && output.Success == "1")
                {
                    var order = _orderService.GetOrderById(orderId);

                    //order note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = output.ToString(),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);
                    
                    //validate order total
                    if (!Math.Round(Decimal.Parse(output.AmountSettlement), 2).Equals(Math.Round(order.OrderTotal, 2)))
                    {
                        string errorStr = string.Format("Payment Express PaymentComplete. Returned order total {0} doesn't equal order total {1}", output.AmountSettlement, order.OrderTotal);
                        _logger.Error(errorStr);

                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    //mark order as paid
                    if (_orderProcessingService.CanMarkOrderAsPaid(order))
                    {
                        order.AuthorizationTransactionId = output.TxnId;
                        _orderService.UpdateOrder(order);

                        _orderProcessingService.MarkOrderAsPaid(order);
                    }
                    
                    return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
                }
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult CancelOrder(FormCollection form)
        {
             string result = _webHelper.QueryString<string>("result");

            var processor = _paymentService.LoadPaymentMethodBySystemName("Payments.PaymentExpress") as PaymentExpressPaymentProcessor;
            if (processor == null ||
                !processor.IsPaymentMethodActive(_paymentSettings) || !processor.PluginDescriptor.Installed)
                throw new NopException("Payment Express module cannot be loaded");


            if (!string.IsNullOrEmpty(result))
            {
                //load settings for a chosen store scope
                var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
                var pxPaymentSettings = _settingService.LoadSetting<PaymentExpressPaymentSettings>(storeScope);

                // Obtain the transaction result
                PaymentExpressHelper helper = new PaymentExpressHelper(pxPaymentSettings.PxUrl,
                                                                        pxPaymentSettings.PxUserId,
                                                                        pxPaymentSettings.PxPassword);

                ResponseOutput output = helper.ProcessResponse(result);
                int orderId;

                if (!int.TryParse(output.MerchantReference, out orderId))
                {
                    string errorStr = string.Format("Payment Express Payment Complete. Invalid order Id returned from DPS: {0}", output.MerchantReference);
                    _logger.Error(errorStr);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                if (output.valid == "1")
                {
                    var order = _orderService.GetOrderById(orderId);

                    //order note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = output.ToString(),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });

                    _orderService.UpdateOrder(order);

                    _orderProcessingService.CancelOrder(order, true);
                }


            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}