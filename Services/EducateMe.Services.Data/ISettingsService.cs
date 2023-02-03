using System.Collections.Generic;

namespace EducateMe.Services.Data;

public interface ISettingsService
{
    int GetCount();

    IEnumerable<T> GetAll<T>();
}
