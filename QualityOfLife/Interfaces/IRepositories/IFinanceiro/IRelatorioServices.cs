using QualityOfLife.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Interfaces.IRepositories.IFinanceiro
{
    public interface IRelatorioServices
    {
        byte[] RelatorioFaturamento(ICollection<FaturamentoViewModels> faturamentos, string mes);
    }
}
