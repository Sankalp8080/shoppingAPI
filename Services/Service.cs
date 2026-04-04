using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace ShoppingAPI.Services
{
    public class Service
    {
        //IConfiguration lets you read values from appsettings.json
        public readonly IConfiguration _config;
        public Service(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(string email)
        {
            //Claims = data inside token
            var Clams = new[] {
            new Claim(ClaimTypes.Name,email),   //ClaimTypes.Name → stores username
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())   //Jti → unique token ID (prevents reuse)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); //SymmetricSecurityKey → creates key object  //  Encoding.UTF8.GetBytes(...) → converts string → bytes  //_config["Jwt:Key"] → reads secret key

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //SigningCredentials → creates signing credentials object  //SecurityAlgorithms.HmacSha256 → specifies hashing algorithm

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],       //Who created token
                audience: _config["Jwt:Audience"],  //Who token is for    
                claims: Clams,  //Claims → data inside token
                expires: DateTime.Now.AddMinutes(30), //Token expiration time
                signingCredentials: creds); //SigningCredentials → signing credentials object

            return new JwtSecurityTokenHandler().WriteToken(token); //JwtSecurityTokenHandler → creates token handler object  //WriteToken(token) → generates token string from token object
        }
    }
}
