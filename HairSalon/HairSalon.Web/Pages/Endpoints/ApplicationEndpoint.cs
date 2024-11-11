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

        // Appointment endpoints
        public const string AppointmentGetAllEndPoint = Domain + "appointments";
        public const string AppointmentGetByCustomerIdEndPoint = Domain + "appointments/customerId/";
        public const string AppointmentGetByIdEndPoint = Domain + "appointments/";

        // Employee endpoints
        public const string EmployeeGetByIdEndPoint = "https://localhost:7200/api/Employees/employees/";

        // Hair service endpoints
        public const string GetHairServiceEndpoint = Domain + "hair-services";

        /// <summary>
        /// Methods: POST, PUT, DELETE
        /// </summary>
        public const string OtherHairServiceEndpoint = Domain + "hair-service";
    }
}
