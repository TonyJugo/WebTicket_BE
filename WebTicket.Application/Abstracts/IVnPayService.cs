using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Requests;

namespace WebTicket.Application.Abstracts
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model);
        PaymentResponseModel PaymentExecute(Dictionary<string, string> collections);

    }
}
