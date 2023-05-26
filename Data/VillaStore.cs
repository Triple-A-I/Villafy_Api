using Villafy_Api.Models.Dto;

namespace Villafy_Api.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto> {

        new VillaDto { VillaId = 1, Name="Royal View", Occupancy=2, Sqft=200},
        new VillaDto { VillaId = 2, Name="Beach View", Occupancy=12, Sqft=2100},
        new VillaDto { VillaId = 3, Name="Village", Occupancy=6, Sqft=400},
        };
    }
}
