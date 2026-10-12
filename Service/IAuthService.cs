using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Service
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Register(string name, string email, string password);
        Task<ApiResponse<string>> Login(string email, string password);
        Task<ApiResponse<string>> VerifyEmail(string token);


    }
}
