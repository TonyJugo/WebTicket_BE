using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebTicket.Application.Abstracts;
using WebTicket.Domain.Requests;
using WebTicket.Infrastructure.Options;

namespace WebTicket.Infrastructure.VnPay
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayOptions _vnPayOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VnPayService(IOptions<VnPayOptions> vnPayOptions, IHttpContextAccessor httpContextAccessor)
        {
            _vnPayOptions = vnPayOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePaymentUrl(PaymentInformationModel model)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();

            var urlCallBack = _vnPayOptions.ReturnUrl;

            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", _vnPayOptions.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            //pay.AddRequestData("vnp_BankCode", "VNPAYQR");
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor.HttpContext));
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(15).ToString("yyyyMMddHHmmss"));


            var paymentUrl = pay.CreateRequestUrl(_vnPayOptions.Url, _vnPayOptions.HashSecret);

            return paymentUrl;
        }


        public PaymentResponseModel PaymentExecute(Dictionary<string, string> collections)
        {
            var pay = new VnPayLibrary();

            // Thêm toàn bộ dữ liệu trả về từ VNPAY vào _responseData trong VnPayLibrary
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    pay.AddResponseData(key, value);
            }

            var orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));

            var paymentId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));

            var vnpResponseCode = pay.GetResponseData("vnp_ResponseCode");

            var vnpSecureHash =
                collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về

            var orderInfo = pay.GetResponseData("vnp_OrderInfo");

            var checkSignature =
                pay.ValidateSignature(vnpSecureHash, _vnPayOptions.HashSecret); //check Signature

            if (!checkSignature || vnpResponseCode != "00")
                return new PaymentResponseModel()
                {
                    Success = false
                };
            return new PaymentResponseModel()
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = orderId.ToString(),
                PaymentId = paymentId.ToString(),
                TransactionId = paymentId.ToString(),
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };

        }


    }
}
