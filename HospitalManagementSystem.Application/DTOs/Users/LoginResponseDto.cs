using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Application.DTOs.Users
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        // Sau này mày có thể thêm các thông tin khác vào đây
        // public string FullName { get; set; }
        // public List<string> Roles { get; set; }
    }
}
