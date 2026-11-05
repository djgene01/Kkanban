using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kkanban.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Colour = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Assignee = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BoardColumnId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Columns_BoardColumnId",
                        column: x => x.BoardColumnId,
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardLabels",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    LabelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLabels", x => new { x.CardId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_CardLabels_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardLabels_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Columns",
                columns: new[] { "Id", "ArchivedAt", "IsDeleted", "Order", "Title" },
                values: new object[,]
                {
                    { 1, null, false, 0, "To Do" },
                    { 2, null, false, 1, "In Progress" },
                    { 3, null, false, 2, "Done" }
                });

            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Colour", "Name" },
                values: new object[,]
                {
                    { 1, "#FF6B6B", "High Priority" },
                    { 2, "#FFC93C", "Medium Priority" },
                    { 3, "#6BCB77", "Low Priority" },
                    { 4, "#FF4757", "Bug" },
                    { 5, "#4A90D9", "Feature" },
                    { 6, "#9B59B6", "Enhancement" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardLabels_LabelId",
                table: "CardLabels",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardColumnId",
                table: "Cards",
                column: "BoardColumnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardLabels");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "Columns");
        }
    }
}
