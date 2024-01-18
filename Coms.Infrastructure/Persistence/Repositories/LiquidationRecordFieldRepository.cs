using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class LiquidationRecordFieldRepository : ILiquidationRecordFieldRepository
    {
        private readonly IGenericRepository<LiquidationRecordField> _genericRepository;
        public LiquidationRecordFieldRepository(IGenericRepository<LiquidationRecordField> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task AddRangeLiquidationRecordField(List<LiquidationRecordField> liquidationRecordFields)
        {
            await _genericRepository.CreateRangeAsync(liquidationRecordFields);
        }

        public async Task<IList<LiquidationRecordField>?> GetByLiquidationRecordId(int liquidationRecordId)
        {
            var list = await _genericRepository.WhereAsync(cf => cf.LiquidationRecordId.Equals(liquidationRecordId), null);
            return (list.Count() > 0) ? list : null;
        }
    }
}
