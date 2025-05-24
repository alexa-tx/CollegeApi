using CollegeApi.Data;
using CollegeApi.DTOs;
using CollegeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CollegeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly CollegeDbContext _context;

        public NewsController(CollegeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var news = await _context.NewsPosts
                .OrderByDescending(n => n.DatePosted)
                .ToListAsync();
            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostNews([FromForm] NewsPostForm form)
        {
            string? imageUrl = null;
            if (form.Image != null && form.Image.Length > 0)
            {
                imageUrl = await SaveImage(form.Image);
            }

            var news = new NewsPost
            {
                Title = form.Title,
                Content = form.Content,
                DatePosted = DateTime.UtcNow,
                ImageUrl = imageUrl
            };

            _context.NewsPosts.Add(news);
            await _context.SaveChangesAsync();
            return Ok(news);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditNews(int id, [FromForm] NewsPostForm form)
        {
            var news = await _context.NewsPosts.FindAsync(id);
            if (news == null)
                return NotFound("Новость не найдена.");

            news.Title = form.Title;
            news.Content = form.Content;

            if (form.Image != null && form.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", news.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                news.ImageUrl = await SaveImage(form.Image);
            }

            _context.NewsPosts.Update(news);
            await _context.SaveChangesAsync();
            return Ok(news);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.NewsPosts.FindAsync(id);
            if (news == null)
                return NotFound("Новость не найдена.");

            if (!string.IsNullOrEmpty(news.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", news.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.NewsPosts.Remove(news);
            await _context.SaveChangesAsync();
            return Ok("Новость удалена.");
        }

        // сохранение картинки
        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "news");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/images/news/{uniqueFileName}";
        }
    }
}
