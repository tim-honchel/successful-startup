using SuccessfulStartup.Api.ViewModels;
using System.Net.Http.Headers; // for HttpClient
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SuccessfulStartup.PresentationTests")] // allows tests to access internal members

namespace SuccessfulStartup.Presentation.Services
{
    public class ApiCallService // Static class used for all API calls
    {
        private static HttpClient _client;

        public ApiCallService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7261/");
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // allows JSON transmission
        }

        public ApiCallService(HttpMessageHandler handler) // for unit testing, to allow mocking
        {
            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri("https://localhost:7260/"); // edited to be fake
        }

        internal async Task DeletePlanAsync(int planId)
        {
            var response = await _client.DeleteAsync($"Plan/{planId}");
            response.EnsureSuccessStatusCode();
        }

        internal async Task<List<BusinessPlanViewModel>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            var response = await _client.GetAsync($"Plan/all/{authorId}");
            var plans = await response.Content.ReadFromJsonAsync<List<BusinessPlanViewModel>>();
            return plans;
        }

        internal async Task<BusinessPlanViewModel> GetPlanByIdAsync(int planId)
        {
            var response = await _client.GetAsync($"Plan/{planId}");
            var plan = await response.Content.ReadFromJsonAsync<BusinessPlanViewModel>();
            return plan;
        }

        internal async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var response = await _client.GetAsync($"User/{username}");
            var authorId = await response.Content.ReadAsStringAsync();
            authorId = authorId.Substring(1, authorId.Length - 2); // original string contains escape characters
            return authorId;
        }

        internal async Task SaveNewPlanAsync(BusinessPlanViewModel plan) // TODO: not working, using temporary workaround
        {
            var response = await _client.PostAsJsonAsync("Plan", plan);
        }

        internal async Task UpdatePlanAsync(BusinessPlanViewModel plan) // TODO: not working, using temporary workaround
        {
            var response = await _client.PutAsJsonAsync("Plan", plan);
        }

    }
}

// var error = response.Content.ReadAsStringAsync().Result; // gets response content for troubleshooting