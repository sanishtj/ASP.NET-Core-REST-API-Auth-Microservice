using AuthMicroservice.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.Tests.IntegrationTests
{
    public class AdministrationControllerTests
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
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
        public async Task Should_Return_All_Roles_Results()
        {

            // _client.DefaultRequestHeaders.Authorization = "Bearer "+ token.Token;
            var httpResponse = await _client.GetAsync("api/admin/role");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<IEnumerable<RoleModel>>(stringResponse);
            Assert.That(roles.Count(), Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public async Task Should_AddRole_Work()
        {
            string data = "{\"RoleName\":\"TestRole\"}";
            var httpResponse = await _client.PostAsync("api/admin/role", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));


        }

        [Test]
        public async Task Should_UpdateRole_Work()
        {

            string data = "{\"RoleName\":\"TestAdminMod\"}";
            var httpResponse = await _client.PutAsync("api/admin/role/e70b15de-fd8a-4423-9f56-566c74930dd0", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));

        }

        [Test]
        public async Task Should_DeleteRole_Work()
        {

            var httpResponse = await _client.DeleteAsync("api/admin/role/dd3f46ee-3e4b-47d4-8da9-c1ad4aae8afd");

            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));

        }
    }
}
