using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
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
    }
}