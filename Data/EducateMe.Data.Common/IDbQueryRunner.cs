using System;
using System.Threading.Tasks;

namespace EducateMe.Data.Common;

public interface IDbQueryRunner : IDisposable
{
    Task RunQueryAsync(string query, params object[] parameters);
}
