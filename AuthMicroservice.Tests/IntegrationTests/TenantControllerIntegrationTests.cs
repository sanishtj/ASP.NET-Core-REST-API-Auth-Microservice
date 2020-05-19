using AuthMicroservice.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.Tests.IntegrationTests
{
    public class TenantControllerIntegrationTests
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
        public async Task Should_Return_2_Results()
        {

            // _client.DefaultRequestHeaders.Authorization = "Bearer "+ token.Token;
            var httpResponse = await _client.GetAsync("/api/tenants");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tenants = JsonConvert.DeserializeObject<IEnumerable<TenantModel>>(stringResponse);
            Assert.That(tenants.Count(), Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public async Task Should_Return_One_Result_With_Id()
        {
            var httpResponse = await _client.GetAsync("/api/tenants/187E5BBF-9F5D-45BD-B8C4-0B894E71DC1F");

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tenant = JsonConvert.DeserializeObject<TenantModel>(stringResponse);

            Assert.That(tenant, Is.TypeOf<TenantModel>());
            Assert.That(tenant, Has.Property("TenantEmails").EqualTo("sanyjose85@gmail.com,sanishtj@gmail.com"));
            Assert.That(tenant, Has.Property("TenantName").EqualTo("Health Record Stack"));
            Assert.That(tenant, Has.Property("TenantPhones").EqualTo("+1 7788708155,+1 7788708048"));
        }

        [Test]
        public async Task Should_Add_item_To_Db()
        {
            Guid newId = new Guid();
            string data = "{\"tenantName\":\"Health Record Stack " + newId + "\",\"tenantEmails\":\"sanyjose56@gmail.com,sanishthtj33@gmail.com\",\"tenantPhones\":\"+1 7788708155,+1 7788708048\"}";
            var httpResponse = await _client.PostAsync("/api/tenants", new StringContent(data, Encoding.UTF8,
                                    "application/json"));

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tenant = JsonConvert.DeserializeObject<TenantModel>(stringResponse);

            Assert.That(tenant, Is.TypeOf<TenantModel>());
            Assert.That(tenant, Has.Property("TenantEmails").EqualTo("sanyjose56@gmail.com,sanishthtj33@gmail.com"));
            Assert.That(tenant, Has.Property("TenantName").EqualTo("Health Record Stack " + newId));
            Assert.That(tenant, Has.Property("TenantPhones").EqualTo("+1 7788708155,+1 7788708048"));
        }

        [Test]
        public async Task Should_Update_item_To_Db()
        {

            string data = "[{\"op\":\"replace\",\"path\":\"/tenantEmails\",\"value\":\"drphilip@gmail.com\"}]";

            var httpResponse = await _client.PatchAsync("/api/tenants/A7BC6C3B-74DE-4F5B-B9C9-94545ABCC6C3", new StringContent(data, Encoding.UTF8,
                                   "application/json-patch+json"));
            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));

            httpResponse = await _client.GetAsync("/api/tenants/A7BC6C3B-74DE-4F5B-B9C9-94545ABCC6C3");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tenant = JsonConvert.DeserializeObject<TenantModel>(stringResponse);

            Assert.That(tenant, Is.TypeOf<TenantModel>());
            Assert.That(tenant, Has.Property("TenantEmails").EqualTo("drphilip@gmail.com"));

        }

        [Test]
        public async Task Should_Delete_item_To_Db()
        {

            var httpResponse = await _client.DeleteAsync("/api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB");
            Assert.That(httpResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));

            httpResponse = await _client.GetAsync("/api/tenants/1183DB51-77B8-4B98-84EE-9FD4C6F2ADFB");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tenant = JsonConvert.DeserializeObject<TenantModel>(stringResponse);

            Assert.That(tenant, Is.TypeOf<TenantModel>());
            Assert.That(tenant, Has.Property("IsDeleted").EqualTo(true));

        }

    }
}
