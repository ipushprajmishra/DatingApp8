using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController(DataContext context,ITokenService tokenService) : BaseApiController
    {

        [HttpPost("register")]   //account/register

        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {

            if (await UserExists(registerDTO.UserName)) return BadRequest("username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key

            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDTO{
                UserName=user.UserName,
                Token=tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {

            return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());


        }


        [HttpPost("login")]   //account/login

        public async Task<ActionResult<UserDTO>> Llgin([FromBody] LoginDTO loginDTO)
        {

            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.UserName.ToLower());

            if (user is null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("wrong password");
            }
           return new UserDTO{
                UserName=user.UserName,
                Token=tokenService.CreateToken(user)
            };
        }

    }
}
