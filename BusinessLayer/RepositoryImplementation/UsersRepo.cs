using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class UsersRepo : IUser
    {
        private readonly EcDbContext _db;
        private readonly IConfiguration _configuration;

        public UsersRepo(EcDbContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            _db = db;
        }

        public async Task<UserDTO> AddUserasync(UserDTO obj, int role)
        {
            try
            {
                User user = new User()
                {
                    ID = obj.ID,
                    Name = obj.Name,
                    RoleId = role,
                };
                _db.Users.AddAsync(user);
                _db.SaveChangesAsync();

                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<UserResponseDTO>> AllUsers()
        {
            var data = await _db.Users.Include(x => x.Roles).Select(x => new UserResponseDTO()
            {
                ID = x.ID,
                Name = x.Name,
                RoleName = x.Roles.RoleName
            }).ToListAsync();

            return data;
        }

        public string Login(int UserId)
        {
            try
            {
                var role = _db.Users.Where(x => x.ID == UserId).Include(x => x.Roles).FirstOrDefault();

                if (role == null)
                {
                    return ("No user Found");
                }
                List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , role.Name),
                new Claim (ClaimTypes.NameIdentifier , UserId.ToString()),
                new Claim(ClaimTypes.Role , role.Roles.RoleName),
            };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: cred

                        );

                var JwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                return (JwtToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UserDTO> DeleteUser(int id)
        {
            try
            {
            var deleteuser = _db.Users.Where(x=>x.ID== id).FirstOrDefault();  
                if (deleteuser == null)
                {
                    throw new Exception($"No User Found with userID {id} ");
                }
                
                _db.Users.Remove(deleteuser);
                _db.SaveChanges();
                return new UserDTO { ID = id ,  Name = deleteuser.Name };
                

            }
            catch (Exception)
            {

                throw;
            }
            
    }
        }

}