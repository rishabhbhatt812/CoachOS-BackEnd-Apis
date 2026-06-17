using CoachOS.Application.Interfaces.Repositories;
using CoachOS.Domain.Identity;
using CoachOS.Infrastructure.Data;

namespace CoachOS.Infrastructure.Repositories
{
    public class TeacherProfileRepository : Repository<TeacherProfile>, ITeacherProfileRepository
    {
        public TeacherProfileRepository(AppDbContext context) : base(context)
        {
        }
    }
}
