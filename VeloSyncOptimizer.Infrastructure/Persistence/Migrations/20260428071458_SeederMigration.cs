using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeederMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The table is already in the identity schema, so we skip RenameTable
            
            // Safely add columns if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('[identity].RefreshTokens') AND name = 'RoleId')
                BEGIN
                    ALTER TABLE [identity].RefreshTokens ADD RoleId INT NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('[identity].RefreshTokens') AND name = 'UserName')
                BEGIN
                    ALTER TABLE [identity].RefreshTokens ADD UserName NVARCHAR(MAX) NOT NULL DEFAULT '';
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "identity",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "identity",
                newName: "RefreshTokens");
        }
    }
}
