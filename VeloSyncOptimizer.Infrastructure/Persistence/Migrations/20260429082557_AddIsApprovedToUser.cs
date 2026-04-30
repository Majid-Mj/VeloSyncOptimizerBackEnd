using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE object_id = OBJECT_ID('[identity].[Users]') 
                    AND name = 'IsApproved'
                )
                BEGIN
                    ALTER TABLE [identity].[Users] ADD [IsApproved] BIT NOT NULL DEFAULT 0;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "identity",
                table: "Users");
        }
    }
}
