using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Api.Controllers.v1
{
    [Route("api/v{v:apiVersion}/prn231-hairsalon")]
    [ApiController]
    [ApiVersion(1)]
    public class BaseApiController : ControllerBase
    {
    }
}
