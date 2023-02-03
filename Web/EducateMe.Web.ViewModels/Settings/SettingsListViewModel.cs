using System.Collections.Generic;

namespace EducateMe.Web.ViewModels.Settings;

public class SettingsListViewModel
{
    public IEnumerable<SettingViewModel> Settings { get; set; }
}
