using System;
using Microsoft.AspNetCore.Mvc;
using publisher.Models;
using publisher.Services;

namespace publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRabbitController : ControllerBase
    {
        private readonly IMessageBrokerService messageBrokerService;

        public TestRabbitController(IMessageBrokerService messageBrokerService)
        {
            this.messageBrokerService = messageBrokerService;
        }
        [HttpGet("{count}")]
        public async Task<IActionResult> SendMessage(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var order = new RegisterUserDto
                {
                    FirstName = $"Ahmad{i}",
                    LastName = $"ha{i}",
                    Role = "Admin",
                    Title = $"Order Number {i}"
                };

                string queueName = "order-queue";
                await messageBrokerService.SendMessage(queueName, order);
            }

            return Ok();
        }
    }
}

