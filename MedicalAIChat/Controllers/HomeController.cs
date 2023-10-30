using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MedicalAIChat.Models;
using Azure.AI.OpenAI;
using Newtonsoft.Json;

namespace MedicalAIChat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost("api/medical-diagnosis")]
    public async Task<IActionResult> MedicalDiagnosisAsync([FromBody] string prompt)
    {
        var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
        var client = new OpenAIClient(apiKey);

        ChatCompletionsOptions options = new(new List<ChatMessage>
        {
            new(ChatRole.User, prompt),
        });

        var completion = await client.GetChatCompletionsAsync("gpt-3.5-turbo", options);

        ChatCompletions a = completion.Value;

        var jsonResponse = JsonConvert.SerializeObject(completion.Value);

        return Content(jsonResponse, "application/json");
    }
}


