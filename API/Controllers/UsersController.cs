using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using API.Extensions;
using API.Entities;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IUserRepository _userRepository;
        public readonly IPhotoService _photoService;
        public readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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
        
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return Ok("Your account has been updated successfully");

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0) {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDto>(photo);

            return BadRequest("Problem adding photo");
        }
    }
}