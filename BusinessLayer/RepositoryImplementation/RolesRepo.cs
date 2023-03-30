using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class RolesRepo : IRoles
    {
        #region DependencyInjection
        private readonly EcDbContext _db;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        #endregion DependencyInjection


        public RolesRepo(EcDbContext db, IMapper mapper, IMemoryCache memoryCache)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoryCache; 
        }


        #region AddRole
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
        #endregion AddRole


        #region ViewRole

        public async Task<IEnumerable<RoleResponseDTO>> ViewRolesAsync()
        {
            try
            {

                var CacheKey = "GetRole";

                //if (!_memoryCache.TryGetValue(CacheKey, out List<RoleResponseDTO> Data))
                //{
                     var Data = _db.Roles.Include(x => x.users)
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

                    //?setting cache to local memory

                //    var cacheExpireOption = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddSeconds(59),
                //        Priority = CacheItemPriority.High,
                //        // will delete data if not call in 30 sec
                //        SlidingExpiration = TimeSpan.FromSeconds(30),
                //    };
                //    _memoryCache.Set(CacheKey, Data, cacheExpireOption);
                //}


                return Data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion ViewRole
    }
}