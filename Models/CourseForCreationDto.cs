﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription (ErrorMessage = "Title must be different from description.")]
    public class CourseForCreationDto
    {
        [Required(ErrorMessage = "Please fill out the title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
