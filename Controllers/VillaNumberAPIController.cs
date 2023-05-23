using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _villaNumberDb;
        private readonly IMapper _mapper;

        public VillaNumberAPIController(IMapper mapper, IVillaNumberRepository villaNumberRepository)
        {
            _mapper = mapper;
            _villaNumberDb = villaNumberRepository;
            this._response = new();
        }

        protected APIResponse _response;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {

            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _villaNumberDb.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumberList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;

        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                var villa = await _villaNumberDb.GetAsync(v => v.VillaNo == villaNo);

                if (villaNo == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because villaNo = 0 " };
                    return BadRequest(_response);

                }
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "VillaNumber Not Found " };


                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDto>(villa);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

    }
}
