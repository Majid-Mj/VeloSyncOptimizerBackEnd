using MediatR;

public record DeleteWarehouseCommand(Guid Id) : IRequest<bool>;