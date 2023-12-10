namespace IMS.ViewModels.Validation
{
    public class ProjectViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public bool? Status { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Class is required")]
        public int? ClassId { get; set; }
    }
}
