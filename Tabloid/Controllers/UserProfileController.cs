﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Tabloid.Data;
using Tabloid.Models;
using Tabloid.Repositories;

namespace Tabloid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileRepository _userProfileRepository;
        public UserProfileController(ApplicationDbContext context)
        {
            _userProfileRepository = new UserProfileRepository(context);
        }

        [HttpGet("{firebaseUserId}")]
        public IActionResult GetUserProfile(string firebaseUserId)
        {
            var user = _userProfileRepository.GetByFirebaseUserId(firebaseUserId);
            return Ok(user);
        }

        [HttpGet("getuserprofilebyid/{id}")]
        public IActionResult GetUserProfileById(int id)
        {
                return Ok(_userProfileRepository.GetByUserId(id));
        }

        [HttpPost]
        public IActionResult Post(UserProfile userProfile)
        {
            userProfile.CreateDateTime = DateTime.Now;
            userProfile.UserTypeId = UserType.AUTHOR_ID;
            _userProfileRepository.Add(userProfile);
            return CreatedAtAction(
                nameof(GetUserProfile),
                new { firebaseUserId = userProfile.FirebaseUserId },
                userProfile);
        }

        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            var userProfile = GetCurrentUserProfile();
            if(userProfile.UserTypeId == 1)
            {
            return Ok(_userProfileRepository.GetAll());
            }
            else
            {
                return Unauthorized();
            }
        }
        private UserProfile GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userProfileRepository.GetByFirebaseUserId(firebaseUserId);
        }

        [HttpPut("updateuserprofile/{id}")]
        public IActionResult UpdateUserProfile(UserProfile userProfile, int id)
        {
            var currentUserProfile = GetCurrentUserProfile();
            if (currentUserProfile.UserTypeId == 1)
            {
                if (id != userProfile.Id)
                {
                return BadRequest();
                }

                _userProfileRepository.Update(userProfile, currentUserProfile);
                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
