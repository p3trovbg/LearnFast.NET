﻿// ReSharper disable VirtualMemberCallInConstructor
namespace LearnFast.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using LearnFast.Data.Common.Models;
    using LearnFast.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class Course : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Requirments { get; set; }

        public bool IsFree { get; set; }

        public int Grade { get; set; }

        public Difficulty Difficulty { get; set; }

        public DateTime Duration { get; set; }

        public int Sells { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int ContentId { get; set; }

        public virtual Content Content { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<StudentCourse> Students { get; set; }
    }
}
