using Indigo.DAL;
using Indigo.Models;
using Indigo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static NuGet.Packaging.PackagingConstants;

namespace Indigo.Areas.IndigoAdmin.Controllers
{
    [Area("IndigoAdmin")]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PostController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _context.Posts.ToListAsync();
            return View(posts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePostVM postVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string fileName = Guid.NewGuid().ToString()+postVM.Photo.FileName;
            using (FileStream fileStream = new FileStream(Path.Combine(_env.WebRootPath, @"images", fileName), FileMode.Create))
            {
                await postVM.Photo.CopyToAsync(fileStream);
            }
            Post post = new Post()
            {
                Title= postVM.Title,
                Description= postVM.Description,
                Image = fileName
            };
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Post existed = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();
            UpdatePostVM postVM = new UpdatePostVM()
            {
                Title= existed.Title,
                Description= existed.Description,
                Image = existed.Image,
            };
            return View(postVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdatePostVM postVM)
        {
            if (id == null || id < 1) return BadRequest();
            Post existed = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();
            if (postVM.Photo != null)
            {
                string path = Path.Combine(_env.WebRootPath, @"images", existed.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string fileName = Guid.NewGuid().ToString() + postVM.Photo.FileName;
                using (FileStream file = new FileStream(Path.Combine(_env.WebRootPath, @"images", fileName), FileMode.Create))
                {
                    await postVM.Photo.CopyToAsync(file);
                }
                existed.Image = fileName;
            }
            existed.Title = postVM.Title;
            existed.Description = postVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Post existed = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if(existed == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, @"images", existed.Image);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Posts.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
