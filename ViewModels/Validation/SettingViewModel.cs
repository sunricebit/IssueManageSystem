namespace IMS.ViewModels.Validation
{
    public class SettingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Order is required")]
        [Range(1, 255, ErrorMessage = "Order must be a integer between 1 and 255.")]
        public sbyte? Order { get; set; }
    }
}
