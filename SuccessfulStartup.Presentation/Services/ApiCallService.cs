using SuccessfulStartup.Data.APIs;
using SuccessfulStartup.Data.Entities;
using SuccessfulStartup.Data.Mapping;
using System.Net.Http.Headers;

namespace SuccessfulStartup.Presentation.Services
{
    public static class ApiCallService // Static class used for all API calls
    {
        private static HttpClient _client = new HttpClient();
        private static bool _isSetup = false;

        private static WriteOnlyApi _writeOnlyApi = new WriteOnlyApi(new Data.Contexts.AuthenticationDbContextFactory(), AllMappingProfiles.GetMapper()); // temporary workaround
        private static EntityConverter _entityConverter = new EntityConverter(AllMappingProfiles.GetMapper()); // temporary workaround

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

        internal static async Task<List<BusinessPlan>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            SetupClient();
            var response = await _client.GetAsync($"Plan/all/{authorId}");
            var plans = await response.Content.ReadFromJsonAsync<List<BusinessPlan>>();
            return plans;
        }

        internal static async Task<BusinessPlan> GetPlanByIdAsync(int planId)
        {
            SetupClient();
            var response = await _client.GetAsync($"Plan/{planId}");
            var plan = await response.Content.ReadFromJsonAsync<BusinessPlan>();
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


        internal static async Task SaveNewPlanAsync(BusinessPlan plan) // TODO: not working, using temporary workaround
        {
            SetupClient();
            await _writeOnlyApi.SaveNewPlan(_entityConverter.Convert(plan));
            //var response = await _client.PostAsJsonAsync("Plan", plan);
            //response.EnsureSuccessStatusCode();
        }

        internal static async Task UpdatePlanAsync(BusinessPlan plan) // TODO: not working, using temporary workaround
        {
            SetupClient();
            await _writeOnlyApi.UpdatePlan(_entityConverter.Convert(plan));
            //var response = await _client.PutAsJsonAsync("Plan", plan);
            //response.EnsureSuccessStatusCode();
        }

    }
}
