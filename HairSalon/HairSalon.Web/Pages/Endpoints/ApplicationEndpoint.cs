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
        public const string AppointmentGetByUserIdEndPoint = Domain + "appointments";
        // Employee endpoints

        // Hair service endpoints

        public const string ServiceGetAllEndPoint = Domain + "hair-services";
    }
}
