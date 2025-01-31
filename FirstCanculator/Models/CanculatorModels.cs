namespace FirstCanculator.Models
{
    public class CanculatorModels
    {
        public int? Id { get; set; }
        public string? Action { get; set; }
        public double? Result { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
