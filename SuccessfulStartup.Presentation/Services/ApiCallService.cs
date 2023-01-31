using SuccessfulStartup.Api.ViewModels;
using System.Net; // for HttpStatusCode
using System.Net.Http.Headers; // for HttpClient
using System.Runtime.CompilerServices; // for InteralsVisibleTo

[assembly: InternalsVisibleTo("SuccessfulStartup.PresentationTests")] // allows tests to access internal members

namespace SuccessfulStartup.Presentation.Services
{
    public class ApiCallService // Static class used for all API calls
    {
        private static HttpClient _client;

        public ApiCallService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7261/"); // TODO: get address through code or connection string
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

            if (response.StatusCode == HttpStatusCode.OK) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{planId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NotFound && response.Content.ReadAsStringAsync().Result.Contains("not found")) { throw new NullReferenceException($"No plan with Id:{planId} could be found."); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match or was deleted previously."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}");  }
        }

        internal async Task<List<BusinessPlanViewModel>> GetAllPlansByAuthorIdAsync(string authorId)
        {
            var response = await _client.GetAsync($"Plan/all/{authorId}");

            if (response.StatusCode == HttpStatusCode.OK) { return await response.Content.ReadFromJsonAsync<List<BusinessPlanViewModel>>(); }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{authorId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match or was deleted previously."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        internal async Task<BusinessPlanViewModel> GetPlanByIdAsync(int planId)
        {
            var response = await _client.GetAsync($"Plan/{planId}");

            if (response.StatusCode == HttpStatusCode.OK) { return await response.Content.ReadFromJsonAsync<BusinessPlanViewModel>(); }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid Id:{planId}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NotFound && response.Content.ReadAsStringAsync().Result.Contains("not found")) { throw new NullReferenceException($"No plan with Id:{planId} could be found."); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match or was deleted previously."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        internal async Task<string> GetUserIdByUsernameAsync(string username)
        {
            /*var response = await _client.GetAsync($"User/{username}");

            if (response.StatusCode == HttpStatusCode.OK) 
            { 
                var authorId = await response.Content.ReadAsStringAsync();
                return authorId.Substring(1, authorId.Length - 2); // original string contains escape character
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid username: {username}. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NotFound && response.Content.ReadAsStringAsync().Result.Contains("not found")) { throw new NullReferenceException($"No user with username: {username} could be found."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}."); }*/

            throw new NullReferenceException($"No user with username: {username} could be found.");
        }

        internal async Task SaveNewPlanAsync(BusinessPlanViewModel plan)
        {
            var response = await _client.PostAsJsonAsync("Plan", plan);

            if (response.StatusCode == HttpStatusCode.Created) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid plan. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

        internal async Task UpdatePlanAsync(BusinessPlanViewModel plan)
        {
            var response = await _client.PutAsJsonAsync("Plan", plan);

            if (response.StatusCode == HttpStatusCode.OK) { return; }
            else if (response.StatusCode == HttpStatusCode.BadRequest && response.Content.ReadAsStringAsync().Result.Contains("null")) { throw new ArgumentNullException($"Invalid plan. Cannot be null"); }
            else if (response.StatusCode == HttpStatusCode.NoContent) { throw new InvalidOperationException("Error writing to the database. Possible that the record did not completely match."); }
            else { throw new Exception($"API request error. Status code: {response.StatusCode}. Details: {response.Content.ReadAsStringAsync()}"); }
        }

    }
}

// var error = response.Content.ReadAsStringAsync().Result; // gets response content for troubleshooting