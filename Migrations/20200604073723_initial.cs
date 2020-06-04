using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListRecruitment.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    idList = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nameList = table.Column<string>(nullable: false),
                    colorHexList = table.Column<string>(nullable: false),
                    statusList = table.Column<int>(nullable: false),
                    created = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.idList);
                });

            migrationBuilder.CreateTable(
                name: "ListItems",
                columns: table => new
                {
                    idListItem = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nameListItem = table.Column<string>(nullable: false),
                    descListItem = table.Column<string>(nullable: false),
                    isDoneListItem = table.Column<int>(nullable: false),
                    created = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    updated = table.Column<DateTime>(nullable: false),
                    idList = table.Column<long>(nullable: false),
                    ListidList = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItems", x => x.idListItem);
                    table.ForeignKey(
                        name: "FK_ListItems_Lists_ListidList",
                        column: x => x.ListidList,
                        principalTable: "Lists",
                        principalColumn: "idList",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListItems_ListidList",
                table: "ListItems",
                column: "ListidList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListItems");

            migrationBuilder.DropTable(
                name: "Lists");
        }
    }
}
