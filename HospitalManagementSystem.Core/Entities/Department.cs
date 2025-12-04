using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Entities
{
    public class Department : BaseEntity
    {
        public required string Name { get; set; } 
        public string? Description { get; set; }
    }
}
