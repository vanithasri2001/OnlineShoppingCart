using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShoppingCart.Models;
using OnlineShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;
        public UserController(IUserRepository user) 
        {
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _user.GetAllAsync();
            return Ok(users);
        }
        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetuserAsync")]
        public async Task<IActionResult> GetuserAsync(int id)
        {
            var users = await _user.GetAsync(id); if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpPost]
        
        [Route("register")]


        public async Task<IActionResult> AddemployeeAsync(UserModel adduser)
        {
            var user = new Models.UserModel()
            {
                FirstName = adduser.FirstName,
                LastName = adduser.LastName,
                PhoneNo = adduser.PhoneNo,
                EmailID = adduser.EmailID,
                Gender=adduser.Gender,
                City=adduser.City,
                Password = adduser.Password,
                ConfirmPassword = adduser.ConfirmPassword,

            };
            var pass = CheckPasswordStrength(adduser.Password); 
            if (!string.IsNullOrEmpty(pass)) return BadRequest(new { Message = pass.ToString() });
            user = await _user.AddAsync(user);
            
          return Ok(new { message = "Register Successful" });


        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var employeeid = await _user.DeleteAsync(id);
                if (employeeid == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            { }
            return Ok();
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] int id, [FromBody] UserModel updateuser)
        {
            try
            {
                var user = new Models.UserModel()
                {
                    FirstName = updateuser.FirstName,
                    LastName = updateuser.LastName,
                    PhoneNo = updateuser.PhoneNo,
                    EmailID = updateuser.EmailID,
                    Gender=updateuser.EmailID,
                    City=updateuser.City,
                    Password = updateuser.Password,
                    ConfirmPassword = updateuser.ConfirmPassword,

                };
                user = await _user.UpdateAsync(id, user);
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            { }
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> AddLogin([FromBody] LoginModel loginmodel)
        {
            if (loginmodel == null)
            {
                return BadRequest();
            }
            var u = await _user.LoginModel(loginmodel); if (u == null) { return BadRequest(new { message = "User not found" }); }
            await _user.LoginModel(loginmodel); string Token = CreateJwt(u); return Ok(new { Token, message = "Login Successful" });
        }
        private string CheckPasswordStrength(string password) { StringBuilder sb = new StringBuilder(); if (password.Length < 8) sb.Append("Minimum password length should be 8" + Environment.NewLine); if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]"))) sb.Append("password should be Alphanumeric" + Environment.NewLine); if (!(Regex.IsMatch(password, "[<,>,@,!,#,$,^,?]"))) sb.Append("Password should contain special Character" + Environment.NewLine); return sb.ToString(); }
        private string CreateJwt(UserModel user) { var jwtTokenHandler = new JwtSecurityTokenHandler(); var key = Encoding.ASCII.GetBytes("chetansatyateja9"); var identity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, $"{user.FirstName}{user.LastName}") }); var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256); var tokenDescriptor = new SecurityTokenDescriptor { Subject = identity, Expires = DateTime.Now.AddDays(1), SigningCredentials = credentials }; var token = jwtTokenHandler.CreateToken(tokenDescriptor); return jwtTokenHandler.WriteToken(token); }
    }
    }

