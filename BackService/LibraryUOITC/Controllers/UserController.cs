using LibraryUOITC.Data;
using LibraryUOITC.Model.User;
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
    public class UserController : ControllerBase
    {
        private readonly LibContext _dbContext;

        public UserController(LibContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthModel model)
        {

            var User = await _dbContext.Users.AsNoTracking()
                .Where(f => f.UserName == model.UserName.ToLower() && f.IsDeleted.Value == false )
                .Select(f => new
                {
                    f.Id,
                    f.UserName,
                    f.Password,
                    f.FullName,
                    f.PolicyId,
                    f.Policy.Name,
                    f.Policy.Number,
                    f.IsDeleted
                }).FirstOrDefaultAsync();

            if (User == null) return NotFound("User Dose Not Exist OR invalid User password");

            if (!JwtAuth.Tools.Hasher.Compare(User.Password, model.Password)) return NotFound("User Dose Not Exist OR invalid User password");


            Dictionary<string, object> cl = new Dictionary<string, object>();
            cl.Add("UserID", User.Id);
            cl.Add("PolicyId", User.PolicyId);
            cl.Add("PolicyNumber", User.Number);


            return StatusCode(200, new
            {
                code = 200,
                message = "user is exsits",
                result = new
                {
                    UserId = User.Id,
                    FullName = User.FullName,
                    PolicyNumber = User.Number,
                    Token = User.FullName+ User.FullName+ User.FullName
                }
            });

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateUserModel model)
        {
            model.UserName = model.UserName.ToLower();
            if (await _dbContext.Users.AnyAsync(f => f.UserName == model.UserName))
                return BadRequest("duplicated username");

            await _dbContext.Users.AddAsync(new Infrastructure.User()
            {
                UserName=model.UserName,
                Password=model.Password,
                FullName=model.FullName,
                PolicyId=1
            });
            await _dbContext.SaveChangesAsync();
            return Ok("user successfuly created");
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit(int Id, [FromBody] CreateUserModel model)
        {
            model.UserName = model.UserName.ToLower();
            var user = await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == Id && f.IsDeleted.Value == false);
            if (user == null) return NotFound("User Dose Not Exist");
            if (model.UserName.Length > 0 && await _dbContext.Users.AsNoTracking().AnyAsync(a => a.UserName == model.UserName && a.Id != Id && a.IsDeleted.Value == false))
                return BadRequest("Duplicated Name ");
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            await _dbContext.SaveChangesAsync();
            return Ok("user updated successfuly");

        }

        [HttpGet("show")]
        public async Task<IActionResult> Show()
        {

            var user = await _dbContext.Users.AsNoTracking().Select(f => new
            {
                f.Id,
                f.UserName,
                f.FullName,
                f.PolicyId,
                f.Policy.Name,
                f.IsDeleted
            }).ToListAsync();
            return await Task.FromResult(StatusCode(200, new
            {
                code = 200,
                message = "user successfuly",
                result = new
                {
                    user
                }
            }));
        }

        [HttpGet("showbyuser")]
        public async Task<IActionResult> ShowByUser(int Id)
        {
            var user = await _dbContext.Users.AsNoTracking()
                .Where(f => f.Id == Id)
                .Select(f => new
                {
                    f.Id,
                    f.UserName,
                    f.FullName,
                    f.PolicyId,
                    f.Policy.Name,
                    f.IsDeleted
                }).FirstOrDefaultAsync();
            return await Task.FromResult(StatusCode(200, new
            {
                code = 200,
                message = "user successfuly",
                result = new
                {
                    user
                }
            }));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int Id)
        {

            var User = await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == Id);
            if (User == null) return NotFound("user does not exsits");

            User.IsDeleted = User.IsDeleted == false;
            await _dbContext.SaveChangesAsync();
            return StatusCode(200, new
            {
                code = 200,
                message = "user delete successfuly",
                result = (object)null
            });
        }
    }
}
