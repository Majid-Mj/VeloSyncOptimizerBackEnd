using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Safely drop the old table with typo if it exists
            migrationBuilder.Sql("IF OBJECT_ID('RefreshTockens', 'U') IS NOT NULL DROP TABLE RefreshTockens;");

            // Safely create the new table in the identity schema if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens' AND schema_id = SCHEMA_ID('identity'))
                BEGIN
                    CREATE TABLE [identity].RefreshTokens (
                        Id UNIQUEIDENTIFIER PRIMARY KEY,
                        UserId UNIQUEIDENTIFIER NOT NULL,
                        Token NVARCHAR(MAX) NOT NULL,
                        UserName NVARCHAR(256) NOT NULL,
                        RoleId INT NOT NULL,
                        ExpiresAt DATETIME2 NOT NULL,
                        IsRevoked BIT NOT NULL,
                        CreatedAt DATETIME2 NOT NULL,
                        CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES [identity].Users(Id)
                    )
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "identity");
        }
    }
}
