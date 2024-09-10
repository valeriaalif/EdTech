using System.Data;

namespace EdTech.Interfaces
{
    public interface IConnectionProvider
    {
        IDbConnection GetConnection();
    }
}
