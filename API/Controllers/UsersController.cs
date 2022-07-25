using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public IUserRepository _userRepository;

        public IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // one end point to get all the users from our database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
            return Ok(await _userRepository.GetMembersAsync());
        }

        // one end point to get a specific user from the database
        [HttpGet("{username}")] // api/users/id
        public async Task<ActionResult<MemberDto>> GetUser(string username) {
            return await _userRepository.GetMemberAsync(username);
        }
        
    }
}