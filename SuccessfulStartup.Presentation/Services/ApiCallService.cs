using SuccessfulStartup.Api.ViewModels;
using System.Net.Http.Headers; // for HttpClient

namespace SuccessfulStartup.Presentation.Services
{
    public static class ApiCallService // Static class used for all API calls
    {
        private static HttpClient _client = new HttpClient();
        private static bool _isSetup = false;

        internal static void SetupClient()
        {
            if (_isSetup == false)
            {
                _client.BaseAddress = new Uri("https://localhost:7261/");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // allows JSON transmission
                _isSetup = true; // ensures method only runs once, prior to the first API call
            }
        }

        internal static async Task DeletePlan(int planId)
        {
            SetupClient();
            var response = await _client.DeleteAsync($"Plan/{planId}");
            response.EnsureSuccessStatusCode();
        }

        internal static async Task<List<BusinessPlanViewModel>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            SetupClient();
            var response = await _client.GetAsync($"Plan/all/{authorId}");
            var plans = await response.Content.ReadFromJsonAsync<List<BusinessPlanViewModel>>();
            return plans;
        }

        internal static async Task<BusinessPlanViewModel> GetPlanByIdAsync(int planId)
        {
            SetupClient();
            var response = await _client.GetAsync($"Plan/{planId}");
            var plan = await response.Content.ReadFromJsonAsync<BusinessPlanViewModel>();
            return plan;
        }

        internal static async Task<string> GetUserIdByUsernameAsync(string username)
        {
            SetupClient();
            var response = await _client.GetAsync($"User/{username}");
            var authorId = await response.Content.ReadAsStringAsync();
            authorId = authorId.Substring(1, authorId.Length - 2); // original string contains escape characters
            return authorId;
        }


        internal static async Task SaveNewPlanAsync(BusinessPlanViewModel plan) // TODO: not working, using temporary workaround
        {
            SetupClient();
            var response = await _client.PostAsJsonAsync("Plan", plan);
        }

        internal static async Task UpdatePlanAsync(BusinessPlanViewModel plan) // TODO: not working, using temporary workaround
        {
            SetupClient();
            var response = await _client.PutAsJsonAsync("Plan", plan);
        }

    }
}

// var error = response.Content.ReadAsStringAsync().Result; // gets response content for troubleshooting