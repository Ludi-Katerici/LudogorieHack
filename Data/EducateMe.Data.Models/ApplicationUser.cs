// ReSharper disable VirtualMemberCallInConstructor
using System;
using System.Collections.Generic;

using EducateMe.Data.Common.Models;

using Microsoft.AspNetCore.Identity;

namespace EducateMe.Data.Models;

public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid().ToString();
        this.Roles = new HashSet<IdentityUserRole<string>>();
        this.Claims = new HashSet<IdentityUserClaim<string>>();
        this.Logins = new HashSet<IdentityUserLogin<string>>();
    }

    // Audit info
    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    // Deletable entity
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

    public int? OrganizationId { get; set; }

    public Organization Organization { get; set; }

    public int? StudentId { get; set; }

    public Student Student { get; set; }
}
