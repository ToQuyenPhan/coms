using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.Coordinates
{
    public interface ICoordinateService
    {
        IList<CoordianteResult> GetCoordinatesContract(int contractId, string searchText);
        IList<CoordianteResult> GetCoordinatesContractAnnex(int contractAnnexId, string searchText);
    }
}
