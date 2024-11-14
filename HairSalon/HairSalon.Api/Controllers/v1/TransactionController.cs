using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Api.Controllers.v1
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;
        private readonly IAppointmentServices _appointmentServices;

        public TransactionController(
            ITransactionService transactionService,
            ICustomerService customerService,
            IAppointmentServices appointmentServices)
        {
            _transactionService = transactionService;
            _customerService = customerService;
            _appointmentServices = appointmentServices;
        }

        /// <summary>
        /// Create url
        /// </summary>
        [HttpPost("payment/vnpay")]
        [Authorize]
        public async Task<IActionResult> AddPayment(int appointmentId, int userId)
        {
            var user = await _customerService.GetCustomerById(userId);
            var appointment = await _appointmentServices.GetAppointment(appointmentId);
            try
            {
                var vnPayModel = new VnPaymentRequestModel()
                {
                    Amount = (decimal)appointment.AppointmentCost,
                    CreatedDate = DateTime.Now,
                    Description = "thanh toán VnPay",
                    OrderId = appointment.Id,
                };
                if (vnPayModel.Amount < 0)
                {
                    return BadRequest("The amount entered cannot be less than 0. Please try again");
                }
                var paymentUrl = _transactionService.CreatePaymentUrl(HttpContext, vnPayModel, user.Id);
                return Ok(new { url = paymentUrl });           
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
            string orderInfo = queryParameters["vnp_OrderInfo"];
            string responseCode = queryParameters["vnp_ResponseCode"];

            if (string.IsNullOrEmpty(orderInfo))
            {
                return BadRequest("Thông tin đơn hàng không tồn tại.");
            }


            string userId = _transactionService.GetUserId(orderInfo);
            string orderId = _transactionService.GetAppointmentId(orderInfo);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(orderId))
            {
                return BadRequest("Thông tin đơn hàng không đầy đủ.");
            }

            int appointmentId;
            if (!int.TryParse(orderId, out appointmentId))
            {
                return BadRequest("Mã đơn hàng không hợp lệ.");
            }

            if (responseCode != "00")
            {
                var updateResult = await _appointmentServices.UpdateAppointmentStatus(appointmentId, Core.Enums.AppointmentStatus.CANCELLED);
                if (updateResult == "Appointment status updated successfully")
                {
                    return Redirect("http://localhost:5178/Payments/PaymentFailed");
                }
                return BadRequest("Error");
            }


            if (!decimal.TryParse(queryParameters["vnp_Amount"], out decimal amount))
            {
                return BadRequest("Thông tin số tiền không hợp lệ.");
            }

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
            var paymentDto = new TransactionResponseDTO()
            {
                CustomerId = int.Parse(userId),
                AppointmentId = int.Parse(orderId),
                Status = "SUCCESS",
                Amount = amount,
                Method = "VnPay",
                TransactionDate = DateTime.UtcNow.AddHours(7),
            };

            var appointmentExistSuccess = await _appointmentServices.GetAppointment(paymentDto.AppointmentId);
            if (appointmentExistSuccess == null)
            {
                return BadRequest("Không tìm thấy thông tin cuộc hẹn.");
            }

            var result = await _transactionService.AddPayment(paymentDto);

            if (result == "Add succesfully")
            {
                var updateResult = await _appointmentServices.UpdateAppointmentStatus(appointmentId, Core.Enums.AppointmentStatus.VERIFIED);
                if (updateResult == "Appointment status updated successfully")
                {
                    return Redirect("http://localhost:5178/Payments/PaymentSuccess");
                }
                return BadRequest("Không thể cập nhật trạng thái cuộc hẹn.");
            }
            else
            {
                return BadRequest("Invalid transaction data.");
            }
        }
    }
}
