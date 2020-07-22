using LibraryUOITC.Data;
using LibraryUOITC.Model.Section;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly LibContext _dbContext;

        public SectionController(LibContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateSectionModel model)
        {

            if (await _dbContext.Sections.AnyAsync(f => f.Name == model.Name))
                return BadRequest("duplicated name");

            await _dbContext.Sections.AddAsync(new Infrastructure.Section()
            {
                Name = model.Name
            });
            await _dbContext.SaveChangesAsync();
            return Ok("Section successfuly created");
        }

        [HttpGet("show")]
        public async Task<IActionResult> Show()
        {

            var section = await _dbContext.Sections.AsNoTracking().Select(f => new
            {
                f.Id,
                f.Name
            }).ToListAsync();
            return await Task.FromResult(StatusCode(200, new
            {
                code = 200,
                message = "section successfuly",
                result = new
                {
                    section
                }
            }));
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit(int Id,[FromBody]CreateSectionModel model)
        {
            var Section = await _dbContext.Sections.Where(f => f.Id == Id).FirstOrDefaultAsync();
            if (Section==null)
                return NotFound();

            Section.Name = model.Name;
            await _dbContext.SaveChangesAsync();
            return Ok("Section successfuly Edited");
        }
    }
}
