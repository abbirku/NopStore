using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A4A.BKASH.Factory;
using A4A.BKASH.Service;
using A4A.BKASH.ViewModel.Request;
using A4A.BKASH.ViewModel.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Bkash.Models;
using Nop.Plugin.Payments.Bkash.Utilities;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Payments.Bkash.Controllers
{
    public class PaymentBkashController : BasePaymentController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly BkashPaymentSettings _bkashPaymentSettings;
        private readonly IOrderService _orderService;
        private int _executeBKashPaymentCall;

        #endregion

        #region ctor

        public PaymentBkashController(IPermissionService permissionService,
            IStoreContext storeContext,
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            BkashPaymentSettings bkashPaymentSettings,
            IOrderService orderService)
        {
            _permissionService = permissionService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _workContext = workContext;
            _bkashPaymentSettings = bkashPaymentSettings;
            _orderService = orderService;
            _executeBKashPaymentCall = 0;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var bkashPaymentSettings = _settingService.LoadSetting<BkashPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                AppKey = bkashPaymentSettings.AppKey,
                AppSecret = bkashPaymentSettings.AppSecret,
                Username = bkashPaymentSettings.Username,
                Password = bkashPaymentSettings.Password,
                TestAppKey = bkashPaymentSettings.AppKey,
                TestAppSecret = bkashPaymentSettings.TestAppSecret,
                TestUsername = bkashPaymentSettings.TestUsername,
                TestPassword = bkashPaymentSettings.TestPassword,
                BaseUrl = bkashPaymentSettings.BaseUrl,
                UseSandbox = bkashPaymentSettings.UseSandbox,
                DoesCreateSuccessfulPayment = bkashPaymentSettings.DoesCreateSuccessfulPayment,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope <= 0)
                return View("~/Plugins/Payments.Bkash/Views/Configure.cshtml", model);

            model.AppKey_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.AppKey, storeScope);
            model.AppSecret_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.AppSecret, storeScope);
            model.Username_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.Username, storeScope);
            model.Password_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.Password, storeScope);
            model.TestAppKey_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestAppKey, storeScope);
            model.TestAppSecret_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestAppSecret, storeScope);
            model.TestUsername_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestUsername, storeScope);
            model.TestPassword_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.TestPassword, storeScope);
            model.BaseUrl_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.BaseUrl, storeScope);
            model.UseSandbox_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.UseSandbox, storeScope);
            model.DoesCreateSuccessfulPayment_OverrideForStore = _settingService.SettingExists(bkashPaymentSettings, x => x.DoesCreateSuccessfulPayment, storeScope);

            return View("~/Plugins/Payments.Bkash/Views/Configure.cshtml", model);
        }


        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();


            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var bkashPaymentSettings = _settingService.LoadSetting<BkashPaymentSettings>(storeScope);

            //Save settings
            bkashPaymentSettings.AppKey = model.AppKey;
            bkashPaymentSettings.AppSecret = model.AppSecret;
            bkashPaymentSettings.Username = model.Username;
            bkashPaymentSettings.Password = model.Password;
            bkashPaymentSettings.TestAppKey = model.TestAppKey;
            bkashPaymentSettings.TestAppSecret = model.TestAppSecret;
            bkashPaymentSettings.TestUsername = model.TestUsername;
            bkashPaymentSettings.TestPassword = model.TestPassword;
            bkashPaymentSettings.BaseUrl = model.BaseUrl;
            bkashPaymentSettings.UseSandbox = model.UseSandbox;
            bkashPaymentSettings.DoesCreateSuccessfulPayment = model.DoesCreateSuccessfulPayment;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.AppKey, model.AppKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.AppSecret, model.AppSecret_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.Username, model.Username_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestAppKey, model.TestAppKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestAppSecret, model.TestAppSecret_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestUsername, model.TestUsername_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.TestPassword, model.TestPassword_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.BaseUrl, model.BaseUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(bkashPaymentSettings, x => x.DoesCreateSuccessfulPayment, model.DoesCreateSuccessfulPayment_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult BkashCheckout(string orderNumber = "", decimal orderTotal = 0)
        {
            var orderNum = int.Parse(orderNumber);
            var paymentMethod = new PaymentModel
            {
                UseSandBox = _bkashPaymentSettings.UseSandbox,
                OrderNumber = orderNumber,
                Currency = /*_workContext.WorkingCurrency.CurrencyCode*/"BDT",
                OrderTotal = orderTotal,
                ReturnUrl = Url.Action("BkashError", "PaymentBkash", null, Request.Scheme),
                SuccessUrl = Url.Action("Completed", "Checkout", new { orderId = orderNum }, Request.Scheme),
                CreateUrl = Url.Action("BkashCreate", "PaymentBkash", null, Request.Scheme),
                ExecuteUrl = Url.Action("BkashExecute", "PaymentBkash", null, Request.Scheme)
            };

            return View("~/Plugins/Payments.Bkash/Views/BkashCheckout.cshtml", paymentMethod);
        }

        [HttpPost]
        public async Task<ActionResult> BkashCreate([FromForm]int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                var provider = new BkashProvider(_bkashPaymentSettings);
                var payload = new BkashCheckoutCreateRequestViewModel
                {
                    Provider = provider,
                    Amount = Math.Round(order.OrderTotal, 2),
                    Currency = "BDT",
                    Intent = "authorization", //"sale",
                    MerchantInvoiceNumber = id.ToString(),
                };

                var result = await BkashCheckoutService.CreatePayment(payload);

                var req = JsonConvert.SerializeObject(payload);
                var res = JsonConvert.SerializeObject(result);

                //Log.Info($"Grant Req: {result.SerRequest}");
                //Log.Info($"Grant Res: {result.SerResponse}");

                //Log.Info($"Create Payment Req: {req}");
                //Log.Info($"Create Payment Res: {res}");

                return Content(res, "application/json");
            }
            catch (Exception ex)
            {
                var res = JsonConvert.SerializeObject(new BkashCheckoutCreatePaymentResponse { ErrorMessage = ex.GetBaseException().Message });
                return Content(res, "application/json");
            }
        }

        [HttpPost]
        public async Task<ActionResult> BkashExecute([FromForm]Payload payload)
        {
            try
            {
                _executeBKashPaymentCall += 1;
                var terminate = false;
                var executeRes = "";
                var transactionId = string.Empty;
                var totalTime = 0;
                var t = new Thread(async () =>
                {
                    var provider = new BkashProvider(_bkashPaymentSettings);
                    var result = await BkashCheckoutService.ExecutePayment(new BkashCheckoutExecutePaymentRequestViewModel()
                    {
                        Provider = provider,
                        PaymentId = payload.PaymentID
                    });
                    executeRes = JsonConvert.SerializeObject(result);
                    //Log.Info($"Execute Payment Res: {executeRes}");

                    if (result.Success && string.IsNullOrEmpty(result.ErrorCode) && !terminate)
                    {
                        if (_bkashPaymentSettings.DoesCreateSuccessfulPayment)
                        {
                            var captureResult = await CaptureBkashPayment(payload.PaymentID);
                            var queryRes = await QueryPayment(payload.PaymentID);
                            var srcTrRes = await SearchTransactionDetails(result.TransactionId);
                        }
                        else
                        {
                            var voidPayment = await VoidBkashPayment(payload.PaymentID);
                        }
                        transactionId = result.TransactionId;
                    }

                    terminate = true;
                });
                t.Start();

                while (!terminate && totalTime < 32000)
                {
                    await Task.Delay(2000);
                    totalTime += 2000;
                }

                if (totalTime > 32000)
                {
                    terminate = true;
                    t.Abort();
                    var queryRes = await QueryPayment(payload.PaymentID);
                    if (queryRes.TransactionStatus == "Authorized")
                    {
                        var captureResult = await CaptureBkashPayment(payload.PaymentID);
                        executeRes = JsonConvert.SerializeObject(captureResult);
                        transactionId = queryRes.TransactionId;
                    }
                    else if (queryRes.TransactionStatus == "Initiated" && _executeBKashPaymentCall < 2)
                        await BkashCreate(payload.Id);
                }

                return Content(executeRes, "application/json");
            }
            catch (Exception ex)
            {
                var res = JsonConvert.SerializeObject(new BkashCheckoutExecutePaymentResponseViewModel { ErrorMessage = ex.GetBaseException().Message });
                return Content(res, "application/json");
            }
        }

        [HttpPost]
        public async Task<BkashCheckoutVoidResponseViewModel> CaptureBkashPayment(string orderNumber)
        {
            try
            {
                var provider = new BkashProvider(_bkashPaymentSettings);
                var result = await BkashCheckoutService.VoidOrCapturePayment(new BkashCheckoutVoidPaymentRequestViewModel
                {
                    Provider = provider,
                    PaymentId = orderNumber,
                    RequestType = VoideCheckoutRequestEnum.Capture
                });
                var jsonR = JsonConvert.SerializeObject(result);
                //Log.Info($"Capture Payment Res: {jsonR}");
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> VoidBkashPayment(string orderNumber)
        {
            try
            {
                var provider = new BkashProvider(_bkashPaymentSettings);
                var result = await BkashCheckoutService.VoidOrCapturePayment(new BkashCheckoutVoidPaymentRequestViewModel()
                {
                    Provider = provider,
                    PaymentId = orderNumber,
                    //IdToken = idToken,
                    //RefreshToken = refreshToken,
                    RequestType = VoideCheckoutRequestEnum.Void
                });
                var jsonR = JsonConvert.SerializeObject(result);
                //Log.Info($"Void Payment: {jsonR}");
                return Json(new { success = true, data = jsonR });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        public async Task<BkashCheckoutExecutePaymentResponseViewModel> QueryPayment(string paymentId)
        {
            try
            {
                //Dll->VeifyPayment
                //Project->VerifyPayment
                var provider = new BkashProvider(_bkashPaymentSettings);
                var result = await BkashCheckoutService.VerifyPayment(new BkashCheckoutVerifyPaymentRequestViewModel()
                {
                    Provider = provider,
                    PaymentId = paymentId
                });
                var jsonR = JsonConvert.SerializeObject(result);
                //Log.Info($"Query Payment Res: {jsonR}");
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<JsonResult> SearchTransactionDetails(string transactionId)
        {
            try
            {
                var provider = new BkashProvider(_bkashPaymentSettings);
                var result = await BkashCheckoutService.SearchTransactionDetails(new BkashSearchTransactionDetailsRequestViewModel()
                {
                    Provider = provider,
                    TransactionId = transactionId
                });
                var jsonR = JsonConvert.SerializeObject(result);
                //Log.Info($"Search Transaction Res: {jsonR}");
                return Json(new { success = true, data = jsonR });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        public IActionResult BkashError(string errorMessage = "")
        {
            var messageModel = new Message
            {
                ErrorMessage = errorMessage
            };
            return View("~/Plugins/Payments.Bkash/Views/BkashError.cshtml", messageModel);
        }
        #endregion
    }
}
