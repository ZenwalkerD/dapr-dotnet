using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Dapr.Controllers
{
    [ApiController]
    public class HelloController : ControllerBase
    {
        IConfiguration m_Configuration;
        DaprClient m_DaprClient;

        public HelloController(IConfiguration configuration, DaprClient daprClient)
        {
            m_Configuration = configuration;
            m_DaprClient = daprClient;
        }

        // GET: api/<HelloController>
        [HttpGet]
        [Route("/api/external")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://reqres.in/api/users?page=1");
                //var response = await m_DaprClient.InvokeMethodWithResponseAsync(httpRequestMessage);
                //var result = await response.Content.ReadAsStringAsync();

                var client = DaprClient.CreateInvokeHttpClient("https://reqres.in");
                var response = client.GetAsync(client.BaseAddress + "api/users?page=1").Result;
                var result = response.Content.ReadAsStringAsync().Result;


                //var client = DaprClient.CreateInvokeHttpClient("https://reqres.in");
                //var response = client.GetAsync(client.BaseAddress + "api/users?page=1").Result;
                //var result = response.Content.ReadAsStringAsync().Result;
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            //return Ok(JsonSerializer.Serialize(m_Configuration.AsEnumerable()));
        }

        [HttpPost]
        [Route("/api/set")]
        public async Task<IActionResult> Set([FromBody] string value)
        {
            //using var client = new DaprClientBuilder().Build();
            //client.SaveBulkSecretAsync("keyvault", new Dictionary<string, string> { { "key", value } }).Wait();

            var req = m_DaprClient.CreateInvokeMethodRequest<string>("backendapi", "api/hi", value);

            var result = await m_DaprClient.InvokeMethodWithResponseAsync(req);
            var content = await result.Content.ReadAsStringAsync();

            return Ok(content);
        }

        [HttpGet]
        [Route("/api/getSecrets")]
        public async Task<IActionResult> Secrets()
        {
            var result = await m_DaprClient.GetBulkSecretAsync("keyvault");
            Console.WriteLine(JsonConvert.SerializeObject(result));

            return Ok(result);
        }
    }
}
