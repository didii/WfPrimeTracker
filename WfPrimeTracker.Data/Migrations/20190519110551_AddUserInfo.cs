using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WfPrimeTracker.Data.Migrations
{
    public partial class AddUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnonymousId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPrimeItemSaveData",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    PrimeItemId = table.Column<int>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrimeItemSaveData", x => new { x.UserId, x.PrimeItemId });
                    table.ForeignKey(
                        name: "FK_UserPrimeItemSaveData_PrimeItems_PrimeItemId",
                        column: x => x.PrimeItemId,
                        principalTable: "PrimeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrimeItemSaveData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrimePartIngredientSaveData",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    PrimePartIngredientId = table.Column<int>(nullable: false),
                    CheckedCount = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    PrimePartId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrimePartIngredientSaveData", x => new { x.UserId, x.PrimePartIngredientId });
                    table.ForeignKey(
                        name: "FK_UserPrimePartIngredientSaveData_PrimeParts_PrimePartId",
                        column: x => x.PrimePartId,
                        principalTable: "PrimeParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPrimePartIngredientSaveData_PrimePartIngredient_PrimePartIngredientId",
                        column: x => x.PrimePartIngredientId,
                        principalTable: "PrimePartIngredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrimePartIngredientSaveData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPrimeItemSaveData_PrimeItemId",
                table: "UserPrimeItemSaveData",
                column: "PrimeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrimePartIngredientSaveData_PrimePartId",
                table: "UserPrimePartIngredientSaveData",
                column: "PrimePartId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrimePartIngredientSaveData_PrimePartIngredientId",
                table: "UserPrimePartIngredientSaveData",
                column: "PrimePartIngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPrimeItemSaveData");

            migrationBuilder.DropTable(
                name: "UserPrimePartIngredientSaveData");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
