using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.BusinessLayer.IServices
{
    public interface IUserService
    {
        int AddRefreshToken(int empId, string refreshToken);
        string GenerateJwtToken(string username, string role);
        string GenerateRefreshToken();
    }
}
