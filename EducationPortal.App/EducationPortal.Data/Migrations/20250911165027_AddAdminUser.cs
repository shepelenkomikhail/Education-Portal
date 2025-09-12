using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EducationPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "93f487bb-95c7-41a3-9956-3e7a0e10cac3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6f5cfc2a-ce46-4d32-81f5-97feb09ee2f4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "Surname", "UserName" },
                values: new object[] { "09975ca1-5691-4c02-a79e-f2ce716cbe9d", "admin@educationportal.com", true, "Admin", "ADMIN@EDUCATIONPORTAL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHy8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q8X5U7Q==", "c772ddaf-cf2a-4f71-ae19-58dd50d83186", "User", "admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "bdaf8f68-5334-4cc1-97b8-a4ff8c46290a", "Jane" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "045d7041-8b0f-4773-983d-231438af0888", "Bob" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "178a16ce-3add-480e-af4c-304b768d45f1", "Alice" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "fc178685-14c0-408f-9096-904237f004f9", "Charlie" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 16, 50, 27, 132, DateTimeKind.Utc).AddTicks(7640));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 16, 50, 27, 132, DateTimeKind.Utc).AddTicks(7650));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 16, 50, 27, 132, DateTimeKind.Utc).AddTicks(7650));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 16, 50, 27, 132, DateTimeKind.Utc).AddTicks(7670));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 11, 16, 50, 27, 132, DateTimeKind.Utc).AddTicks(7670));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0a6fb46f-a61a-484f-8d20-20cdc8b42509");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4461b7a6-7377-4b53-957e-474b4799a21b");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "Surname", "UserName" },
                values: new object[] { "125c0735-0450-46a3-9055-86dfc3b69192", "john.doe@email.com", false, "", null, null, "password123", null, "Doe", "John" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "c2ec0f0a-48db-4968-9902-3b61493590e0", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "c535adc0-4ce0-45be-a36e-881fb0ea9ef9", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "7c412716-6d4e-423f-a97d-7d961418b3b0", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "FirstName" },
                values: new object[] { "3e4c8546-ad6c-4eea-8fa0-f48dc7ba429f", "" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 17, 31, 21, 75, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 17, 31, 21, 75, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 17, 31, 21, 75, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 17, 31, 21, 75, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 17, 31, 21, 75, DateTimeKind.Utc).AddTicks(9800));
        }
    }
}
