﻿using System;
namespace IMS.ViewModels.Auth
{
    public class Contact
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        [Display(Name = "Contact")]
        public int Id { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please enter your message.")]
        public string? Content { get; set; }
    }
}
