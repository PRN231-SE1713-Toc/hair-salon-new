﻿using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Api.Controllers.v1
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;
        private readonly IAppointmentServices _appointmentServices;

        public TransactionController(ITransactionService transactionService, ICustomerService customerService, IAppointmentServices appointmentServices)
        {
            _transactionService = transactionService;
            _customerService = customerService;
            _appointmentServices = appointmentServices;
        }

        /// <summary>
        /// Create url
        /// </summary>
        [HttpPost("payment/vnpay")]
        public async Task<IActionResult> AddPayment(int orderId, int userId)
        {
            var user = await _customerService.GetCustomerById(userId);
            var order = await _appointmentServices.GetAppointment(orderId);
            try
            {
                var vnPayModel = new VnPaymentRequestModel()
                {
                    Amount = (decimal)order.AppointmentCost,
                    CreatedDate = DateTime.Now,
                    Description = "thanh toán VnPay",
                    OrderId = order.Id,
                };
                if (vnPayModel.Amount < 0)
                {
                    return BadRequest("The amount entered cannot be less than 0. Please try again");
                }
                var paymentUrl = _transactionService.CreatePaymentUrl(HttpContext, vnPayModel, userId);
                return Ok(new { url = paymentUrl });
                //return Redirect(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));
                //return new JsonResult(_vpnPayServices.CreatePaymentUrl(HttpContext, vnPayModel, userId));               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentBack")]
        public async Task<IActionResult> PaymenCalltBack()
        {
            var queryParameters = HttpContext.Request.Query;
            // Kiểm tra và lấy giá trị 'vnp_OrderInfo' từ Query
            string orderInfo = queryParameters["vnp_OrderInfo"];
            string userId = _transactionService.GetUserId(orderInfo);
            string orderId = _transactionService.GetAppointmentId(orderInfo);
            decimal amount = decimal.Parse(queryParameters["vnp_Amount"]);
            if (string.IsNullOrEmpty(orderInfo))
            {
                return BadRequest("Thông tin đơn hàng không tồn tại.");
            }
            // Phân tích chuỗi 'orderInfo' để lấy các thông tin cần thiết
            var orderInfoDict = new Dictionary<string, string>();
            string[] pairs = orderInfo.Split(',');
            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                if (keyValue.Length == 2)
                {
                    orderInfoDict[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            //Tạo và lưu trữ thông tin giao dịch
            var paymentDto = new TransactionResponseDTO()
            {
                CustomerId = int.Parse(userId),
                AppointmentId = int.Parse(orderId),
                Status = "SUCCESS",
                Amount = amount,  // Chia cho 100 nếu giá trị 'amount' là theo đơn vị nhỏ nhất của tiền tệ
                Method = "VnPay",
                TransactionDate = DateTime.UtcNow.AddHours(7),

            };
            var result = await _transactionService.AddPayment(paymentDto);

            if (result == "AddSuccessful")
            {
                return Redirect("http://localhost:5000/" /*+ userId*/); // thay đổi đường link
            }
            return BadRequest("Invalid transaction data.");
        }


    }
}
