using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Audit
    {
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}