using QualityOfLife.Data;
using QualityOfLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Services
{
    public class AgendaService
    {
        private readonly ApplicationDbContext _context;

        public AgendaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DateTime> BuscaDatas(int repetir, DateTime dataHora)
        {
            var list = new List<Agenda>();
            List<DateTime> datas = new List<DateTime>();

            //DateTime com o último dia do mês
            DateTime DiaUmMesSeguinte = new DateTime(dataHora.Year, dataHora.Month, DateTime.DaysInMonth(dataHora.Year, dataHora.Month)).AddDays(1);

            //Adiciona a data enviada
            datas.Add(dataHora);

            if (repetir == 1)
            {
                //data enviada mais 1 dia
                DateTime diaMaisUm = dataHora.AddDays(1);

                while (diaMaisUm < DiaUmMesSeguinte)
                {
                    datas.Add(diaMaisUm);
                    diaMaisUm = diaMaisUm.AddDays(1);
                }

            }
            else if (repetir == 2)
            {
                //data enviada mais 7 dias
                DateTime diaMaisSete = dataHora.AddDays(7);

                while (diaMaisSete < DiaUmMesSeguinte)
                {
                    datas.Add(diaMaisSete);
                    diaMaisSete = diaMaisSete.AddDays(7);
                }
            }
            else if (repetir == 3)
            {
                //data enviada mais 1 mês
                DateTime diaMaisMes = dataHora.AddMonths(1);

                int ano = diaMaisMes.Year;

                while (diaMaisMes.Month <= 12 && diaMaisMes.Year == ano)
                {
                    datas.Add(diaMaisMes);
                    diaMaisMes = diaMaisMes.AddMonths(1);
                }
            }

            return datas;
        }

        internal List<DateTime> BuscaDatasPorDia(AgendaEmLote agendaEmLote, Paciente dadosPaciente, List<string> listaFeriados)
        {
            List<DateTime> datas = new List<DateTime>();
            string[] diasAtendimento = dadosPaciente.DiaAtendimento.Split(',');

            foreach (var item in diasAtendimento)
            {
                string diaAtendimento = item.Replace('.', ' ').Trim();
                DateTime DiaUmMesSeguinte = new DateTime(agendaEmLote.MesReferencia.Year, agendaEmLote.MesReferencia.Month, DateTime.DaysInMonth(agendaEmLote.MesReferencia.Year, agendaEmLote.MesReferencia.Month)).AddDays(1);

                DateTime Dia = agendaEmLote.MesReferencia.AddDays(0);

                while (Dia < DiaUmMesSeguinte)
                {
                    string diaDaSemana = Dia.ToString("dddd");
                    if (diaDaSemana.ToLower().Contains(diaAtendimento.ToLower()))
                    {
                        string data = Dia.ToString("dd/MM/yyyy");
                        if (!listaFeriados.Contains(data))
                        {
                            datas.Add(Dia);
                        }
                        Dia = Dia.AddDays(1);
                    }
                    else
                    {
                        Dia = Dia.AddDays(1);
                    }

                }

            }
            return(datas.OrderBy(x => x.TimeOfDay).ToList());
        }

    }
}
