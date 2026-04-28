namespace VeloSyncOptimizer.Infrastructure.Dapper.Queries;

/// <summary>
/// Stored procedure names for the Warehouse feature.
/// </summary>
public static class WarehouseQueries
{
    /// <summary>[inventory].[sp_GetWarehouses] — returns all non-deleted warehouses.</summary>
    public const string GetAll = "[inventory].[sp_GetWarehouses]";
}
