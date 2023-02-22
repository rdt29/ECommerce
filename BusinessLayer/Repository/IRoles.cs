using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface IRoles
    {
        public Task<RoleDTO> AddRolesAsync(RoleDTO obj, int userid);

        public Task<IEnumerable<RoleResponseDTO>> ViewRolesAsync();

        //public RoleDTO GetRoleAsync(int id);
    }
}