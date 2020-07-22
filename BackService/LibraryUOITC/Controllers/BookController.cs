using LibraryUOITC.Data;
using LibraryUOITC.Model.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibContext _dbContext;

        public BookController(LibContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateBookModel model)
        {
            model.Name = model.Name.ToLower();
            if (await _dbContext.Books.AnyAsync(f => f.Name == model.Name))
                return BadRequest("duplicated name");

            await _dbContext.Books.AddAsync(new Infrastructure.Book()
            {
                Name = model.Name,
                Code = model.Code,
                SectionId = model.SectionId,
                Note = model.Note,
                AuthorName = model.AuthorName,
                NamePrinting = model.NamePrinting,
                Date = model.Date,
                Path = model.Path,
                Photos = model.Photos,
                ShelfNumber=model.ShelfNumber
            });
            await _dbContext.SaveChangesAsync();
            return Ok("Book successfuly created");
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            var Book = await _dbContext.Books.Where(f => f.Id == Id).FirstOrDefaultAsync();
            if (Book == null) return NotFound();

            _dbContext.Remove(Book);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("show")]
        public async Task<IActionResult> Show(int? SectionId)
        {

            var book = await _dbContext.Books.AsNoTracking()
                .Where(f=>SectionId.HasValue?f.SectionId==SectionId:true)
                .Select(f => new
            {
                f.Id,
                f.Name,
                f.Code,
                f.SectionId,
                SectionName=f.Section.Name,
                f.Note,
                f.AuthorName,
                f.NamePrinting,
                f.Date,
                f.Path,
                Photos=f.Photos,
                f.ShelfNumber
            }).ToListAsync();
            return await Task.FromResult(StatusCode(200, new
            {
                code = 200,
                message = "book successfuly",
                result = new
                {
                    book
                }
            }));
        }

        [HttpGet("showbybook")]
        public async Task<IActionResult> ShowByBook(int Id)
        {
            var book = await _dbContext.Books.AsNoTracking()
                .Where(f => f.Id == Id)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Code,
                    f.SectionId,
                    SectionName = f.Section.Name,
                    f.Note,
                    f.AuthorName,
                    f.NamePrinting,
                    f.Date,
                    f.Path,
                    Photos = f.Photos,
                    f.ShelfNumber
                }).FirstOrDefaultAsync();
            return await Task.FromResult(StatusCode(200, new
            {
                code = 200,
                message = "book successfuly",
                result = new
                {
                    book
                }
            }));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> upload([FromForm] Model.Book.File fileInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                IFormFile file = fileInput.file;

                string host = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                Directory.CreateDirectory(Path.Combine(host));

                var pathImage = Path.Combine(host, file.FileName);
                using (var stream = new FileStream(pathImage, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                string id = ChangeNameFile(pathImage, file);
                return Ok(new { stutes = true, path = Path.Combine(id + file.FileName) });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    res = ex.Message
                });
            }
        }
        private string ChangeNameFile(string path, IFormFile fie)
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var stringChars = new char[30];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);
                System.IO.File.Move(path, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", finalString + fie.FileName));
                System.IO.File.Delete(path);
                return finalString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string filepath)
        {
            try
            {
                if (filepath == null)
                    return Content("filename not present");
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filepath);


                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/pdf", Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
