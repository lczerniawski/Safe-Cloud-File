﻿// <auto-generated />
using System;
using ApiServer_Example.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiServer_Example.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191215185538_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApiServer_Example.Domains.Models.FileModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("ApiServer_Example.Domains.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ApiServer_Example.Domains.Models.User", b =>
                {
                    b.OwnsOne("ApiServer_Example.Domains.Models.RSAKeys", "RsaKeys", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte[]>("D")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("DP")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("DQ")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("Exponent")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("InverseQ")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("Modulus")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("P")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("Q")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
