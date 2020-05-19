using AuthMicroservice.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.Tests.IntegrationTests
{
    public class UsersControllerTests
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        string emailToChangePassword = "";
        [OneTimeSetUp]
        public async Task InitialSetup()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
            var httpAuthResponse = await _client.PostAsync("/api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB/users/login", new StringContent("{\"Email\":\"sanyjose85@gmail.com\",\"Password\":\"2wsx@WSX2\"}", Encoding.UTF8,
                                    "application/json"));

            var stringAuthResponse = await httpAuthResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenModel>(stringAuthResponse);
            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);


        }

        [Test]
        public async Task Should_Register_RegularHRS_user_Work()
        {
            string email = "testemail" + Guid.NewGuid() + "@email.com";
            emailToChangePassword = email;
            string data = "{\"Email\":\"" + email + "\",\"Password\":\"1qaz!QAZ1\",\"ConfirmUrl\":\"http://healthrecordstack.com/confirmemail\"}";
            var httpResponse = await _client.PostAsync("api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB/users/register", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        }

        [Test]
        public async Task Should_Register_Tenant_user_Work()
        {
            string email = "testemail" + Guid.NewGuid() + "@email.com";
            string data = "{\"Email\":\"" + email + "\",\"Password\":\"1qaz!QAZ1\",\"ConfirmUrl\":\"http://healthrecordstack.com/confirmemail\",\"Roles\":[\"TestAdmin\",\"HRSUSER\"]}";
            var httpResponse = await _client.PostAsync("api/tenants/A7BC6C3B-74DE-4F5B-B9C9-94545ABCC6C3/users/tregister", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        }

        [Test]
        public async Task Should_Forgot_Password_Work()
        {

            string data = "{\"Email\":\"sanyjose90@gmail.com\",\"ForgotPasswordUrl\":\"http://healthrecordstack.com/forgotpassword\"}";
            var httpResponse = await _client.PostAsync("api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB/users/forgotpassword", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        }

        [Test]
        public async Task Should_Change_Password_Work()
        {

            string data = "{\"Email\":\"sanyjose90@gmail.com\",\"OldPassword\":\"1qaz!QAZ1\",\"NewPassword\":\"2wsx@WSX2\",\"ConfirmNewPassword\":\"2wsx@WSX2\"}";
            var httpResponse = await _client.PostAsync("api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB/users/changepassword", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        }


    }
}
