// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;
    using System.Collections.Generic;

    using LearnFast.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Reviews = new HashSet<Review>();
            this.OwnCourses = new HashSet<Course>();
            this.BuyedCourses = new HashSet<StudentCourse>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImagePath { get; set; }

        public string WebsitePath { get; set; }

        public string Biography { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<StudentCourse> BuyedCourses { get; set; }

        public virtual ICollection<Course> OwnCourses { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
