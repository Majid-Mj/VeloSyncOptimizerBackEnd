using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloSyncOptimizer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetUserByEmailSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [identity].[sp_GetUserByEmail]
                    @Email NVARCHAR(256)
                AS
                BEGIN
                    SELECT 
                        u.*, 
                        r.Name as RoleName 
                    FROM [identity].[Users] u
                    JOIN [identity].[UserRoles] r ON u.RoleId = r.Id
                    WHERE u.Email = @Email AND u.IsDeleted = 0
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [identity].[sp_GetPendingUsers]
                AS
                BEGIN
                    SELECT 
                        u.Id,
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        r.Name as RoleName,
                        u.IsActive,
                        u.IsApproved,
                        u.CreatedAt
                    FROM [identity].[Users] u
                    JOIN [identity].[UserRoles] r ON u.RoleId = r.Id
                    WHERE u.IsApproved = 0 AND u.IsDeleted = 0
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
