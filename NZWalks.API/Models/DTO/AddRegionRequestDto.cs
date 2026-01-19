namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        // id is not rquired it will generated automatically
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
