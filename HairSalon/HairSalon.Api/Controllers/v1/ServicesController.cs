using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Commons;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace HairSalon.Api.Controllers.v1
{
    public class ServicesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IHairService _hairService;

        public ServicesController(IMapper mapper, IHairService hairService)
        {
            _mapper = mapper;
            _hairService = hairService;
        }

        /// <summary>
        /// Get all hair services
        /// </summary>
        /// <returns></returns>
        [HttpGet("hair-services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<HairServiceResponse>>> GetServices()
        {
            var services = await _hairService.GetHairServices();
            if (!services.Any())
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No services found!"
                });
            }

            return Ok(services);
        }

        /// <summary>
        /// Get hair service by id
        /// </summary>
        /// <param name="id">Service's id</param>
        /// <returns></returns>
        [HttpGet("hair-services/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HairServiceResponse>> GetById(int id)
        {
            var hairService = await _hairService.GetService(id);
            if (hairService == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No service found!"
                });
            }
            return StatusCode(200, hairService);
        }

        /// <summary>
        /// Update service
        /// </summary>
        /// <param name="id">Hair service's id</param>
        /// <param name="request">Updated request model</param>
        /// <returns></returns>
        [HttpPut("hair-service/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateService(int id, UpdateHairServiceRequest request)
        {
            if (id != request.Id)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Id is not match!"
                });
            if (!ModelState.IsValid) return BadRequest();
            var result = await _hairService.UpdateService(request);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cannot update service!"
                });
            return NoContent();
        }

        [HttpPost("hair-service")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponseModel<string>>> Create(HairServiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _hairService.CreateService(request);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cannot create hair service!"
                });
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success!"
            });
        }

        /// <summary>
        /// Delete hair service
        /// </summary>
        /// <param name="id">Service's id</param>
        /// <returns></returns>
        [HttpDelete("hair-service/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _hairService.GetServiceById(id);
            if (service == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No service found!"
                });
            }
            var result = await _hairService.DeleteService(service);
            if (!result)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cannot delete service!"
                });
            return NoContent();
        }
        
        /*[HttpDelete("hair-service-filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHairServiceByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] ServiceFilterDTO serviceFilterDTO)
        {
            try
            {
                var result = await _hairService.GetHairServiceByFilterAsync(paginationParameter, serviceFilterDTO);

                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
