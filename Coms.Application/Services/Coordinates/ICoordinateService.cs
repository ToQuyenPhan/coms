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
        IList<CoordianteResult> GetCoordinates(int contractId, string searchText);
    }
}
