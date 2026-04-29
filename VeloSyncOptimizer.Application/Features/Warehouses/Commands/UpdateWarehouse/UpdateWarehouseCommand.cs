using MediatR;

public record UpdateWarehouseCommand(
    Guid Id,
    string? Code,
    string? Name,
    string? City,
    string? State,
    string? Country,
    int? TotalCapacity,
    bool? IsActive
) : IRequest<bool>;