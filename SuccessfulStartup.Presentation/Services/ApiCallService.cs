using Microsoft.AspNetCore.Components.Authorization; // for AuthenticationStateProvider
using SuccessfulStartup.Api.ViewModels;
using System.Net; // for HttpStatusCode
using System.Net.Http.Headers; // for HttpClient
using System.Runtime.CompilerServices; // for InteralsVisibleTo

[assembly: InternalsVisibleTo("SuccessfulStartup.PresentationTests")] // allows tests to access internal members

namespace SuccessfulStartup.Presentation.Services
{
    public class ApiCallService : IApiCallService // helper class used for all API calls
    {
        private static HttpClient _client;
        private readonly AuthenticationStateProvider _provider;
        private readonly string _securityStamp;

        public ApiCallService(AuthenticationStateProvider provider)
        {
            _provider = provider;
            try
            {
                _securityStamp = provider.GetAuthenticationStateAsync().Result.User.Claims.FirstOrDefault(claim => claim.Type == "AspNet.Identity.SecurityStamp").Value;
            }
            catch (Exception) // for example, if no user is logged in
            {
                _securityStamp = "";
            }
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7261/"); // TODO: get address through code or connection string
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // allows JSON transmission
            _client.DefaultRequestHeaders.Add("key", _securityStamp);
            Console.WriteLine();
        }

        public ApiCallService(HttpMessageHandler handler) // for unit testing, to allow mocking
        {
            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri("https://localhost:7260/"); // edited to be fake
        }

        public async Task DeletePlanAsync(int planId) // must be public to implement interface
        {
            var response = await _client.DeleteAsync($"Plan/{planId}");

            if (response.StatusCode == HttpStatusCode.OK) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{planId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NotFound && response.Content.ReadAsStringAsync().Result.Contains("not found")) { throw new NullReferenceException($"No plan with Id:{planId} could be found."); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match or was deleted previously."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("Lack authorization to delete the plan."); }
            else if (response.StatusCode == HttpStatusCode.Forbidden) { throw new InvalidOperationException("Must provide credentials to delete plans."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        public async Task<List<BusinessPlanViewModel>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            var response = await _client.GetAsync($"Plan/all/{authorId}");

            if (response.StatusCode == HttpStatusCode.OK) { return await response.Content.ReadFromJsonAsync<List<BusinessPlanViewModel>>(); }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{authorId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match or was deleted previously."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("Lack authorization to view the plans."); }
            else if (response.StatusCode == HttpStatusCode.Forbidden) { throw new InvalidOperationException("Must provide credentials to view plans."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        public async Task<BusinessPlanViewModel> GetPlanByIdAsync(int planId)
        {
            var response = await _client.GetAsync($"Plan/{planId}");

            if (response.StatusCode == HttpStatusCode.OK) { return await response.Content.ReadFromJsonAsync<BusinessPlanViewModel>(); }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{planId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NotFound && response.Content.ReadAsStringAsync().Result.Contains("not found")) { throw new NullReferenceException($"No plan with Id:{planId} could be found."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("Lack authorization to view the plan."); }
            else if (response.StatusCode == HttpStatusCode.Forbidden) { throw new InvalidOperationException("Must provide credentials to view plans."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        public async Task<int> SaveNewPlanAsync(BusinessPlanViewModel plan)
        {
            var response = await _client.PostAsJsonAsync("Plan", plan);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var newId = response.Content.ReadAsStringAsync().Result;
                return Convert.ToInt32(newId);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid plan. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("Lack authorization to save the plan."); }
            else if (response.StatusCode == HttpStatusCode.Forbidden) { throw new InvalidOperationException("Must provide credentials to save plans."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        public async Task SaveNewUserAsync(string userId, string securityStamp)
        {
            var user = new Dictionary<string, string>() { { "userId", userId }, { "securityStamp",securityStamp } };
            var response = await _client.PostAsJsonAsync("User",user);
            if (response.StatusCode == HttpStatusCode.OK) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid user. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("User already exists."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }


        public async Task UpdatePlanAsync(BusinessPlanViewModel plan)
        {
            var response = await _client.PutAsJsonAsync("Plan", plan);

            if (response.StatusCode == HttpStatusCode.OK) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid plan. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match."); }
            else if (response.StatusCode == HttpStatusCode.Unauthorized) { throw new InvalidOperationException("Lack authorization to update the plan."); }
            else if (response.StatusCode == HttpStatusCode.Forbidden) { throw new InvalidOperationException("Must provide credentials to update plans."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

    }
}

// var error = response.Content.ReadAsStringAsync().Result; // gets response content for troubleshooting