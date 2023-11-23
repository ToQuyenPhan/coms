using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Common.Intefaces.Persistence
{
    public interface IActionHistoryRepository
    {
        Task<ActionHistory> GetActionHistoryById(int id);
        Task AddActionHistory(ActionHistory actionHistory);
    }
}
