using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CMIS.Models;

namespace CMIS.Services
{
    public class DistrictEventService
    {
        private readonly HttpClient _http;

        public DistrictEventService(HttpClient http)
        {
            _http = http;
        }

        public List<DistrictEventModel> Events { get; private set; } = new();

        public async Task LoadEventsAsync()
        {
            Events = await _http.GetFromJsonAsync<List<DistrictEventModel>>("api/DistrictEvents") ?? new();
        }

        public async Task AddEventAsync(DistrictEventModel eventModel)
        {
            var response = await _http.PostAsJsonAsync("api/DistrictEvents", eventModel);
            if (response.IsSuccessStatusCode)
            {
                await LoadEventsAsync(); // Refresh list after adding
            }
        }

        public async Task UpdateEventAsync(DistrictEventModel eventModel)
        {
            var response = await _http.PutAsJsonAsync($"api/DistrictEvents/{eventModel.Id}", eventModel);
            if (response.IsSuccessStatusCode)
            {
                await LoadEventsAsync(); // Refresh list after update
            }
        }

        public async Task<DistrictEventModel?> GetEventByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DistrictEventModel>($"api/DistrictEvents/{id}");
        }

        public async Task<List<Profile>> GetDistrictLeadersAsync()
        {
            return await _http.GetFromJsonAsync<List<Profile>>("api/Leadership/DistrictLeaders") ?? new();
        }

        public async Task<List<Profile>> GetAllMembersAsync()
        {
            return await _http.GetFromJsonAsync<List<Profile>>("api/Leadership") ?? new();
        }

        // Legacy compatibility
        public void AddEvent(DistrictEventModel eventModel) => AddEventAsync(eventModel).ConfigureAwait(false).GetAwaiter().GetResult();
        public void UpdateEvent(DistrictEventModel eventModel) => UpdateEventAsync(eventModel).ConfigureAwait(false).GetAwaiter().GetResult();
        public List<Profile> GetDistrictLeaders() => GetDistrictLeadersAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        public List<Profile> GetAllMembers() => GetAllMembersAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
