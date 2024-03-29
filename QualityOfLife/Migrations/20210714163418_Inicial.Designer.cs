﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QualityOfLife.Data;

namespace QualityOfLife.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210714163418_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("QualityOfLife.Models.Agenda", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Anotações");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataHora");

                    b.Property<bool>("Falta");

                    b.Property<bool>("FaltaJustificada");

                    b.Property<string>("Local");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<long?>("PacienteId");

                    b.Property<bool>("Presenca");

                    b.Property<long?>("ProfissionalId");

                    b.Property<bool>("Reagendar");

                    b.Property<int>("Repetir");

                    b.Property<int>("TipoAtendimento");

                    b.Property<string>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("Agenda");
                });

            modelBuilder.Entity("QualityOfLife.Models.Anamnese", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AspectosPerceptoCognitivos");

                    b.Property<string>("AtividadesAlimentação");

                    b.Property<string>("AtividadesBrincar");

                    b.Property<string>("AtividadesHigiene");

                    b.Property<string>("AtividadesVestuario");

                    b.Property<string>("Cirurgias");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataAnamnese");

                    b.Property<string>("DesenvolvimentoAndar");

                    b.Property<string>("DesenvolvimentoArrastar");

                    b.Property<string>("DesenvolvimentoCabeça");

                    b.Property<string>("DesenvolvimentoEngatinhar");

                    b.Property<string>("DesenvolvimentoRolar");

                    b.Property<string>("DesenvolvimentoSentar");

                    b.Property<string>("Escola");

                    b.Property<string>("HabilidadeMotoraFina");

                    b.Property<string>("HabilidadeMotoraGrossa");

                    b.Property<string>("HistoricoPeri");

                    b.Property<string>("HistoricoPos");

                    b.Property<string>("HistoricoPre");

                    b.Property<string>("InteracaoAmbientes");

                    b.Property<string>("InteracaoCrianças");

                    b.Property<string>("InteracaoFamilia");

                    b.Property<string>("Linguagem");

                    b.Property<string>("Medicamentos");

                    b.Property<string>("MetodosCorretivos");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("MotivoEncaminhamento");

                    b.Property<string>("OutrosTratamentos");

                    b.Property<long?>("PacienteId");

                    b.Property<long?>("ProfissionalId");

                    b.Property<string>("ReageDificuldades");

                    b.Property<string>("Rotina");

                    b.Property<string>("SistemasSensoriais");

                    b.Property<string>("TOAjudar");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("Anamnese");
                });

            modelBuilder.Entity("QualityOfLife.Models.Boleto", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Banco");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataVencimento");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Numero");

                    b.Property<long?>("PacienteResponsavelId");

                    b.Property<double>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("PacienteResponsavelId");

                    b.ToTable("Boleto");
                });

            modelBuilder.Entity("QualityOfLife.Models.Paciente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cpf");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Nome");

                    b.Property<long?>("ResponsavelId");

                    b.Property<int>("StatusPacientes");

                    b.HasKey("Id");

                    b.HasIndex("ResponsavelId");

                    b.ToTable("Paciente");
                });

            modelBuilder.Entity("QualityOfLife.Models.Pedido", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Credito");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataPedido");

                    b.Property<double>("Desconto");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Observacoes");

                    b.Property<long?>("PacienteId");

                    b.Property<long?>("ProfissionalId");

                    b.Property<int>("TipoAtendimento");

                    b.Property<double>("Total");

                    b.Property<double>("Valor");

                    b.Property<string>("mesreferencia");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("QualityOfLife.Models.Profissional", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("Cep");

                    b.Property<string>("Cidade");

                    b.Property<string>("Complemento");

                    b.Property<string>("Cpf");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Email");

                    b.Property<string>("Estado");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Nome");

                    b.Property<string>("Numero");

                    b.Property<string>("Profissao");

                    b.Property<string>("Rua");

                    b.Property<string>("Telefone1");

                    b.Property<string>("Telefone2");

                    b.Property<string>("Telefone3");

                    b.HasKey("Id");

                    b.ToTable("Profissional");
                });

            modelBuilder.Entity("QualityOfLife.Models.ProfissionalPaciente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("PacienteId");

                    b.Property<long?>("ProfissionalId");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("ProfissionalPacientes");
                });

            modelBuilder.Entity("QualityOfLife.Models.ProfissionalPedido", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("PedidoId");

                    b.Property<long?>("ProfissionalId");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("ProfissionalPedidos");
                });

            modelBuilder.Entity("QualityOfLife.Models.Relatorio", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataRelatorio");

                    b.Property<string>("Link");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<long?>("PacienteId");

                    b.Property<long?>("ProfissionalId");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("ProfissionalId");

                    b.ToTable("Relatorio");
                });

            modelBuilder.Entity("QualityOfLife.Models.Responsavel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("Cep");

                    b.Property<string>("Cidade");

                    b.Property<string>("Complemento");

                    b.Property<string>("Cpf");

                    b.Property<string>("Criado");

                    b.Property<DateTime>("CriadoData")
                        .HasColumnType("DATETIME");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Email");

                    b.Property<string>("Estado");

                    b.Property<string>("Modificado");

                    b.Property<DateTime>("ModificadoData")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Nome");

                    b.Property<string>("Numero");

                    b.Property<string>("Profissao");

                    b.Property<string>("Rua");

                    b.Property<string>("Telefone1");

                    b.Property<string>("Telefone2");

                    b.Property<string>("Telefone3");

                    b.Property<string>("TipoLogradouro");

                    b.HasKey("Id");

                    b.ToTable("Responsavel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QualityOfLife.Models.Agenda", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Paciente")
                        .WithMany("Agendas")
                        .HasForeignKey("PacienteId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany("Agendas")
                        .HasForeignKey("ProfissionalId");
                });

            modelBuilder.Entity("QualityOfLife.Models.Anamnese", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Paciente")
                        .WithMany("Anamneses")
                        .HasForeignKey("PacienteId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany("Anamneses")
                        .HasForeignKey("ProfissionalId");
                });

            modelBuilder.Entity("QualityOfLife.Models.Boleto", b =>
                {
                    b.HasOne("QualityOfLife.Models.Responsavel", "PacienteResponsavel")
                        .WithMany("Boletos")
                        .HasForeignKey("PacienteResponsavelId");
                });

            modelBuilder.Entity("QualityOfLife.Models.Paciente", b =>
                {
                    b.HasOne("QualityOfLife.Models.Responsavel", "Responsavel")
                        .WithMany("Pacientes")
                        .HasForeignKey("ResponsavelId");
                });

            modelBuilder.Entity("QualityOfLife.Models.Pedido", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Paciente")
                        .WithMany("Pedidos")
                        .HasForeignKey("PacienteId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany("Pedidos")
                        .HasForeignKey("ProfissionalId");
                });

            modelBuilder.Entity("QualityOfLife.Models.ProfissionalPaciente", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Paciente")
                        .WithMany("ProfissionalPacientes")
                        .HasForeignKey("PacienteId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany("ProfissionalPacientes")
                        .HasForeignKey("ProfissionalId");
                });

            modelBuilder.Entity("QualityOfLife.Models.ProfissionalPedido", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany()
                        .HasForeignKey("ProfissionalId");
                });

            modelBuilder.Entity("QualityOfLife.Models.Relatorio", b =>
                {
                    b.HasOne("QualityOfLife.Models.Paciente", "Paciente")
                        .WithMany("Relatorios")
                        .HasForeignKey("PacienteId");

                    b.HasOne("QualityOfLife.Models.Profissional", "Profissional")
                        .WithMany("Relatorios")
                        .HasForeignKey("ProfissionalId");
                });
#pragma warning restore 612, 618
        }
    }
}
