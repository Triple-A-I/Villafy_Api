using Microsoft.AspNetCore.Mvc;
using Villafy_Api.Data;
using Villafy_Api.Models.Dto;

namespace Villafy_Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]

    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            var villaList = VillaStore.VillaList;
            return Ok(villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDto> GetVilla(int id)
        {
            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            if (id == 0)
            {
                return BadRequest("Invalid Id");
            }
            if (villa == null)
            {
                return NotFound("Villa not found");
            }

            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villa)
        {
            /// If you neglect [ApiController] you will use modelState
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            ///CUSTOM MODELSTATE VALIDATION
            if ((VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null))
            {
                ModelState.AddModelError("Customer Error", "Villa Already Exists");
                return BadRequest(ModelState);
            }
            {

            }
            if (villa == null)
            {
                return BadRequest();
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaStore.VillaList.Count() + 1;
            VillaStore.VillaList.Add(villa);

            //return Ok(villa);

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        public ActionResult DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.VillaList.Remove(villa);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public ActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);

            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.Sqft = villaDto.Sqft;

            return NoContent();

        }
    }
}
