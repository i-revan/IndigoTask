namespace Indigo.ViewModels
{
    public class CreatePostVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
