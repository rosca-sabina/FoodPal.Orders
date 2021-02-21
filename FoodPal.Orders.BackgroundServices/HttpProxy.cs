using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodPal.Orders.BackgroundServices
{
    internal class HttpProxy
    {
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("Accept", "application/json");

            try
            {
                using(var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(900) })
                {
                    var responseMessage = await httpClient.SendAsync(requestMessage);
                    return await HandleResponseMessage<TResponse>(responseMessage);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Error occured while sending POST request to endpoint {endpoint}.", ex);
            }
        }

        private async Task<TResponse> HandleResponseMessage<TResponse>(HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                var httpErrorMessage = await HandleUnsuccessfulResponseMessage(message);
                throw new Exception($"Http request failed with response content: {httpErrorMessage}.");
            }

            try
            {
                var content = await message.Content.ReadAsStringAsync();
                var responseObject = !string.IsNullOrEmpty(content) ? JsonConvert.DeserializeObject<TResponse>(content) : default;
                return responseObject;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to read and deserialze response message", ex);
            }
        }
        private async Task<string> HandleUnsuccessfulResponseMessage(HttpResponseMessage responseMessage)
        {
            try
            {
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read and deserialize HTTP unsuccessful response message.", ex);
            }
        }
    }
}
