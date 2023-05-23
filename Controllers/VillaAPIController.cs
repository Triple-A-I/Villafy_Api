using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villafy_Api.Models;
using Villafy_Api.Models.Dto;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]

    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger;
        //public VillaAPIController(ILogger logger)
        //{
        //    _logger = logger;
        //}
        //private readonly ILogging _logger;
        private readonly IMapper _mapper;
        //private readonly ApplicationDbContext _dbContext;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            //_logger = logger;
            _mapper = mapper;
            _dbVilla = dbVilla;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDto>>(villaList);
            _response.statusCode = HttpStatusCode.OK;
            //var villaList = VillaStore.VillaList;

            //_logger.LogInformation("Logging all villas");
            //_logger.Log("Getting All Villas", "info");
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id);

            if (id == 0)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string> { "Invalid Update because  = 0 " };

                return BadRequest(_response);
                //_logger.LogError("Logging invald id " + id);
                //_logger.Log($"Logging invald id {id}", "error");

            }
            if (villa == null)
            {
                _response.statusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Villa Not Found " };


                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
        {
            /// If you neglect [ApiController] you will use modelState
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            ///CUSTOM MODELSTATE VALIDATION
            //if ((VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null))
            if ((await _dbVilla.GetAsync(u => u.Name.ToLower() == villaCreateDto.Name.ToLower()) != null))
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Villa Already Exists" };
                return BadRequest(_response);
                //ModelState.AddModelError("Customer Error", "Villa Already Exists");
                //return BadRequest(ModelState);
            }
            {

            }
            if (villaCreateDto == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Not Valid" };
                return BadRequest(_response);
            }
            //if (villa.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            //villa.Id = VillaStore.VillaList.Count() + 1;
            //villa.Id = _dbContext.Villas.Count() + 1;
            //Villa model = new()
            //{

            //    Sqft = villa.Sqft,
            //    Amenity = villa.Amenity,
            //    CreateDate = DateTime.Now,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //};
            //VillaStore.VillaList.Add(villa);
            Villa villa = _mapper.Map<Villa>(villaCreateDto);


            await _dbVilla.CreateAsync(villa);
            //return Ok(villa);
            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {

            if (id == 0)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Invalid Id = 0" };
                return BadRequest(_response);
            }

            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);

            var villa = await _dbVilla.GetAsync(v => v.Id == id);
            if (villa == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Villa Not Found" };
                return BadRequest(_response);
            }

            //VillaStore.VillaList.Remove(villa);
            await _dbVilla.Remove(villa);
            _response.statusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Invalid Update because it's null or Id Not Found" };
                return BadRequest(_response);
            }

            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);

            //villa.Name = villaDto.Name;
            //villa.Occupancy = villaDto.Occupancy;
            //villa.Sqft = villaDto.Sqft;
            //Villa model = new()
            //{
            //    Amenity = villaDto.Amenity,
            //    Details = villaDto.Details,
            //    Id = villaDto.Id,
            //    ImageUrl = villaDto.ImageUrl,
            //    Name = villaDto.Name,
            //    Sqft = villaDto.Sqft,
            //    Rate = villaDto.Rate,
            //    Occupancy = villaDto.Occupancy,
            //    UpdateDate = DateTime.Now
            //};

            Villa model = _mapper.Map<Villa>(villaUpdateDto);
            await _dbVilla.UpdateAsync(model);
            _response.statusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<ActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> jsonPatch)
        {

            if (id == 0 || jsonPatch == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Invalid Update because Id = 0 " };
                return BadRequest(_response);
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id, tracked: false);
            VillaUpdateDto villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);
            if (villa == null)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Invalid Update because  Id is Not Found " };
                return BadRequest(_response);
            }
            //VillaUpdateDto villaDto = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft,
            //    Id = villa.Id
            //};

            jsonPatch.ApplyTo(villaUpdateDto, ModelState);

            //Villa model = new()
            //{
            //    Id = villaDto.Id,
            //    Amenity = villaDto.Amenity,
            //    Name = villaDto.Name,
            //    Details = villaDto.Details,
            //    ImageUrl = villaDto.ImageUrl,
            //    Occupancy = villaDto.Occupancy,
            //    Rate = villaDto.Rate,
            //    Sqft = villaDto.Sqft,
            //    UpdateDate = DateTime.Now

            //};
            Villa model = _mapper.Map<Villa>(villaUpdateDto);
            await _dbVilla.UpdateAsync(model);
            //_dbContext.Villas.Update(model);
            if (!ModelState.IsValid)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = true;
                _response.ErrorMessages = new List<string> { "Invalid Update" };
                return BadRequest(_response);
            }
            _response.statusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);


        }
    }
}
