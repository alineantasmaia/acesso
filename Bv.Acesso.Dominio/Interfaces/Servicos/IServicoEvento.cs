using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bv.Acesso.Dominio.Dto;

namespace Bv.Acesso.Dominio.Interfaces.Servicos
{
    public interface IServicoEvento
    {
        Task Executar(Evento evento);
    }
}
