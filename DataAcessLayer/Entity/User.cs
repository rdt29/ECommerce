using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }

        public ICollection<Products> products { get; set; }

        #region Navigation

        //?----------------------Roles-----------------------------------
        [ForeignKey(nameof(RoleId))]
        public Roles Roles { get; set; }

        #endregion Navigation
    }
}