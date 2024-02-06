using Microsoft.AspNetCore.Mvc;
using QualityOfLife.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QualityOfLife.Interfaces.IRepositories.IFinanceiro
{
    public interface IFinanceiroService
    {
        Task<byte[]> GeraFaturamento(string mesRef);

        Task<ICollection<FaturamentoViewModels>> BuscaFaturamento(string mesRef);

    }
}
