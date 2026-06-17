using System.Data;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
