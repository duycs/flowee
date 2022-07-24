﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillInfrastructure.DataAccess;

#nullable disable

namespace SkillInfrastructure.DataAccess.Migrations
{
    [DbContext(typeof(SkillContext))]
    [Migration("20220723084222_UpdateResultCodeType")]
    partial class UpdateResultCodeType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("DateDeleted")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.MatrixSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("DateDeleted")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EstimationTimeInMiniSecond")
                        .HasColumnType("int");

                    b.Property<int>("ResultId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("SpecificationSkillLevelId")
                        .HasColumnType("int");

                    b.Property<int>("WorkerSkillLevelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("ResultId");

                    b.HasIndex("SkillId");

                    b.HasIndex("SpecificationSkillLevelId");

                    b.HasIndex("WorkerSkillLevelId");

                    b.ToTable("MatrixSkills");
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("DateDeleted")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("DateDeleted")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.SpecificationSkillLevel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("SpecificationSkillLevels", (string)null);
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.WorkerSkillLevel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.ToTable("WorkerSkillLevels", (string)null);
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.MatrixSkill", b =>
                {
                    b.HasOne("SkillDomain.AgreegateModels.SkillAgreegate.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkillDomain.AgreegateModels.SkillAgreegate.Result", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkillDomain.AgreegateModels.SkillAgreegate.Skill", "Skill")
                        .WithMany("MatrixSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkillDomain.AgreegateModels.SkillAgreegate.SpecificationSkillLevel", "SpecificationSkillLevel")
                        .WithMany()
                        .HasForeignKey("SpecificationSkillLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkillDomain.AgreegateModels.SkillAgreegate.WorkerSkillLevel", "WorkerSkillLevel")
                        .WithMany()
                        .HasForeignKey("WorkerSkillLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("Result");

                    b.Navigation("Skill");

                    b.Navigation("SpecificationSkillLevel");

                    b.Navigation("WorkerSkillLevel");
                });

            modelBuilder.Entity("SkillDomain.AgreegateModels.SkillAgreegate.Skill", b =>
                {
                    b.Navigation("MatrixSkills");
                });
#pragma warning restore 612, 618
        }
    }
}