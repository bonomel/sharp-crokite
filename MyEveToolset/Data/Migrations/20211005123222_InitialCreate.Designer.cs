﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyEveToolset.Data;

namespace MyEveToolset.Data.Migrations
{
    [DbContext(typeof(SharpCrokiteDbContext))]
    [Migration("20211005123222_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("MyEveToolset.Data.Models.Harvestable", b =>
                {
                    b.Property<int>("HarvestableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IsCompressedVariantOfType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("HarvestableId");

                    b.ToTable("Harvestables");
                });

            modelBuilder.Entity("MyEveToolset.Data.Models.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("MaterialId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("MyEveToolset.Data.Models.MaterialContent", b =>
                {
                    b.Property<int>("MaterialContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HarvestableId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("MaterialContentId");

                    b.HasIndex("HarvestableId");

                    b.ToTable("MaterialContents");
                });

            modelBuilder.Entity("MyEveToolset.Data.Models.MaterialContent", b =>
                {
                    b.HasOne("MyEveToolset.Data.Models.Harvestable", null)
                        .WithMany("MaterialContents")
                        .HasForeignKey("HarvestableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyEveToolset.Data.Models.Harvestable", b =>
                {
                    b.Navigation("MaterialContents");
                });
#pragma warning restore 612, 618
        }
    }
}
