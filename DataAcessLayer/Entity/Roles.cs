using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Roles : Audit
    {
        public int ID { get; set; }
        public string RoleName { get; set; }

        public ICollection<User> users { get; set; }
    }
}