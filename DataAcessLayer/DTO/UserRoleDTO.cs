using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DTO
{
    public class UserRoleDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}