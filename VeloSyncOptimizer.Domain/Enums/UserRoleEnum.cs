using System.Text.Json.Serialization;

namespace VeloSyncOptimizer.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRoleEnum
{
    Administrator = 1,
    WarehouseManager = 2,
    ProcurementManager = 3
}
