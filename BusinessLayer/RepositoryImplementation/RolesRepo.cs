using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class RolesRepo : IRoles
    {
        private readonly EcDbContext _db;

        public RolesRepo(EcDbContext db)
        {
            _db = db;
        }

        public async Task<RoleDTO> GetRolesAsync(RoleDTO role)
        {
            try
            {
                Roles obj = new Roles()
                {
                    RoleName = role.RoleName,
                    ID = role.ID,
                };
                _db.Roles.AddAsync(obj);
                _db.SaveChangesAsync();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> ViewRolesAsync()
        {
            try
            {
                var obj = _db.Roles.Include(x => x.users)
                    .Select(x => new RoleDTO()
                    {
                        ID = x.ID,
                        RoleName = x.RoleName,
                        users = x.users.Select(u => new UserRoleDTO()
                        {
                            ID = u.ID,
                            Name = u.Name
                        }).ToList(),
                    })
                    .ToList();

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}