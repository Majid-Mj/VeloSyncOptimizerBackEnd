using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateWarehouseSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [inventory].[sp_UpdateWarehouse]
                    @Id         UNIQUEIDENTIFIER,
                    @Code       NVARCHAR(50)  = NULL,
                    @Name       NVARCHAR(200) = NULL,
                    @City       NVARCHAR(100) = NULL,
                    @State      NVARCHAR(100) = NULL,
                    @Country    NVARCHAR(100) = NULL,
                    @TotalCapacity INT         = NULL,
                    @IsActive   BIT            = NULL
                AS
                BEGIN
                    UPDATE [inventory].[Warehouses]
                    SET
                        Code           = ISNULL(@Code,           Code),
                        Name           = ISNULL(@Name,           Name),
                        City           = ISNULL(@City,           City),
                        State          = ISNULL(@State,          State),
                        Country        = ISNULL(@Country,        Country),
                        TotalCapacity  = ISNULL(@TotalCapacity,  TotalCapacity),
                        IsActive       = ISNULL(@IsActive,       IsActive),
                        UpdatedAt      = GETUTCDATE()
                    WHERE Id = @Id AND IsDeleted = 0
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
