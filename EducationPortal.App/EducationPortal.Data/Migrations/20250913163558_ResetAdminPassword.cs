using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResetAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "056320a1-0590-4673-b333-1508948bf6c9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2f2db4ea-0a97-42a2-b824-a27261e63eb9");
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c06f3f8-aebc-4ec6-b381-c6bc1557bfd9", "password123", "81e6b56d-e891-484d-bcf6-84027862f26f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e6054b99-f589-4071-a38f-0a37d8b2548c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "09aadba0-dc2e-4946-98a7-22ab304cf673");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "8083c90d-d14e-435d-a1f8-23552247caeb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "5ebfb62e-7909-43f0-a301-80a0cb41509c");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 35, 58, 144, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 35, 58, 144, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 35, 58, 144, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 35, 58, 144, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 35, 58, 144, DateTimeKind.Utc).AddTicks(2530));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "34757e61-adfa-4a93-a488-6c1f16bce129");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "551de843-ebce-43e6-b660-1493baff6529");

            // Restore original admin password hash
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "59b96f4d-547f-4cb9-b576-ef53e0dc1ef9", "AQAAAAEAACcQAAAAEHy8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q==", "a3adab0c-7035-4c8d-abbb-c26ad7bf748d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e97e781a-34c6-49f1-a146-1a9588061edb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e111b56c-6466-4aa2-84fa-77bda799fdfb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d9bce5b1-04b1-41c5-a545-0cf2b8aa1fd5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "39b44096-c995-47bb-9dbc-b1eaca8bea1d");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 0, 36, 266, DateTimeKind.Utc).AddTicks(3110));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 0, 36, 266, DateTimeKind.Utc).AddTicks(3120));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 0, 36, 266, DateTimeKind.Utc).AddTicks(3120));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 0, 36, 266, DateTimeKind.Utc).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 13, 16, 0, 36, 266, DateTimeKind.Utc).AddTicks(3140));
        }
    }
}
