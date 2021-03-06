﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WfPrimeTracker.Data;

namespace WfPrimeTracker.Data.Migrations
{
    [DbContext(typeof(PrimeContext))]
    [Migration("20190418184313_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WfPrimeTracker.Domain.Image", b =>
                {
                    b.Property<int>("Id");

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.IngredientsGroup", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.Property<int>("PrimeItemId");

                    b.HasKey("Id");

                    b.HasIndex("PrimeItemId");

                    b.ToTable("IngredientsGroup");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimeItem", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name");

                    b.Property<string>("WikiUrl");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("PrimeItems");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimePart", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("PrimeParts");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimePartIngredient", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Count");

                    b.Property<int>("PrimeItemId");

                    b.Property<int>("PrimePartId");

                    b.HasKey("Id");

                    b.HasIndex("PrimeItemId");

                    b.HasIndex("PrimePartId");

                    b.ToTable("PrimePartIngredient");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.Relic", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("ImageId");

                    b.Property<bool>("IsVaulted");

                    b.Property<string>("Name");

                    b.Property<int>("Tier");

                    b.Property<string>("WikiUrl");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Relics");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.RelicDrop", b =>
                {
                    b.Property<int>("RelicId");

                    b.Property<int>("PrimePartIngredientId");

                    b.Property<int>("DropChance");

                    b.HasKey("RelicId", "PrimePartIngredientId");

                    b.HasIndex("PrimePartIngredientId");

                    b.ToTable("RelicDrop");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.Resource", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.ResourceIngredient", b =>
                {
                    b.Property<int>("IngredientsGroupId");

                    b.Property<int>("ResourceId");

                    b.Property<int>("Count");

                    b.HasKey("IngredientsGroupId", "ResourceId");

                    b.HasIndex("ResourceId");

                    b.ToTable("ResourceIngredient");
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.IngredientsGroup", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.PrimeItem", "PrimeItem")
                        .WithMany("IngredientsGroups")
                        .HasForeignKey("PrimeItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimeItem", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.Image", "Image")
                        .WithMany("PrimeItem")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimePart", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.Image", "Image")
                        .WithMany("PrimePart")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.PrimePartIngredient", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.PrimeItem", "PrimeItem")
                        .WithMany("PrimePartIngredients")
                        .HasForeignKey("PrimeItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WfPrimeTracker.Domain.PrimePart", "PrimePart")
                        .WithMany("PrimePartIngredients")
                        .HasForeignKey("PrimePartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.Relic", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.Image", "Image")
                        .WithMany("Relic")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.RelicDrop", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.PrimePartIngredient", "PrimePartIngredient")
                        .WithMany("RelicDrops")
                        .HasForeignKey("PrimePartIngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WfPrimeTracker.Domain.Relic", "Relic")
                        .WithMany("RelicDrops")
                        .HasForeignKey("RelicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.Resource", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.Image", "Image")
                        .WithMany("Resource")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WfPrimeTracker.Domain.ResourceIngredient", b =>
                {
                    b.HasOne("WfPrimeTracker.Domain.IngredientsGroup", "IngredientsGroup")
                        .WithMany("ResourceIngredients")
                        .HasForeignKey("IngredientsGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WfPrimeTracker.Domain.Resource", "Resource")
                        .WithMany("ResourceIngredients")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
