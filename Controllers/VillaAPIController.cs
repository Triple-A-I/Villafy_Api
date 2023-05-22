using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            //_logger = logger;
            _mapper = mapper;
            _dbVilla = dbVilla;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            //var villaList = VillaStore.VillaList;

            //_logger.LogInformation("Logging all villas");
            //_logger.Log("Getting All Villas", "info");
            return Ok(_mapper.Map<List<VillaDto>>(villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id);

            if (id == 0)
            {
                //_logger.LogError("Logging invald id " + id);
                //_logger.Log($"Logging invald id {id}", "error");
                return BadRequest("Invalid Id");
            }
            if (villa == null)
            {
                return NotFound("Villa not found");
            }

            return Ok(_mapper.Map<VillaDto>(villa));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
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
                ModelState.AddModelError("Customer Error", "Villa Already Exists");
                return BadRequest(ModelState);
            }
            {

            }
            if (villaCreateDto == null)
            {
                return BadRequest();
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
            Villa model = _mapper.Map<Villa>(villaCreateDto);

            await _dbVilla.CreateAsync(model);
            //return Ok(villa);

            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        public async Task<ActionResult> DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);

            var villa = await _dbVilla.GetAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            //VillaStore.VillaList.Remove(villa);
            await _dbVilla.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                return BadRequest();
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
            return NoContent();

        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<ActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> jsonPatch)
        {

            if (id == 0 || jsonPatch == null)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id, tracked: false);
            VillaUpdateDto villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);
            if (villa == null)
            {
                return BadRequest();
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
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
