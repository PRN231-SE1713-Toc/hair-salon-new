using System.Net;

namespace HairSalon.Core.Commons
{
    public record ApiResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public required string Message { get; set; }
        
        public T? Response { get; set; }
    }
}
