using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WfPrimeTracker.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimeItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WikiUrl = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimeItems_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PrimeParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimeParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimeParts_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Relics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Tier = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WikiUrl = table.Column<string>(nullable: true),
                    IsVaulted = table.Column<bool>(nullable: false),
                    ImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relics_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "IngredientsGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PrimeItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientsGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientsGroup_PrimeItems_PrimeItemId",
                        column: x => x.PrimeItemId,
                        principalTable: "PrimeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimePartIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PrimeItemId = table.Column<int>(nullable: false),
                    PrimePartId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimePartIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimePartIngredient_PrimeItems_PrimeItemId",
                        column: x => x.PrimeItemId,
                        principalTable: "PrimeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimePartIngredient_PrimeParts_PrimePartId",
                        column: x => x.PrimePartId,
                        principalTable: "PrimeParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceIngredient",
                columns: table => new
                {
                    ResourceId = table.Column<int>(nullable: false),
                    IngredientsGroupId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceIngredient", x => new { x.IngredientsGroupId, x.ResourceId });
                    table.ForeignKey(
                        name: "FK_ResourceIngredient_IngredientsGroup_IngredientsGroupId",
                        column: x => x.IngredientsGroupId,
                        principalTable: "IngredientsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceIngredient_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelicDrop",
                columns: table => new
                {
                    RelicId = table.Column<int>(nullable: false),
                    PrimePartIngredientId = table.Column<int>(nullable: false),
                    DropChance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelicDrop", x => new { x.RelicId, x.PrimePartIngredientId });
                    table.ForeignKey(
                        name: "FK_RelicDrop_PrimePartIngredient_PrimePartIngredientId",
                        column: x => x.PrimePartIngredientId,
                        principalTable: "PrimePartIngredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelicDrop_Relics_RelicId",
                        column: x => x.RelicId,
                        principalTable: "Relics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientsGroup_PrimeItemId",
                table: "IngredientsGroup",
                column: "PrimeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimeItems_ImageId",
                table: "PrimeItems",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimePartIngredient_PrimeItemId",
                table: "PrimePartIngredient",
                column: "PrimeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimePartIngredient_PrimePartId",
                table: "PrimePartIngredient",
                column: "PrimePartId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimeParts_ImageId",
                table: "PrimeParts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_RelicDrop_PrimePartIngredientId",
                table: "RelicDrop",
                column: "PrimePartIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Relics_ImageId",
                table: "Relics",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceIngredient_ResourceId",
                table: "ResourceIngredient",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ImageId",
                table: "Resources",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelicDrop");

            migrationBuilder.DropTable(
                name: "ResourceIngredient");

            migrationBuilder.DropTable(
                name: "PrimePartIngredient");

            migrationBuilder.DropTable(
                name: "Relics");

            migrationBuilder.DropTable(
                name: "IngredientsGroup");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "PrimeParts");

            migrationBuilder.DropTable(
                name: "PrimeItems");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
