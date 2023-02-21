using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public interface IUser
    {
        public Task<UserDTO> AddUserasync(UserDTO obj, int role);

        public Task<IList<UserResponseDTO>> AllUsers();

        string Login(int UserId);

        public Task<UserDTO> DeleteUser(int id);
    }
}