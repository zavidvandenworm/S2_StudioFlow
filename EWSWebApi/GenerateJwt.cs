using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EWSDomain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace EWSWebApi;

public static class GenerateJwt
{
    public static string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("db0d3f294fb2357b22c851702795e3f33014e64ea863b463712ff2b3783b53e92eefaf549a2c34c42905c6eacea31b11634bb015cc37c96808a390e953b86cf0134fdaf89469dca31843f7ce666aa0bce3568362a673f16e89f82dcadfc2ab3f05b309e9a290129230ab781e31641642051ccccd480b685f1dc82e7d984cc8b7ba18f4cc104d18cca786c50f7f496a7a5dbb1661074755033572a38e1243c9395b188a62a17475a943a4fd8bbfa570a603589a9a7616f4b4535414b548789e9c9aeffcf3a8fe52d15a58128a91e0baad04d26dcd7ef3e5823397bdaa20e22e48ada681fe18b00f6f2e558c8b82ddd32fdc95ce0aa1192992721577c1c773da36"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}