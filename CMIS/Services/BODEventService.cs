using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CMIS.Models;

namespace CMIS.Services
{
    public class BODEventService
    {
        private readonly HttpClient _http;

        public BODEventService(HttpClient http)
        {
            _http = http;
        }

        public List<BODEventModel> Events { get; private set; } = new();

        public async Task LoadEventsAsync()
        {
            Events = await _http.GetFromJsonAsync<List<BODEventModel>>("api/BODEvents") ?? new();
        }

        public async Task AddEventAsync(BODEventModel eventModel)
        {
            var response = await _http.PostAsJsonAsync("api/BODEvents", eventModel);
            if (response.IsSuccessStatusCode)
            {
                await LoadEventsAsync(); // Refresh list after adding
            }
        }

        public async Task UpdateEventAsync(BODEventModel eventModel)
        {
            var response = await _http.PutAsJsonAsync($"api/BODEvents/{eventModel.Id}", eventModel);
            if (response.IsSuccessStatusCode)
            {
                await LoadEventsAsync(); // Refresh list after update
            }
        }

        public async Task<BODEventModel?> GetEventByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<BODEventModel>($"api/BODEvents/{id}");
        }

        public async Task<List<Profile>> GetNationalLeadersAsync()
        {
            return await _http.GetFromJsonAsync<List<Profile>>("api/Leadership/NationalLeaders") ?? new();
        }

        // Legacy compatibility - keeping for now but should use Async versions
        public void AddEvent(BODEventModel eventModel) => AddEventAsync(eventModel).ConfigureAwait(false).GetAwaiter().GetResult();
        public void UpdateEvent(BODEventModel eventModel) => UpdateEventAsync(eventModel).ConfigureAwait(false).GetAwaiter().GetResult();
        public BODEventModel? GetEventById(int id) => GetEventByIdAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();
        public List<Profile> GetNationalLeaders() => GetNationalLeadersAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
