    namespace HairSalon.Web.Pages.Endpoints
    {
        public class ApplicationEndpoint
        {
            public const string Domain = "http://localhost:5255/api/v1/prn231-hairsalon/";

            // Authentication endpoints

            public const string CustomerLoginEndpoint = Domain + "customer/login";
        
            public const string EmployeeLoginEndpoint = Domain + "employee/login";


            // Customer endpoints

            public const string CustomerGetAllEndPoint = Domain + "customers";

            public const string CustomerRegisterEndPoint = Domain + "customer";

            // Appointment endpoints
            public const string AppointmentGetAllEndPoint = Domain + "appointments";
            public const string AppointmentGetByCustomerIdEndPoint = Domain + "appointments/customerId/";
            public const string AppointmentGetByStylistIdEndPoint = Domain + "appointments/stylistId/";
            public const string AppointmentGetByIdEndPoint = Domain + "appointments/";
            public const string AppointmentCreateEndPoint = Domain + "appointment";

            // Employee endpoints
            public const string EmployeeGetAllEndPoint = "http://localhost:5255/api/Employees/employees";
            public const string EmployeeGetByIdEndPoint = "http://localhost:5255/api/Employees/employees/";
            public const string EmployeeGetAvailableEndPoint = Domain + "date/";

            // EmployeeSchedule endpoints
            public const string EmployeeScheduleGetAllEndpoint = Domain + "schedules";

            // Hair service endpoints
            public const string GetHairServiceEndpoint = Domain + "hair-services";
            public const string GetHairServiceByIdEndpoint = Domain + "hair-services/";
            public const string CreateHairServiceEndpoint = Domain + "hair-service";
            public const string UpdateHairServiceEndpoint = Domain + "hair-service/";
            public const string DeleteHairServiceEndpoint = Domain + "hair-service/";

            // Payment endpounts
            public const string PaymentVNPayEndpoint = Domain + "payment/vnpay";

            /// <summary>
            /// Methods: POST, PUT, DELETE
            /// </summary>
            public const string OtherHairServiceEndpoint = Domain + "hair-service";
        }
    }
