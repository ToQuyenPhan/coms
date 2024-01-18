using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface ILiquidationRecordFieldRepository
    {
        Task AddRangeLiquidationRecordField(List<LiquidationRecordField> liquidationRecordFields);
        Task<IList<LiquidationRecordField>?> GetByLiquidationRecordId(int liquidationRecordId);
    }
}
