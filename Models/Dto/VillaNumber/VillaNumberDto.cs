using System.ComponentModel.DataAnnotations;
using Villafy_Api.Models.Dto.Villa;

namespace Villafy_Api.Models.Dto.VillaNumber
{
    public class VillaNumberDto
    {
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        [Required]

        public int VillaId { get; set; }
        public VillaDto Villa { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
