using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using otel_advisor_webApp.DTO;
using otel_advisor_webApp.Models;
using Microsoft.Extensions.Logging;

namespace otel_advisor_webApp.Services
{
    public class RecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<RecommendationService> _logger;

        public RecommendationService(HttpClient httpClient, string baseUrl, ILogger<RecommendationService> logger)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _logger = logger;
        }

        public async Task<List<HotelDto>> GetRecommendations(RecommendationRequestDto request)
        {
            try
            {
                _logger.LogInformation($"Sending request to Python API. BaseUrl: {_baseUrl}");
                
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                
                var json = JsonSerializer.Serialize(request, jsonOptions);
                _logger.LogInformation($"Sent JSON: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/recommend/", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response: Status: {response.StatusCode}, Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API request failed: {response.StatusCode}, Error: {responseContent}");
                    throw new Exception($"API request failed: {response.StatusCode}, Error: {responseContent}");
                }

                var recommendations = JsonSerializer.Deserialize<List<HotelDto>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (recommendations == null || recommendations.Count == 0)
                {
                    _logger.LogWarning("No hotel recommendations received from API.");
                    return new List<HotelDto>();
                }

                _logger.LogInformation($"Successfully received {recommendations.Count} hotel recommendations.");
                return recommendations;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw new Exception($"Error connecting to API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON conversion error: {ex.Message}");
                throw new Exception($"Error processing API response: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw new Exception($"An error occurred while getting hotel recommendations: {ex.Message}", ex);
            }
        }
    }
}