using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bv.Acesso.Dominio.Dto
{
    public class FundTransferDto
    {
        public string Id { get; set; }
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public double Value { get; set; }
    }
}
