using MediatR;

public record DeleteWarehouseCommand(int Id) : IRequest<bool>;