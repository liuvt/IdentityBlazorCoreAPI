using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityBlazorCoreAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class User_PublishAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "30/07/2024 3:44:07 SA");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "30/07/2024 3:44:07 SA");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "30/07/2024 3:44:07 SA");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "30/07/2024 3:44:07 SA");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5",
                column: "ConcurrencyStamp",
                value: "30/07/2024 3:44:07 SA");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PublishedAt", "SecurityStamp" },
                values: new object[] { "38fdaf70-85c2-4e70-9736-632982888336", "AQAAAAIAAYagAAAAEI1hNaWINV3swQvF26efn5r54ChqtGHoqEcptTbWedriPvSJLVsPIsbA+OI6BPLx7Q==", null, "ac9e7b1c-02cd-4404-bdb3-0297939cb40e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BUYER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PublishedAt", "SecurityStamp" },
                values: new object[] { "07f27a7f-e4f5-45d1-8c09-053d65730a68", "AQAAAAIAAYagAAAAEDS5OTDqWOmJX7pmFmE7pin25vSHuS+ys+uO60MPCIqO0gHTFOD7c+xF5N7T+3DNrQ==", null, "892b92ac-21d5-4cdf-b97f-30190873b01c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "OWNER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PublishedAt", "SecurityStamp" },
                values: new object[] { "dccf21e0-06e4-45cc-85a2-0a31984e8320", "AQAAAAIAAYagAAAAEG+Ux58SADLpnjwEm/l2lvhCaCYNa2DcDURXI5yYETwGH/xGb6w2fVnAp1IkHEv5hw==", null, "b963d7cf-e7d1-4384-8710-aff8ee6e73dc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SELLER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PublishedAt", "SecurityStamp" },
                values: new object[] { "4d79e0d2-a3ee-4a01-9990-42d50092b473", "AQAAAAIAAYagAAAAEFpjWwe80o67Zzte/7KhTpi1JE2rfRAxjg5wAwShvCxkdzTfUornNipUDZnU8KrgPQ==", null, "98115bfe-d9f5-4291-a8ed-ef87345551a3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "29/07/2024 11:19:36 CH");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "29/07/2024 11:19:36 CH");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "29/07/2024 11:19:36 CH");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "29/07/2024 11:19:36 CH");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5",
                column: "ConcurrencyStamp",
                value: "29/07/2024 11:19:36 CH");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMIN-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38fa488d-2d53-41a4-910e-5444e4823e93", "AQAAAAIAAYagAAAAEKqJQ6Q7c9y/0GYRcvUteGhHkSxiekVWyz8fT2ZhkIuvMD4Xdn8y5nhAWA8/yUnsDA==", "0751326c-80b0-4fbf-b644-e7f03441842a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "BUYER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1cbedc58-3a07-4153-9630-c1f6611ed9ae", "AQAAAAIAAYagAAAAEMX13zn35Xo3bsfnQuYLxhWkLTA8gbBiRgsvnFejmTZ4ijhR0IofhAnEC/UaIxGZlg==", "54b7590c-928a-4ba5-b75a-e34c27d9d89d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "OWNER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "966010ad-773e-442f-9628-5f55c549d618", "AQAAAAIAAYagAAAAEAshLP9SzwD3ZG1K5pYO4aOJsNKEIVSCGjnZrI7if1Nb64jsT9hc3+mSm0i/ru5XBA==", "f32e832b-bc31-4ae7-bd5f-a9fdfaa58719" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SELLER-AUGCENTER-2023",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28d07aaa-5616-482d-8042-957adb82e8f4", "AQAAAAIAAYagAAAAEGuip/BhRb/ci40Gw4X6wJoGH5kkivyT2rwoYfEpAPp/e5SdkMpSxgDEBc3P4Spn1Q==", "66ca5f1e-b691-4c21-95b5-ff9d43d277d2" });
        }
    }
}
