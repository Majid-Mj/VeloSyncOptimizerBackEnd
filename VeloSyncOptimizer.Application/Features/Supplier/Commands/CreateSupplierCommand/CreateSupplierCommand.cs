using MediatR;

public class CreateSupplierCommand : IRequest<Guid>
{
    public string Name { get; set; } = default!;

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }
}