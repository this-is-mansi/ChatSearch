using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("chat/ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var claimService = new ClaimService();

            string context = "";

            // detect claim id from message
            var words = request.Message.Split(" ");

            foreach (var word in words)
            {
                if (int.TryParse(word, out int claimId))
                {
                    var claim = claimService.GetClaim(claimId);

                    if (claim != null)
                    {
                        context = $"""
                Claim Details:
                Claim Id: {claim.Id}
                Customer: {claim.Customer}
                Status: {claim.Status}
                Amount: {claim.Amount}
                """;
                    }
                }
            }

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer myKey");

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new { role = "system", content = "You are an assistant for an insurance claims management system." },
            new { role = "user", content = $"Context:\n{context}\n\nUser question:\n{request.Message}" }
        }
            };

            var response = await client.PostAsJsonAsync(
                "https://models.inference.ai.azure.com/chat/completions",
                body
            );

            var json = await response.Content.ReadAsStringAsync();

            var doc = System.Text.Json.JsonDocument.Parse(json);

            try
            {
                var reply = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return Ok(reply);
            }
            catch
            {
                return Ok("Error reading AI response.");
            }
        }
    

        public class ChatRequest
    {
        public string Message { get; set; }
    }

        [HttpPost("investment/search")]
        public IActionResult SearchInvestment([FromBody] ChatRequest request)
        {
            var investmentService = new InvestmentService();

            int budget = 0;

            var words = request.Message.Split(" ");

            foreach (var word in words)
            {
                if (int.TryParse(word, out int amount))
                {
                    budget = amount;
                }
            }

            var plans = investmentService.GetPlans(budget);

            if (plans.Count == 0)
                return Ok("No plans found for this budget.");

            var result = "Recommended Investment Plans:\n";

            foreach (var plan in plans)
            {
                result += $"- {plan.Name} (Min ₹{plan.MinAmount}, Risk: {plan.Risk})\n";
            }

            return Ok(result);
        }
    }
}