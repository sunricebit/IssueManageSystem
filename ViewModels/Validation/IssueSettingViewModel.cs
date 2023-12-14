namespace IMS.ViewModels.Validation
{
    public class IssueSettingViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = null!;
        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; } = null!;
        [Required(ErrorMessage = "Name is required")]
        public string Color { get; set; } = null!;
        public string Description { get; set; }
    }
}
