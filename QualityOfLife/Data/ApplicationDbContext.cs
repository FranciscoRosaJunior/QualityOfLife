using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualityOfLife.Models;

namespace QualityOfLife.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProfissionalPaciente> ProfissionalPacientes { get; set; }
        public DbSet<ProfissionalPedido> ProfissionalPedidos { get; set; }
        public DbSet<AgendaPedido> AgendaPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProfissionalPaciente>();
            modelBuilder.Entity<ProfissionalPedido>();
        }
        
        public DbSet<QualityOfLife.Models.Responsavel> Responsavel { get; set; }
        
        public DbSet<QualityOfLife.Models.Paciente> Paciente { get; set; }
        
        public DbSet<QualityOfLife.Models.Agenda> Agenda { get; set; }
        
        public DbSet<QualityOfLife.Models.Profissional> Profissional { get; set; }
        
        public DbSet<QualityOfLife.Models.Pedido> Pedido { get; set; }
        
        public DbSet<QualityOfLife.Models.ContasApagar> ContasApagar { get; set; }
        
        public DbSet<QualityOfLife.Models.Recibo> Recibo { get; set; }
    }
}
