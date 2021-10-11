using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using twelve_factor_app.Models;

namespace twelve_factor_app.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly HttpClient _httpClient;

        public UserService(ILogger<UserService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> getUsersFromRemote(int count = 1000)
        {
            string serviceUrl = string.Format("https://api.mockaroo.com/api/6351d820?count={0}", count);
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", "2118bfe0");
            var response = await _httpClient.GetAsync(serviceUrl);

            _logger.LogInformation("GET /api/6351d820?count={0} : HTTP STATUS {1} : {2}"
                                , count, response.StatusCode, response.ReasonPhrase);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var userList = JsonSerializer.Deserialize<IEnumerable<User>>(responseJson);
                _logger.LogInformation("GET /api/6351d820?count={0} : Count retrieved : {1}", count, userList.Count());

                return userList;
            }
            else
                return new List<User>();
        }

        public IEnumerable<User> makeUsersSendToRemote(int countToSend = 1)
        {
            _logger.LogInformation("make called");
            var usersList = this.getUsersFromRemote(countToSend).Result;
            var usersListReturn = new List<User>();
            foreach (var item in usersList)
            {
                yield return sendPostToUser(item).Result;
            }
        }

        private async Task<User> sendPostToUser(User item)
        {
            string serviceUrl = string.Format("https://jsonplaceholder.typicode.com/posts");

            var post = new Post()
            {
                body = "A post body",
                title = "A post title",
                userId = item.Id
            };
            var content = new StringContent(JsonSerializer.Serialize(post), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(serviceUrl, content);

            _logger.LogInformation("POST {0} : HTTP STATUS {1} : {2}", serviceUrl, response.StatusCode, response.ReasonPhrase);

            if (response.IsSuccessStatusCode)
            {
                item.post = post;
                return item;
            }
            else
                return null;
        }
    }
}