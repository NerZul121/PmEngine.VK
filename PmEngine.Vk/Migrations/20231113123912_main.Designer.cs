﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PmEngine.Vk;

#nullable disable

namespace PmEngine.Vk.Migrations
{
    [DbContext(typeof(VkContext))]
    [Migration("20231113123912_main")]
    partial class main
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PmEngine.Core.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("LastOnlineDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("PmEngine.Vk.Entities.VkDataUserEntity", b =>
                {
                    b.Property<long>("VkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("VkId"));

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("VkId");

                    b.HasIndex("OwnerId");

                    b.ToTable("VkDataUserEntity");
                });

            modelBuilder.Entity("PmEngine.Vk.Entities.VkDataUserEntity", b =>
                {
                    b.HasOne("PmEngine.Core.Entities.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });
#pragma warning restore 612, 618
        }
    }
}