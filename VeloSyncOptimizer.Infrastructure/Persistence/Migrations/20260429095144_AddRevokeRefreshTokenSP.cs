using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRevokeRefreshTokenSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                schema: "identity",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [identity].[sp_RevokeRefreshToken]
                    @Token NVARCHAR(MAX)
                AS
                BEGIN
                    UPDATE [identity].[RefreshTokens]
                    SET IsRevoked = 1,
                        RevokedAt = GETUTCDATE()
                    WHERE Token = @Token AND IsRevoked = 0
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokedAt",
                schema: "identity",
                table: "RefreshTokens");
        }
    }
}
