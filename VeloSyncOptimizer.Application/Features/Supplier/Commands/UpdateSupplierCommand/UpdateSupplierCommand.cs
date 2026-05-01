using MediatR;

public class UpdateSupplierCommand : IRequest<bool>
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public bool? IsActive { get; set; }
}