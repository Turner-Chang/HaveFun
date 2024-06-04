using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberProfile",
                columns: table => new
                {
                    MemberProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BirthDay = table.Column<DateTime>(type: "Date", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfile", x => x.MemberProfileId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberProfile");
        }
    }
}
