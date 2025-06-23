using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using KorteBroeken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KorteBroeken.Controllers
{
    public class ApiController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiController> _logger;

        public ApiController(IHttpClientFactory httpClientFactory, ILogger<ApiController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [Route("KorteBroek/Api")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Gebruiken van Open-Meteo gratis weer API voor Eindhoven
                var response = await client.GetAsync(
                    "https://api.open-meteo.com/v1/forecast?latitude=51.44&longitude=5.47&current=temperature_2m,precipitation_probability&timezone=Europe%2FAmsterdam");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<JsonElement>(content);

                    // Verkrijg de huidige temperatuur en regenkans
                    var temperatuur = (int)Math.Round(weatherData.GetProperty("current").GetProperty("temperature_2m").GetDouble());
                    var regenkans = weatherData.GetProperty("current").GetProperty("precipitation_probability").GetInt32();

                    var model = new WeerModel
                    {
                        Temperatuur = temperatuur,
                        Regenkans = regenkans
                    };

                    // Bepaal of een korte broek gedragen kan worden
                    if (temperatuur > 20 && regenkans < 50)
                    {
                        model.KanKorteBroekAan = true;
                        model.Bericht = "Ja! je kunt vandaag een korte broek aan";
                    }
                    else
                    {
                        model.KanKorteBroekAan = false;
                        model.Bericht = "Nee, je kunt vandaag helaas geen korte broek aan";
                    }

                    return View("ApiResult", model);
                }
                else
                {
                    _logger.LogError($"API request failed with status code {response.StatusCode}");
                    ViewBag.Error = "Er ging iets mis bij het ophalen van de weergegevens.";
                    return View("ApiResult", new WeerModel());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                ViewBag.Error = "Er is een fout opgetreden bij het ophalen van de weergegevens.";
                return View("ApiResult", new WeerModel());
            }
        }
    }
}