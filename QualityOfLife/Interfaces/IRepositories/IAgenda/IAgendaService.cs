using Microsoft.AspNetCore.Mvc;
using QualityOfLife.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualityOfLife.Interfaces.IRepositories.IAgenda
{
    public interface IAgendaService
    {
        List<DateTime> BuscaDatas(int repetir, DateTime dataHora);
        Task<List<DateTime>> BuscaDatasPorDia(AgendaEmLote agendaEmLote, Paciente dadosPaciente, List<string> listaFeriados);

        Task<List<Agenda>> BuscaPresencaAsync(string paciente, string dataInicio, string dataFim);
        string ConvertDiaParaIngles(string v);
    }
}