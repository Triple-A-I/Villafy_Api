using Villafy_Api.Models.Dto;

namespace Villafy_Api.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto>{
                new VillaDto { Id = 1, Name = "Kafoori block 12", Occupancy=2, Sqft=200 },
            new VillaDto { Id=2, Name="Burri block 2", Occupancy=4, Sqft=300},
            };
    }
}
