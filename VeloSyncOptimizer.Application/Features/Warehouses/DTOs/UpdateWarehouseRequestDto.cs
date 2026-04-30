public class UpdateWarehouseRequestDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }

    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }

    public int? TotalCapacity { get; set; }
    public bool? IsActive { get; set; }
}