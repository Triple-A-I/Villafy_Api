using System.ComponentModel.DataAnnotations;

namespace Villafy_Api.Models.Dto
{
    public class VillaNumberUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
