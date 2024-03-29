﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Discoverabillity;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;
using ShopBackend.Security;
using ShopBackend.Utils;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordAuth _passwordAuth;
        private readonly LinkGenerator _linkGenerator;

        public UsersController(IUserRepository userRepository, IAuthService authService, IPasswordAuth passwordAuth, LinkGenerator linkGenerator )
        {
            _userRepository = userRepository;
            _authService = authService;
            _passwordAuth = passwordAuth;
            _linkGenerator = linkGenerator;
        }
        //Get api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = (await _userRepository.GetAll()).Select(user => user.AsUserDto());
            if (users.Any())
            {
                var userList = users.ToList();
                foreach (UserDto userDto in userList)
                {
                    userDto.Links = (List<Link>)CreateLinksForUser(userDto.Email, "GET");
                }

                return Ok(userList);
            }

            return NotFound("The specified users does not exist!");
        }

        //Get api/users
        [HttpGet("{email}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<UserDto>> GetUser(string email)
        {
            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)
            {
                return BadRequest("Access denied!");
            }

            var result = await _userRepository.Get(email);
            if (result != default)
            {
                UserDto userDto = result.AsUserDto();
                userDto.Links = (List<Link>)CreateLinksForUser(userDto.Email, "GET");
                return Ok(userDto);
            }

            return NotFound("The specified user does not exist!");
        }


        //Post api/users/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RegisterUser([FromBody] CreateUserDto userDto)
        {
            var isPasswordStrong = _passwordAuth.IsPasswordStrong(userDto.Password);
            if (!isPasswordStrong)
            {
                return BadRequest("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!");
            }

            if (string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest("User email is required to register the user!");
            }

            var isEmailValid = _passwordAuth.IsEmailValid(userDto.Email);
            if (!isEmailValid)
            {
                return BadRequest("Email format is not valid!");
            }

            var isEmailTaken = await _userRepository.Get(userDto.Email);
            if (isEmailTaken != default)
            {
                return BadRequest("This email is already in use!");
            }

            var user = new User
            {
                Email = userDto.Email,
                Password = _passwordAuth.GeneratePasswordHash(userDto.Password),
                Role = UserRoles.Customer,
            };

            var result = await _userRepository.Insert(user);
            if (result != default && result > 0)
            {
                await _authService.AuthenticateUser(new LoginDto { Email = userDto.Email, Password = userDto.Password });
                var token = _authService.CreateToken();
                var msg = "User is inserted successfully!";
                return Ok(new { Token = token, Msg = msg });
            }

            return NotFound("User could not be registered!");
        }


        //Put api/customers
        [HttpPut]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<string>> UpdateUser([FromBody] UpdateUserDto userDto)
        {
            var isPasswordStrong = _passwordAuth.IsPasswordStrong(userDto.Password);
            if (!isPasswordStrong)
            {
                return BadRequest("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!");
            }

            if (string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest("User email is required to update the user!");
            }

            var isEmailValid = _passwordAuth.IsEmailValid(userDto.Email);
            if (!isEmailValid)
            {
                return BadRequest("Email format is not valid!");
            }

            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != userDto.Email)
            {
                return BadRequest("Access denied!");
            }

            var userToUpdate = await _userRepository.Get(userDto.Email);
            if (userToUpdate == default)
            {
                return NotFound("User does not exsist!");
            }

            userToUpdate.Password = _passwordAuth.GeneratePasswordHash(userDto.Password);

            var result = await _userRepository.Update(userToUpdate);
            if (result != default && result > 0)
            {
                return Ok(CreateLinksForUser(userToUpdate.Email, "PUT"));
            }

            return NotFound("The specified user does not exist!");
        }


        //Delete api/customers
        [HttpDelete("{email}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<string>> DeleteUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User email is required to delete the user!");
            }

            var userEmail = _authService.GetEmailFromToken(User);
            var userRole = _authService.GetRoleFromToken(User);
            if (userRole != UserRoles.Admin.ToString() && userEmail != email)
            {
                return BadRequest("Access denied!");
            }

            var customerToDelete = await _userRepository.Get(email);
            if (customerToDelete == default)
            {
                return NotFound("User does not exsist!");
            }

            var result = await _userRepository.Delete(email);
            if (result != default)
            {
                return Ok("User has been deleted!");
            }

            return NotFound("User could not be deleted!");
        }

        //Based on https://code-maze.com/hateoas-aspnet-core-web-api/
        private IEnumerable<Link> CreateLinksForUser(string email, string requestType)
        {
            
            var GetUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(GetUser), values: new { email })!;
            var UpdateUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(UpdateUser))!;
            var DeleteUrl = _linkGenerator.GetUriByAction(HttpContext, nameof(DeleteUser), values: new { email })!;
            switch (requestType)
            {
                case "GET":
                    return new List<Link> {
                        new Link(DeleteUrl, "delete_user", "DELETE"),
                        new Link(UpdateUrl, "update_user",  "PUT")
                    };
                case "PUT":
                    return new List<Link>
                    {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_user", "DELETE")
                    };
                case "POST":
                    return new List<Link> {
                        new Link(href: GetUrl, "self", "GET"),
                        new Link(href: DeleteUrl, "delete_user", "DELETE"),
                        new Link(href: UpdateUrl, "update_user",  "PUT")
                     };
                default:
                    throw new Exception("Invalid requestType");
            }
        }
    }
}

