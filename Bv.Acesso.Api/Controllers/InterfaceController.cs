using Microsoft.AspNetCore.Mvc;
using Bv.Acesso.Dominio;
using Bv.Acesso.Dominio.Dto;
using Bv.Acesso.Dominio.Interfaces.Servicos;

namespace Bv.Acesso.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    public class InterfaceController : Controller
    {
        private readonly IServicoFundTransfer _servicoFundTransfer;
        public InterfaceController(IServicoFundTransfer servicoFundTransfer) 
        {
            _servicoFundTransfer = servicoFundTransfer;
        }

        [HttpPost("fund-transfer")]
        public async Task<IActionResult> FundTransfer(FundTransferDto fundTransfer)
        {
            try
            {
                await _servicoFundTransfer.Executar(fundTransfer);
                return Ok("Tranferencia realizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
