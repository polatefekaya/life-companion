using LifeCompanion.Domain.Data.DTO.Response;
using LifeCompanion.Domain.Data.Models;
using LifeCompanion.Domain.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System;

namespace LifeCompanion.API.Controllers.v1 {
    public class TestController : CustomBaseController {
        private readonly GeminiOptions _textOnlyGemini;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IOptionsMonitor<GeminiOptions> options, IHttpClientFactory httpClientFactory, IConfiguration configuration) {
            _textOnlyGemini = options.Get(GeminiOptions.TextOnly);
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Test(string message) {
            HttpClient client = _httpClientFactory.CreateClient("textOnlyGemini");
            string requestUri = $"{_textOnlyGemini.ApiVersion}/models/{_textOnlyGemini.Model}:{_textOnlyGemini.RequestType}?key={_configuration["ApiKeys:Gemini"]}";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            //requestMessage.Headers.Add("Content-Type", _textOnlyGemini.ContentType);

            TextOnlyGeminiEntity textOnlyGeminiEntity = new TextOnlyGeminiEntity {
                contents = [
                    new TextOnlyGeminiEntity.Content {
                        parts = [
                            new TextOnlyGeminiEntity.Part {
                                text = message
                            }
                        ]
                    }
                ]
            };

            string content = JsonConvert.SerializeObject(textOnlyGeminiEntity);
            _logger.LogInformation(content);

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _textOnlyGemini.ContentType);
            requestMessage.Content = httpContent;

            
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            _logger.LogInformation(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode) {
                return BadRequest(response);
            }
            string responseContent = await response.Content.ReadAsStringAsync();
            GeminiTestResponse? geminiTestResponse = JsonConvert.DeserializeObject<GeminiTestResponse>(responseContent);
            if (geminiTestResponse is not null ) {
                string responseText = geminiTestResponse.candidates[0].content.parts[0].text;
                return Ok(responseText);
            }

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TestImage(string message, IFormFile file) {
            HttpClient client = _httpClientFactory.CreateClient("textOnlyGemini");
            string requestUri = $"{_textOnlyGemini.ApiVersion}/models/{_textOnlyGemini.Model}:{_textOnlyGemini.RequestType}?key={_configuration["ApiKeys:Gemini"]}";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            //requestMessage.Headers.Add("Content-Type", _textOnlyGemini.ContentType);

            string filePath = Path.GetTempFileName();

            string fileBase64;
            using (MemoryStream memoryStream = new MemoryStream()) {
                await file.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                
                fileBase64 = Convert.ToBase64String(bytes);
            }

            TextAndImageGeminiEntity textAndImageGeminiEntity = new TextAndImageGeminiEntity {
                contents = [
                    new TextAndImageGeminiEntity.Content {
                        parts = [
                            new TextAndImageGeminiEntity.Part {
                                text = message
                                
                            },
                            new TextAndImageGeminiEntity.Part{
                                inline_data = new TextAndImageGeminiEntity.Inline_Data{
                                    mime_type = "image/jpeg",
                                    data = fileBase64
                                }
                            }
                        ]
                    }
                ]
            };

            string content = JsonConvert.SerializeObject(textAndImageGeminiEntity);
            _logger.LogInformation(content);

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _textOnlyGemini.ContentType);
            requestMessage.Content = httpContent;


            HttpResponseMessage response = await client.SendAsync(requestMessage);
            _logger.LogInformation(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode) {
                return BadRequest(response);
            }
            string responseContent = await response.Content.ReadAsStringAsync();
            GeminiTestResponse? geminiTestResponse = JsonConvert.DeserializeObject<GeminiTestResponse>(responseContent);
            if (geminiTestResponse is not null) {
                string responseText = geminiTestResponse.candidates[0].content.parts[0].text;
                return Ok(responseText);
            }

            return Ok(response);
        }
    }
}
