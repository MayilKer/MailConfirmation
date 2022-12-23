using EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailComfiramitonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IEmailSender _emailSender;

        public WeatherForecastController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            EmailConfiguration email = new EmailConfiguration();
            string token = Guid.NewGuid().ToString();
            var link = Url.Action(nameof(VerifyEmail), "WeatherForecast", new { token }, Request.Scheme, Request.Host.ToString());
            email.Address = "kerimov0410@gmail.com";
            email.DisplayName = "Johns Johnson";
            List<EmailConfiguration> emailConfigurations = new List<EmailConfiguration>();
            emailConfigurations.Add(email);
            var message = new Message(emailConfigurations, "Test email", link);
            
            _emailSender.SendEmail(message);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("{token}")]
        public IActionResult VerifyEmail(string token)
        {
            if (token == null) return NotFound();

            return Ok("Veryfied");
        }
    }
}
