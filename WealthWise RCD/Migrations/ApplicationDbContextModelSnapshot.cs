﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WealthWise_RCD.Models;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WealthWise_RCD.Models.Test", b =>
                {
                    b.Property<int>("testId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("testId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("WealthWise_RCD.Models.testtest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("testtests");
                });
#pragma warning restore 612, 618
        }
    }
}
