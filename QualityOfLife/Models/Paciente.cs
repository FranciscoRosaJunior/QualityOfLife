using ConsultorioTO.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Paciente : Entidades
    {
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("Data Nascimento")]
        public DateTime DataNascimento { get; set; }

        //Um paciente pode ter apenas um
        [DisplayName("Responsável")]
        public Responsavel Responsavel { get; set; }

        [DisplayName("Status")]
        public StatusPaciente StatusPacientes { get; set; }

        [DisplayName("Dia do Atendimento")]
        public string DiaAtendimento { get; set; }
        //Fim apenas um

        //Um paciente pode ter varios 
        public ICollection<ProfissionalPaciente> ProfissionalPacientes { get; set; } = new List<ProfissionalPaciente>();
        public ICollection<Anamnese> Anamneses { get; set; } = new List<Anamnese>();
        public ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();

        public Paciente()
        {
        }

        public Paciente(string nome, string cpf, DateTime dataNascimento, StatusPaciente statusPaciente, Responsavel responsavel)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            StatusPacientes = statusPaciente;
            Responsavel = responsavel;
        }
    }
}
