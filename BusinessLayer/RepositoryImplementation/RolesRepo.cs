using AutoMapper;
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
        private readonly IMapper _mapper;

        public RolesRepo(EcDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RoleDTO> AddRolesAsync(RoleDTO obj, int userId)
        {
            try
            {
                var Role = _mapper.Map<Roles>(obj);
                //Roles obj = new Roles()
                //{
                //    RoleName = RoleName,
                //    //ID = role.ID,
                //    CreatedAt = DateTime.Now,
                //    CreatedBy = userId,
                //};
                Role.CreatedAt = DateTime.Now;
                Role.CreatedBy = userId;

                await _db.Roles.AddAsync(Role);
                _db.SaveChanges();

                return (_mapper.Map<RoleDTO>(Role));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleResponseDTO>> ViewRolesAsync()
        {
            try
            {
                var obj = _db.Roles.Include(x => x.users)
                    .Select(x => new RoleResponseDTO()
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