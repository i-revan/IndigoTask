namespace Indigo.ViewModels
{
    public class UpdatePostVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
