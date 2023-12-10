using System;
namespace IMS.ViewModels.Subject
{
	public class AddSubjectViewModel
	{
        [Required(ErrorMessage = "Please enter subject code.")]
        [StringLength(10, ErrorMessage = "The code must be at least {2} characters long and max 10 characters.", MinimumLength = 3)]
        [RegularExpression(@"^\S*$", ErrorMessage = "Code cannot contain spaces")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter subject name.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = false;

        [Required(ErrorMessage = "Add subject manager")]
        [Display(Name = "Manager")]
        public int SubjectManagerId { get; set; }
    }
}

