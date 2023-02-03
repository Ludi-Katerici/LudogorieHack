using EducateMe.Common;
using EducateMe.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducateMe.Web.Areas.Administration.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
[Area("Administration")]
public class AdministrationController : BaseController
{
}
