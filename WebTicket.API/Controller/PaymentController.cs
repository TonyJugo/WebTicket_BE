using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTicket.Application.Abstracts;
using WebTicket.Domain.Requests;

namespace WebTicket.API.Controller
{
    [Route("Unitic/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        [HttpGet("CreatePaymentUrlVnpay")]
        public IActionResult CreatePaymentUrlVnpay()
        {
            PaymentInformationModel model = new PaymentInformationModel
            {
                Name = "Haha",
                Amount = 12000,
                OrderDescription = "Test",
                OrderType = "other"
            };
            var url = _vnPayService.CreatePaymentUrl(model);

            return Redirect(url);
        }

        
        [HttpGet("callback-vnpay")]
        public IActionResult PaymentCallbackVnpay()
        {
            // Convert query sang Dictionary
            var vnpayData = Request.Query
                .Where(x => x.Key.StartsWith("vnp_"))
                .ToDictionary(k => k.Key, v => v.Value.ToString());

            var response = _vnPayService.PaymentExecute(vnpayData);
            return new JsonResult(response);
        }


    }
}
