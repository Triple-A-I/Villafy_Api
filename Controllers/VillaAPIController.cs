using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto.Villa;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]

    [ApiController]

    public class VillaAPIController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _mapper = mapper;
            _dbVilla = dbVilla;
            this._response = new();
        }

        [ResponseCache(CacheProfileName = "Default30")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]


        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery] int? occupancy, [FromQuery] string? search)
        {
            try
            {
                IEnumerable<Villa> villaList;
                if (occupancy > 0)
                {
                    villaList = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy);
                }
                else
                {

                    villaList = await _dbVilla.GetAllAsync();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(u => u.Amenity.ToLower().Contains(search) || u.Name.ToLower().Contains(search));
                }
                _response.Result = _mapper.Map<List<VillaDto>>(villaList);
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

        [HttpGet("{villaId:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<APIResponse>> GetVilla(int villaId)
        {
            try
            {
                var villa = await _dbVilla.GetAsync(v => v.VillaId == villaId);

                if (villaId == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because  = 0 " };
                    return BadRequest(_response);
                }
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Villa Not Found " };


                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
        {
            try
            {
                if ((await _dbVilla.GetAsync(u => u.Name.ToLower() == villaCreateDto.Name.ToLower()) != null))
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Villa Already Exists" };
                    return NotFound(_response);

                }

                if (villaCreateDto == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Not Valid" };
                    return NotFound(_response);
                }

                Villa villa = _mapper.Map<Villa>(villaCreateDto);


                await _dbVilla.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = villa.VillaId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpDelete("{villaId:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int villaId)
        {
            try
            {
                if (villaId == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Id = 0" };
                    return BadRequest(_response);
                }

                var villa = await _dbVilla.GetAsync(v => v.VillaId == villaId);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Villa Not Found" };
                    return NotFound(_response);
                }

                await _dbVilla.Remove(villa);
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

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpPut("{villaId:int}", Name = "UpdateVilla")]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int villaId, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            try
            {
                if (villaUpdateDto == null || villaId != villaUpdateDto.VillaId)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because it's null or Id Not Found" };
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.VillaId == villaId, tracked: false);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Villa Not Found" };
                    return BadRequest(_response);
                }

                Villa model = _mapper.Map<Villa>(villaUpdateDto);
                await _dbVilla.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> jsonPatch)
        {
            try
            {
                if (id == 0 || jsonPatch == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because Id = 0 " };
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.VillaId == id, tracked: false);
                VillaUpdateDto villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);
                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid Update because  Id is Not Found " };
                    return BadRequest(_response);
                }

                jsonPatch.ApplyTo(villaUpdateDto, ModelState);

                Villa model = _mapper.Map<Villa>(villaUpdateDto);
                await _dbVilla.UpdateAsync(model);
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
