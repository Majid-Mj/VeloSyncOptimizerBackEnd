using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VeloSyncOptimizer.Application.Common.Interfaces;
using VeloSyncOptimizer.Application.Features.Warehouses.Commands.CreateWarehouse;
using VeloSyncOptimizer.Application.Features.Warehouses.DTOs;
using VeloSyncOptimizer.Infrastructure.Dapper.Queries;
using VeloSyncOptimizer.Infrastructure.Persistence.Context;
using VeloSyncOptimizer.Domain.Entities;

namespace VeloSyncOptimizer.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _db;

    public WarehouseRepository(AppDbContext db)
    {
        _db = db;
    }

    // ── READ — Dapper + Stored Procedure ──────────────────────────────────

    /// <summary>
    /// Calls [inventory].[sp_GetWarehouses] — returns all non-deleted warehouses.
    /// </summary>
    public async Task<IEnumerable<WarehouseDto>> GetAllAsync(CancellationToken ct)
    {
        using var conn = new SqlConnection(_db.Database.GetConnectionString());

        var result = await conn.QueryAsync<WarehouseDto>(
            WarehouseQueries.GetAll,
            commandType: System.Data.CommandType.StoredProcedure);

        return result;
    }

    // ── WRITE — EF Core ───────────────────────────────────────────────────

    /// <summary>
    /// Inserts a new warehouse via EF Core and returns its generated Id.
    /// </summary>
    public async Task<Guid> CreateAsync(CreateWarehouseCommand command, CancellationToken ct)
    {
        var warehouse = new Warehouse
        {
            Id            = Guid.NewGuid(),
            Code          = command.Code,
            Name          = command.Name,
            AddressLine1  = command.AddressLine1,
            City          = command.City,
            State         = command.State,
            Country       = command.Country,
            PostalCode    = command.PostalCode,
            Latitude      = command.Latitude,
            Longitude     = command.Longitude,
            TotalCapacity = command.TotalCapacity,
            ManagerId     = command.ManagerId,
            IsActive      = true,
            IsDeleted     = false,
            CreatedAt     = DateTime.UtcNow,
            UpdatedAt     = DateTime.UtcNow
        };

        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync(ct);

        return warehouse.Id;
    }
}
