using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityOfLife.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profissional",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Profissao = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone1 = table.Column<string>(nullable: true),
                    Telefone2 = table.Column<string>(nullable: true),
                    Telefone3 = table.Column<string>(nullable: true),
                    Cep = table.Column<string>(nullable: true),
                    Rua = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Cidade = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profissional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Responsavel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Profissao = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone1 = table.Column<string>(nullable: true),
                    Telefone2 = table.Column<string>(nullable: true),
                    Telefone3 = table.Column<string>(nullable: true),
                    Cep = table.Column<string>(nullable: true),
                    Rua = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Cidade = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsavel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boleto",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Banco = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    DataVencimento = table.Column<DateTime>(nullable: false),
                    Valor = table.Column<double>(nullable: false),
                    PacienteResponsavelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boleto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boleto_Responsavel_PacienteResponsavelId",
                        column: x => x.PacienteResponsavelId,
                        principalTable: "Responsavel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    ResponsavelId = table.Column<long>(nullable: true),
                    StatusPacientes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paciente_Responsavel_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "Responsavel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agenda",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    PacienteId = table.Column<long>(nullable: true),
                    ProfissionalId = table.Column<long>(nullable: true),
                    DataHora = table.Column<DateTime>(nullable: false),
                    Local = table.Column<string>(nullable: true),
                    TipoAtendimento = table.Column<int>(nullable: false),
                    Presenca = table.Column<bool>(nullable: false),
                    FaltaJustificada = table.Column<bool>(nullable: false),
                    Falta = table.Column<bool>(nullable: false),
                    Reagendar = table.Column<bool>(nullable: false),
                    Anotações = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agenda_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agenda_Profissional_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Anamnese",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DataAnamnese = table.Column<DateTime>(nullable: false),
                    MotivoEncaminhamento = table.Column<string>(nullable: true),
                    OutrosTratamentos = table.Column<string>(nullable: true),
                    Escola = table.Column<string>(nullable: true),
                    HistoricoPre = table.Column<string>(nullable: true),
                    HistoricoPeri = table.Column<string>(nullable: true),
                    HistoricoPos = table.Column<string>(nullable: true),
                    DesenvolvimentoCabeça = table.Column<string>(nullable: true),
                    DesenvolvimentoRolar = table.Column<string>(nullable: true),
                    DesenvolvimentoArrastar = table.Column<string>(nullable: true),
                    DesenvolvimentoSentar = table.Column<string>(nullable: true),
                    DesenvolvimentoEngatinhar = table.Column<string>(nullable: true),
                    DesenvolvimentoAndar = table.Column<string>(nullable: true),
                    Cirurgias = table.Column<string>(nullable: true),
                    Medicamentos = table.Column<string>(nullable: true),
                    Rotina = table.Column<string>(nullable: true),
                    InteracaoFamilia = table.Column<string>(nullable: true),
                    InteracaoCrianças = table.Column<string>(nullable: true),
                    InteracaoAmbientes = table.Column<string>(nullable: true),
                    ReageDificuldades = table.Column<string>(nullable: true),
                    MetodosCorretivos = table.Column<string>(nullable: true),
                    AtividadesBrincar = table.Column<string>(nullable: true),
                    AtividadesHigiene = table.Column<string>(nullable: true),
                    AtividadesVestuario = table.Column<string>(nullable: true),
                    AtividadesAlimentação = table.Column<string>(nullable: true),
                    HabilidadeMotoraGrossa = table.Column<string>(nullable: true),
                    HabilidadeMotoraFina = table.Column<string>(nullable: true),
                    SistemasSensoriais = table.Column<string>(nullable: true),
                    AspectosPerceptoCognitivos = table.Column<string>(nullable: true),
                    Linguagem = table.Column<string>(nullable: true),
                    TOAjudar = table.Column<string>(nullable: true),
                    PacienteId = table.Column<long>(nullable: true),
                    ProfissionalId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anamnese", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anamnese_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Anamnese_Profissional_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    TipoAtendimento = table.Column<int>(nullable: false),
                    DataPedido = table.Column<DateTime>(nullable: false),
                    DataPrevista = table.Column<DateTime>(nullable: false),
                    Valor = table.Column<double>(nullable: false),
                    Desconto = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    PacienteId = table.Column<long>(nullable: true),
                    NomeProfissional = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedido_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfissionalPacientes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PacienteId = table.Column<long>(nullable: true),
                    ProfissionalId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalPacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionalPacientes_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfissionalPacientes_Profissional_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Relatorio",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Criado = table.Column<string>(nullable: true),
                    CriadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Modificado = table.Column<string>(nullable: true),
                    ModificadoData = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DataRelatorio = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PacienteId = table.Column<long>(nullable: true),
                    ProfissionalId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatorio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relatorio_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relatorio_Profissional_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfissionalPedidos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PedidoId1 = table.Column<long>(nullable: true),
                    ProfissionalId = table.Column<long>(nullable: true),
                    PedidoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfissionalPedidos_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfissionalPedidos_Paciente_PedidoId1",
                        column: x => x.PedidoId1,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfissionalPedidos_Profissional_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_PacienteId",
                table: "Agenda",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_ProfissionalId",
                table: "Agenda",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Anamnese_PacienteId",
                table: "Anamnese",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Anamnese_ProfissionalId",
                table: "Anamnese",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boleto_PacienteResponsavelId",
                table: "Boleto",
                column: "PacienteResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ResponsavelId",
                table: "Paciente",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_PacienteId",
                table: "Pedido",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPacientes_PacienteId",
                table: "ProfissionalPacientes",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPacientes_ProfissionalId",
                table: "ProfissionalPacientes",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPedidos_PedidoId",
                table: "ProfissionalPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPedidos_PedidoId1",
                table: "ProfissionalPedidos",
                column: "PedidoId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalPedidos_ProfissionalId",
                table: "ProfissionalPedidos",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Relatorio_PacienteId",
                table: "Relatorio",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Relatorio_ProfissionalId",
                table: "Relatorio",
                column: "ProfissionalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agenda");

            migrationBuilder.DropTable(
                name: "Anamnese");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Boleto");

            migrationBuilder.DropTable(
                name: "ProfissionalPacientes");

            migrationBuilder.DropTable(
                name: "ProfissionalPedidos");

            migrationBuilder.DropTable(
                name: "Relatorio");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Profissional");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "Responsavel");
        }
    }
}
