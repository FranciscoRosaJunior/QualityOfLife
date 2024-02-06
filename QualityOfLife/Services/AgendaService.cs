using QualityOfLife.Data;
using QualityOfLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityOfLife.Interfaces.IRepositories.IAgenda;
using Microsoft.EntityFrameworkCore;

namespace QualityOfLife.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly ApplicationDbContext _context;

        public AgendaService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Agenda>> BuscaPresencaAsync(string paciente, string dataInicio, string dataFim)
        {
            var model = new List<Agenda>();
            if (paciente == null && dataInicio == null)
            {
                DateTime dataAtual = DateTime.Now;
                model = await _context.Agenda
                    .Where(x => x.DataHora.ToString("dd/MM/yyyy")
                    .Contains(dataAtual.ToString("dd/MM/yyyy"))).Include(x => x.Profissional)
                    .OrderBy(x => x.DataHora)
                    .ToListAsync();
            }
            else if (paciente != null && dataInicio == null)
            {
                model = await _context.Agenda
                    .Where(x => x.Paciente.Cpf == paciente)
                    .Include(x => x.Profissional)
                    .OrderBy(x => x.DataHora)
                    .ToListAsync();
            }
            else if (paciente == null && dataInicio != null)
            {
                DateTime DataInicio = DateTime.Parse(dataInicio);
                DateTime DataFim = DateTime.Parse(dataFim);
                model = await _context.Agenda
                    .Where(x => x.DataHora >= DataInicio)
                    .Where(x => x.DataHora <= DataFim)
                    .Include(x => x.Profissional)
                    .OrderBy(x => x.DataHora)
                    .ToListAsync();
            }
            else
            {
                DateTime DataInicio = DateTime.Parse(dataInicio);
                DateTime DataFim = DateTime.Parse(dataFim);
                model = await _context.Agenda.Where(x => x.Paciente.Cpf == paciente)
                    .Where(x => x.DataHora >= DataInicio)
                    .Where(x => x.DataHora <= DataFim)
                    .Include(x => x.Profissional)
                    .OrderBy(x => x.DataHora)
                    .ToListAsync();
            }
            return model;
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

        public async Task<List<DateTime>> BuscaDatasPorDia(AgendaEmLote agendaEmLote, Paciente dadosPaciente, List<string> listaFeriados)
        {
            List<DateTime> datas = new List<DateTime>();

           var diasAtendimento = await _context.PacienteDiaAtendimento
                .Where(x => x.Paciente.Id == dadosPaciente.Id)
                .ToListAsync();


            foreach (var item in diasAtendimento)
            {
                DateTime DiaUmMesSeguinte = new DateTime(agendaEmLote.MesReferencia.Year, agendaEmLote.MesReferencia.Month, DateTime.DaysInMonth(agendaEmLote.MesReferencia.Year, agendaEmLote.MesReferencia.Month)).AddDays(1);
                DateTime Dia = agendaEmLote.MesReferencia.AddDays(0);
                item.DiaDaSemana = ConvertDiaParaIngles(item.DiaDaSemana).ToLower();

                while (Dia < DiaUmMesSeguinte)
                {
                    string diaDaSemana = Dia.DayOfWeek.ToString().ToLower();
                    if (diaDaSemana.Contains(item.DiaDaSemana))
                    {
                        string data = Dia.ToString("dd/MM/yyyy");
                        String[] hora = item.Horario.ToString("HH:mm").Split(':');
                        Dia = Dia.Date.AddHours(Convert.ToDouble(hora[0]));
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
            return (datas.OrderBy(x => x.Date).ToList());
        }

        public string ConvertDiaParaIngles(string diaDaSemana)
        {
            if (diaDaSemana == "Segunda") return "Monday";
            if (diaDaSemana == "Terça") return "Tuesday";
            if (diaDaSemana == "Quarta") return "Wednesday";
            if (diaDaSemana == "Quinta") return "Thursday";
            if (diaDaSemana == "Sexta") return "Friday";
            if (diaDaSemana == "Sábado") return "Saturday";
            if (diaDaSemana == "Domingo") return "Sunday";

            else return diaDaSemana;
        }
    }
}
