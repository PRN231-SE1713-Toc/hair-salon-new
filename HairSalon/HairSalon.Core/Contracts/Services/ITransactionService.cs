using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Contracts.Services
{
    public interface ITransactionService
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model, int userid);
        Task<string> AddPayment(TransactionResponseDTO paymentResponseDto);
        VnPaymentResponseModel PaymentExecute(Dictionary<string, string> url);
        public string GetUserId(string orderInfo);
        public string GetAppointmentId(string orderInfo);
    }
}
