using LL.Application.Common.Interfaces.Services.DataAPIService;
using LL.Application.Common.Interfaces.Services.DBService.Jobs.JobsByOrganizationId;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LL.Infrastructure.Integrations.DataAPI;

public class DataAPIService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly DataAPIConfig _config;
    private readonly ILogger<DataAPIService> _logger;

    public DataAPIService(HttpClient httpClient, DataAPIConfig config, ILogger<DataAPIService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }
    public async Task<List<Job>> GetJobListAsync(string authorizationKey,Guid organizationId)
    {
        try
        {
            _logger.LogInformation($"Getting data  for organization {organizationId}");
            SetDefaultAuthorizationHeader(authorizationKey);

            var response = await _httpClient.GetAsync($"{_config.DataAPIBaseUrl}/api/jobs?organizationId ={organizationId}");
            if (!response.IsSuccessStatusCode)
                return new List<Job>();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Job>>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Job retrival failed , {organizationId}",ex);
            throw;
        }
    }
    private void SetDefaultAuthorizationHeader(string authorizationKey)
    {
        if (_httpClient.DefaultRequestHeaders.Authorization is null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", $"{authorizationKey}");
    }
}
