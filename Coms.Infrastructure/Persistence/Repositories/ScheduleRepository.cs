using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using Coms.Domain.Enum;

namespace Coms.Infrastructure.Persistence.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IGenericRepository<Schedule> _genericRepository;

        public ScheduleRepository(IGenericRepository<Schedule> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task Add(Schedule schedule)
        {
            await _genericRepository.CreateAsync(schedule);
        }

        public async Task<IList<Schedule>?> GetByUSerId(int userId)
        {
            var list = await _genericRepository.WhereAsync(s => s.UserId.Equals(userId) && 
                s.Status.Equals(ScheduleStatus.Active), null);
            return (list.Count() > 0) ? list : null;
        }

        public async Task Update(Schedule schedule)
        {
            await _genericRepository.UpdateAsync(schedule);
        }

        public async Task<Schedule?> GetSchedule(int id)
        {
            return await _genericRepository.FirstOrDefaultAsync(s => s.Id.Equals(id), null);
        }
    }
}
