// <copyright file="InputEventViewModel.cs" company="AspNetCoreTemplate">
// Copyright (c) AspNetCoreTemplate. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EducateMe.Web.Infrastructure.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EducateMe.Web.ViewModels.Events;

public class InputEventViewModel
{
    [Required(ErrorMessage = "Не сте въвели име")]
    [StringLength(25)]
    [MinLength(2)]
    [Display(Name = "Име")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Не сте въвели описание")]
    [StringLength(1000)]
    [MinLength(100, ErrorMessage = "Трябва да има поне 100 въведени знака")]
    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Не сте въвели минимални възраст")]
    [Range(3, 100, ErrorMessage = "Допустимите стойности са между 7 и 100")]
    [Display(Name = "Минимални години")]
    public int MinAge { get; set; }

    [Required(ErrorMessage = "Не сте въвели максимална възраст")]
    [Range(3, 100, ErrorMessage = "Допустимите стойности са между 7 и 100")]
    [Display(Name = "Максимална години")]
    public int MaxAge { get; set; }

    [Required(ErrorMessage = "Не сте въвели изисквания")]
    [StringLength(1000)]
    [MinLength(2)]
    [Display(Name = "Изисквания")]
    public string Requirements { get; set; }

    [Required(ErrorMessage = "Не сте въвели крайна дата")]
    [Display(Name = "Крайна дата за кандидатстване")]
    [FutureDate(ErrorMessage = "Датата трябва бъде в бъдещето")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime ExpirationDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Не сте въвели линк за форма за кандидатстване")]
    [Display(Name = "Форма за кандидатстване (LINK)")]
    public string ApplicationUrl { get; set; }

    public List<SelectListItem> Provinces { get; set; } = new();

    public List<SelectListItem> Cities { get; set; } = new();

    [Required(ErrorMessage = "Не сте избрали град")]
    [Display(Name = "Град")]
    public int CityId { get; set; }

    [Required(ErrorMessage = "Не сте въвели начална дата")]
    [Display(Name = "Начална дата")]
    [FutureDate(ErrorMessage = "Датата трябва бъде в бъдещето")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Не сте въвели крайна дата")]
    [Display(Name = "Крайна дата")]
    [FutureDate(ErrorMessage = "Датата трябва бъде в бъдещето")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime EndDate { get; set; } = DateTime.Today;


    public int OrganizationId { get; set; }

    public List<SelectListItem> Interests { get; set; }

    [Display(Name = "Интереси")]
    [EnsureOneElement(ErrorMessage = "Не сте избрали интерес")]
    public List<int> InterestsIds { get; set; } = new();

    public List<SelectListItem> Categories { get; set; }

    [Display(Name = "Категории")]
    [EnsureOneElement(ErrorMessage = "Не сте избрали категория")]
    public List<int> CategoriesIds { get; set; } = new();

    [Required(ErrorMessage = "Не сте качили снимка")]
    [Display(Name = "Снимка")]
    public IFormFile Image { get; set; }
}
