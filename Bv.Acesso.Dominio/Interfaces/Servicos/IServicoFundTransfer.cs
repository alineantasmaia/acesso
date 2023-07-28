using Bv.Acesso.Dominio.Dto;

namespace Bv.Acesso.Dominio.Interfaces.Servicos
{
    public interface IServicoFundTransfer
    {
        Task Executar(FundTransferDto fundTransfer);
    }
}
