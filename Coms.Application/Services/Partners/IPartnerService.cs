using Coms.Application.Services.ContractCategories;
using Coms.Application.Services.ContractCosts;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Partners
{
    public interface IPartnerService
    {
        ErrorOr<IList<PartnerResult>> GetActivePartners();
    }
}
