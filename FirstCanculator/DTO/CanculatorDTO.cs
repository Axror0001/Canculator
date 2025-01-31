namespace FirstCanculator.DTO
{
    public class CanculatorDTO
    {
        public string? Action { get; set; }
        public double? Result { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
