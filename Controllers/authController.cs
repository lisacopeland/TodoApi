using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{

    [ApiController]
    public class authController : ControllerBase
    {

        private const string _clientId = "c18kuusdt073nqkk1uecnusd4";
        private readonly RegionEndpoint _region = RegionEndpoint.USWest2;

        public class AWSUser
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
            public string? Email { get; set; }
        }

        [Route("auth/register")]
        public async Task<ActionResult<string>> Register([FromQuery] AWSUser user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Password = user.Password,
                Username = user.Email,

            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };
            request.UserAttributes.Add(emailAttribute);

            var response = await cognito.SignUpAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return Ok();
            } else {
              return BadRequest();
            }
        }

        [Route("auth/signin")]
        public async Task<ActionResult<string>> SignIn([FromQuery] string Username,[FromQuery] string Password, [FromQuery] string Email)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = "us-west-2_IrEqKjSrn",
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_USER_PASSWORD_AUTH
            };

            request.AuthParameters.Add("USERNAME", Username);
            request.AuthParameters.Add("PASSWORD", Password);

            var response = await cognito.AdminInitiateAuthAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.AuthenticationResult);
            }
            else
            {
                return BadRequest();
            }
        }        
    }
}
