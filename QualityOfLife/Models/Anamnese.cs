using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Models
{
    public class Anamnese : Entidades
    {
        public DateTime DataAnamnese { get; set; }
        public string MotivoEncaminhamento { get; set; }
        public string OutrosTratamentos { get; set; }
        public string Escola { get; set; }

        //Histórico pré natal, peri natal e pós natal.
        public string HistoricoPre { get; set; }
        public string HistoricoPeri { get; set; }
        public string HistoricoPos { get; set; }

        //História do desenvolvimento:Controle de cabeça:  Rolar:Arrastar: Sentar:Engatinhar: Andar:
        public string DesenvolvimentoCabeça { get; set; }
        public string DesenvolvimentoRolar { get; set; }
        public string DesenvolvimentoArrastar { get; set; }
        public string DesenvolvimentoSentar { get; set; }
        public string DesenvolvimentoEngatinhar { get; set; }
        public string DesenvolvimentoAndar { get; set; }

        public string Cirurgias { get; set; }
        public string Medicamentos { get; set; }
        public string Rotina { get; set; }

        //Interação com a família, com outras crianças, outros ambientes:
        public string InteracaoFamilia { get; set; }
        public string InteracaoCrianças { get; set; }
        public string InteracaoAmbientes { get; set; }
        public string ReageDificuldades { get; set; }
        public string MetodosCorretivos { get; set; }

        //Fazer enums com esses tipos
        //Atividades da Vida Diária: (Independência, adaptações, postura, experiências sensoriais)  Brincar:Higiene: Vestuário: Alimentação: 
        public string AtividadesBrincar { get; set; }
        public string AtividadesHigiene { get; set; }
        public string AtividadesVestuario { get; set; }
        public string AtividadesAlimentação { get; set; }
        public string HabilidadeMotoraGrossa { get; set; }
        public string HabilidadeMotoraFina { get; set; }
        public string SistemasSensoriais { get; set; }
        public string AspectosPerceptoCognitivos { get; set; }
        public string Linguagem { get; set; }
        public string TOAjudar { get; set; }


        public Paciente Paciente { get; set; }
        public Profissional Profissional { get; set; }

        public Anamnese()
        {
        }

        public Anamnese(DateTime dataAnamnese, string motivoEncaminhamento, string outrosTratamentos, string escola, string historicoPre, string historicoPeri, string historicoPos, string desenvolvimentoCabeça, string desenvolvimentoRolar, string desenvolvimentoArrastar, string desenvolvimentoSentar, string desenvolvimentoEngatinhar, string desenvolvimentoAndar, string cirurgias, string medicamentos, string rotina, string interacaoFamilia, string interacaoCrianças, string interacaoAmbientes, string reageDificuldades, string metodosCorretivos, string atividadesBrincar, string atividadesHigiene, string atividadesVestuario, string atividadesAlimentação, string habilidadeMotoraGrossa, string habilidadeMotoraFina, string sistemasSensoriais, string aspectosPerceptoCognitivos, string linguagem, string tOAjudar, Paciente paciente, Profissional profissional)
        {
            DataAnamnese = dataAnamnese;
            MotivoEncaminhamento = motivoEncaminhamento;
            OutrosTratamentos = outrosTratamentos;
            Escola = escola;
            HistoricoPre = historicoPre;
            HistoricoPeri = historicoPeri;
            HistoricoPos = historicoPos;
            DesenvolvimentoCabeça = desenvolvimentoCabeça;
            DesenvolvimentoRolar = desenvolvimentoRolar;
            DesenvolvimentoArrastar = desenvolvimentoArrastar;
            DesenvolvimentoSentar = desenvolvimentoSentar;
            DesenvolvimentoEngatinhar = desenvolvimentoEngatinhar;
            DesenvolvimentoAndar = desenvolvimentoAndar;
            Cirurgias = cirurgias;
            Medicamentos = medicamentos;
            Rotina = rotina;
            InteracaoFamilia = interacaoFamilia;
            InteracaoCrianças = interacaoCrianças;
            InteracaoAmbientes = interacaoAmbientes;
            ReageDificuldades = reageDificuldades;
            MetodosCorretivos = metodosCorretivos;
            AtividadesBrincar = atividadesBrincar;
            AtividadesHigiene = atividadesHigiene;
            AtividadesVestuario = atividadesVestuario;
            AtividadesAlimentação = atividadesAlimentação;
            HabilidadeMotoraGrossa = habilidadeMotoraGrossa;
            HabilidadeMotoraFina = habilidadeMotoraFina;
            SistemasSensoriais = sistemasSensoriais;
            AspectosPerceptoCognitivos = aspectosPerceptoCognitivos;
            Linguagem = linguagem;
            TOAjudar = tOAjudar;
            Paciente = paciente;
            Profissional = profissional;
        }
    }
}
