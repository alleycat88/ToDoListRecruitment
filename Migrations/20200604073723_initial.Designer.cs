﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoListRecruitment;

namespace ToDoListRecruitment.Migrations
{
    [DbContext(typeof(ApiDb))]
    [Migration("20200604073723_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ToDoListRecruitment.Models.List", b =>
                {
                    b.Property<long>("idList")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("colorHexList")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("nameList")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("statusList")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("idList");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("ToDoListRecruitment.Models.ListItem", b =>
                {
                    b.Property<long>("idListItem")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long?>("ListidList")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("descListItem")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long>("idList")
                        .HasColumnType("bigint");

                    b.Property<int>("isDoneListItem")
                        .HasColumnType("int");

                    b.Property<string>("nameListItem")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("updated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("idListItem");

                    b.HasIndex("ListidList");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("ToDoListRecruitment.Models.ListItem", b =>
                {
                    b.HasOne("ToDoListRecruitment.Models.List", null)
                        .WithMany("listItems")
                        .HasForeignKey("ListidList");
                });
#pragma warning restore 612, 618
        }
    }
}