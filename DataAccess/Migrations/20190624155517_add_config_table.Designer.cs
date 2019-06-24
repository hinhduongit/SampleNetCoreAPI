﻿// <auto-generated />
using System;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(SampleNetCoreAPIContext))]
    [Migration("20190624155517_add_config_table")]
    partial class add_config_table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccess.Model.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("blogs");
                });

            modelBuilder.Entity("DataAccess.Model.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("BlogId")
                        .HasName("FK_Post_Blog_BlogId_idx");

                    b.ToTable("posts");
                });

            modelBuilder.Entity("DataAccess.Model.SampleNetCoreConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<string>("Key");

                    b.Property<DateTime?>("UpdatedTime");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("sample_net_core_config");
                });

            modelBuilder.Entity("DataAccess.Model.Post", b =>
                {
                    b.HasOne("DataAccess.Model.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .HasConstraintName("FK_Post_Blog_BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
