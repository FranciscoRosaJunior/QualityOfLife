using QualityOfLife.Interfaces.IRepositories.IFinanceiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QualityOfLife.Data;
using QualityOfLife.Models;
using QualityOfLife.Models.Enums;
using QualityOfLife.Models.ViewModels;
using System.Drawing;
using System.IO;

namespace QualityOfLife.Services
{
    public class FinanceiroService : IFinanceiroService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRelatorioServices _relatorioServices;

        public FinanceiroService(ApplicationDbContext context, IRelatorioServices relatorioServices)
        {
            _context = context;
            _relatorioServices = relatorioServices;
        }

        public async Task<byte[]> GeraFaturamento(string mesRef)
        {
            var model = await BuscaFaturamento(mesRef);

            return _relatorioServices.RelatorioFaturamento(model, mesRef);
        }

        

        public async Task<ICollection<FaturamentoViewModels>> BuscaFaturamento(string mesRef)
        {
            ICollection<FaturamentoViewModels> model = new List<FaturamentoViewModels>();

            var pacientesList = await _context.Paciente
                .Where(x => x.StatusPacientes.ToString() == "3" || x.StatusPacientes.ToString() == "2")
                .Include(x => x.Responsavel)
                .ToListAsync();

            foreach (var paciente in pacientesList)
            {
                ICollection<Paciente> pacientes = new List<Paciente>();
                pacientes.Add(paciente);

                var pedidoList = await _context.Pedido
                    .Where(x => x.Paciente.Id == paciente.Id)
                    .Where(x => x.DataPagamento.ToString("yyyy-MM") == mesRef)
                    .Where(x => x.Pagamento == true)
                    .ToListAsync();

                if (pedidoList.Count > 0)
                {
                    var AgendaPedidos = new List<AgendaPedido>();
                    foreach (var pedido in pedidoList)
                    {
                        AgendaPedidos = await _context.AgendaPedidos
                        .Where(x => x.Pedido.Id == pedido.Id)
                        .Include(x => x.Agenda)
                        .ToListAsync();

                        var agenda = new List<Agenda>();
                        agenda = AgendaPedidos.Select(x => x.Agenda).ToList();

                        model.Add(new FaturamentoViewModels()
                        {
                            Paciente = paciente,
                            Pedido = pedido,
                            Agendas = agenda,
                        });
                    }
                }
            }
            return model;
        }

    }
}