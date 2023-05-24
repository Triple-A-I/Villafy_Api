using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Controllers
{
    [Route("api/VillaNumberAPI")]

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto villaNumberCreateDto)
        {
            try
            {
                if ((await _villaNumberDb.GetAsync(u => u.VillaNo == villaNumberCreateDto.VillaNo) != null))
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "VillaNo Already Exists" };
                    return NotFound(_response);

                }

                if (villaNumberCreateDto == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Not Valid" };
                    return NotFound(_response);
                }

                VillaNumber villaNo = _mapper.Map<VillaNumber>(villaNumberCreateDto);


                await _villaNumberDb.CreateAsync(villaNo);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNo);
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { villaNo = villaNo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                var villaNumber = await _villaNumberDb.GetAsync(v => v.VillaNo == villaNo);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "VillaNumber Not Found" };
                    return NotFound(_response);
                }

                await _villaNumberDb.Remove(villaNumber);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int villaNo, [FromBody] VillaNumberUpdateDto villanumberUpdateDto)
        {
            try
            {
                if (villanumberUpdateDto == null || villaNo != villanumberUpdateDto.VillaNo)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because it's null or villaNo Not Found" };
                    return BadRequest(_response);
                }
                var villaNumber = await _villaNumberDb.GetAsync(v => v.VillaNo == villaNo, tracked: false);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "VillaNumber Not Found" };
                    return BadRequest(_response);
                }

                VillaNumber model = _mapper.Map<VillaNumber>(villanumberUpdateDto);
                await _villaNumberDb.UpdateAsync(model);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{villNo:int}", Name = "UpdatePartialVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int villNo, JsonPatchDocument<VillaNumberUpdateDto> jsonPatch)
        {
            try
            {
                if (villNo == 0 || jsonPatch == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because Id = 0 " };
                    return BadRequest(_response);
                }
                //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
                var villaNumber = await _villaNumberDb.GetAsync(v => v.VillaNo == villNo, tracked: false);
                VillaNumberUpdateDto villaNumberUpdateDto = _mapper.Map<VillaNumberUpdateDto>(villaNumber);
                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because  Id is Not Found " };
                    return BadRequest(_response);
                }

                jsonPatch.ApplyTo(villaNumberUpdateDto, ModelState);

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDto);
                await _villaNumberDb.UpdateAsync(model);
                if (!ModelState.IsValid)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update" };
                    return BadRequest(_response);
                }
                _response.statusCode = HttpStatusCode.NoContent;
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
