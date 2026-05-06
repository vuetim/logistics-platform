public class OrderDateDto
{
    public DateTime Date { get; set; }
    public string? Timezone { get; set; }
    public bool HasTime { get; set; } = true;
}
