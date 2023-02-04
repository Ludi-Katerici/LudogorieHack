// <copyright file="InputInterestViewModel.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducateMe.Web.ViewModels.Administration.Interests;

public class InputInterestViewModel
{
    [Required(ErrorMessage = "Не сте посочили име на категорията")]
    [Display(Name = "Име")]
    public string Name { get; set; }

    public List<InterestTableViewModel> Interests { get; set; }
}
