﻿using System.Security.Claims;
using EsyaStore.Data.Context;
using EsyaStore.Data.Entity;
using EsyaStoreApi.Extensions;
using EsyaStoreApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EsyaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenGenerator _jwtTokenGenerator;

        public UsersController(ApplicationDbContext context, TokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user=_context.users.FirstOrDefault(u=>u.Email==loginDto.Email&&u.Password==loginDto.Password);
            if ( user is null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var token=_jwtTokenGenerator.CreateToken(user,loginDto.UserType);

            return Ok(new { token });

            
        }

        [HttpGet]
        [Authorize(Policy ="UserPolicy")]
        public IActionResult GetUsers() {
           var users= _context.users.ToList();
            if(users is null)
            {
                return NotFound();
            }

            var usersdto = new List<UsersDTO>();

            foreach (var user in users) 
            {
                var newuser = new UsersDTO
                {
                    Name = user.Name,
                    Email = user.Email,
                    Contact = user.Contact,
                    isActiveUser = user.isActiveUser == 1 ? "Active" : "Not Active"
                };
                usersdto.Add(newuser);
            }
            return Ok(usersdto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult GetSpecificUser(int id) { 
          var user= _context.users.Find(id);
            if (user is null) { 
                return NotFound();
            }
            var userdto = new UsersDTO
            {
                Name = user.Name,
                Email = user.Email,
                Contact = user.Contact,
                isActiveUser = user.isActiveUser == 1 ? "Active" : "Not Active",
            };
            return Ok(userdto);
        }

        [HttpPost("/Register")]
        public IActionResult Register(CreateUserDTO newuser)
        {
            var user = new Users
            {
                Name = newuser.Name,
                Contact = newuser.Contact,
                Email = newuser.Email,
                Password = newuser.Password,
                isActiveUser = newuser.isActiveUser
            };

            _context.users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSpecificUser),new {id=user.Id},user);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult EditUsers(int id,UpdateUserDTO EditUser) {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaim != null)
            {
                return Unauthorized("Invalid token. User ID not found.");
            }
            if (userClaim != id.ToString())
            {
                return Forbid("You are not authorized to edit this user's information.");
            }
            var existingUser=_context.users.Find(id);
            if (existingUser is null)
            {
                return NotFound();
            }
            existingUser.Contact= EditUser.Contact;
            existingUser.Name= EditUser.Name;
            existingUser.isActiveUser= EditUser.isActiveUser;
            existingUser.Password = EditUser.Password;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult DeleteUser(int id) {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaim == null)
            {
                return Unauthorized("Invalid token. User ID not found.");
            }
            if (userClaim != id.ToString())
            {
                return StatusCode(403, new { Message = "You are not authorized to edit this user's information." });
            }
            var deleteUser= _context.users.Find(id);
            if(deleteUser is null)
            {
                return NotFound();
            }
            _context.users.Remove(deleteUser);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
