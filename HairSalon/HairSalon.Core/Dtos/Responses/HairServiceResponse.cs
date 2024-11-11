namespace HairSalon.Core.Dtos.Responses
{
    public class HairServiceResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? EstimatedDuration { get; set; }
        
        public decimal Price { get; set; }
    }
}
