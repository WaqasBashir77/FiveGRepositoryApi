using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.Web.Security;
using FiveGApi.DTOModels;
using FiveGApi.Models;
using System.Linq;

namespace FiveGApi.Controllers
{
    public class AccountController : ApiController
    {
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        [HttpPost]
        [Route("Authenticate")]
        public IHttpActionResult Login([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;

            IHttpActionResult response;
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            bool isUsernamePasswordValid = false;

            //isUsernamePasswordValid=loginrequest.Password=="admin" ? true:false;
            var dbUser = db.Users.Where(x => x.UserName == login.Username && x.Password == login.Password).SingleOrDefault(); //_userRepo.GetUser(loginrequest.Username, loginrequest.Password);
            if (dbUser != null)
                isUsernamePasswordValid = true;
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = createToken(loginrequest.Username);
                //return the token
                var user = new
                {
                    token = token,
                    user = dbUser
                };
                return Ok<object>(user);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(loginResponse.responseMsg);
                return response;
            }
        }



        private string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            string sec = ConfigurationManager.AppSettings["TokenKey"].ToString();
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: ConfigurationManager.AppSettings["ApiUrl"], audience: ConfigurationManager.AppSettings["ApiUrl"],
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
        public IHttpActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Ok();
        }
    }
}
