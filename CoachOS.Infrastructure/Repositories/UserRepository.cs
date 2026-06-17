using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Domain.Identity;
using CoachOS.Infrastructure.Data;

namespace CoachOS.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
