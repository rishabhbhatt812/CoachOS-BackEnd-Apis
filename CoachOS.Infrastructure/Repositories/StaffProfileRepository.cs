using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Domain.Identity;
using CoachOS.Infrastructure.Data;

namespace CoachOS.Infrastructure.Repositories
{
    public class StaffProfileRepository : Repository<StaffProfile>, IStaffProfileRepository
    {
        public StaffProfileRepository(AppDbContext context) : base(context)
        {
        }
    }
}
