using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            this._context = context;
        }

        // one end point to get all the users from our database
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
            return await _context.Users.ToListAsync();
        }

        // one end point to get a specific user from the database
        [EnableCors("Policy1")]
        [HttpGet("{id}")] // api/users/id
        public async Task<ActionResult<AppUser>> GetUser(int id) {
            return await _context.Users.FindAsync(id);
        }
    }
}