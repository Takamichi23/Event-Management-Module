using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CMIS.Models;

namespace CMIS.Services
{
    public class EventService
    {
        private readonly HttpClient _http;

        public EventService(HttpClient http)
        {
            _http = http;
        }

        public List<EventModel> Events { get; private set; } = new();

        public async Task LoadEventsAsync()
        {
            Events = await _http.GetFromJsonAsync<List<EventModel>>("api/Events") ?? new();
        }

        public async Task AddEventAsync(EventModel eventModel)
        {
            var response = await _http.PostAsJsonAsync("api/Events", eventModel);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP {response.StatusCode}: {error}");
            }
        }

        public async Task UpdateEventAsync(EventModel eventModel)
        {
            await _http.PutAsJsonAsync($"api/Events/{eventModel.Id}", eventModel);
        }

        public async Task<EventModel?> GetEventByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<EventModel>($"api/Events/{id}");
        }

        // Legacy compatibility
        public void AddEvent(EventModel eventModel) => AddEventAsync(eventModel).GetAwaiter().GetResult();
        public void UpdateEvent(EventModel eventModel) => UpdateEventAsync(eventModel).GetAwaiter().GetResult();
    }
}
