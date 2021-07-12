using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Responsavel : Entidades
    {
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [DisplayName("Profissão")]
        public string Profissao { get; set; }

        //Dados Contato
        public string Email { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Telefone3 { get; set; }

        //Dados Endereço
        public string Cep { get; set; }
        public string Rua { get; set; }
        [DisplayName("Número")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

        public Responsavel()
        {
        }

        public Responsavel(string nome, string cpf, DateTime dataNascimento, string profissao, string email, string telefone1, string telefone2, string telefone3, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Profissao = profissao;
            Email = email;
            Telefone1 = telefone1;
            Telefone2 = telefone2;
            Telefone3 = telefone3;
            Cep = cep;
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }
    }
}
