using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhotoAlbum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoAlbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumCollectionController : ControllerBase
    {

        private readonly ILogger<AlbumCollectionController> _logger;

        public AlbumCollectionController(ILogger<AlbumCollectionController> logger)
        {
            _logger = logger;
        }
        // GET: api/<AlbumCollectionController>
        [HttpGet]
        public async Task<IEnumerable<Album>> Get()
        {
            try
            {
                var albums = await GetResponseAsync<Album>("http://jsonplaceholder.typicode.com/albums");
                var photos = await GetResponseAsync<Photo>("http://jsonplaceholder.typicode.com/photos");
                if (albums != null)
                {
                    albums.ForEach((album) =>
                    {
                        album.Photos = photos.FindAll(photo => photo.AlbumId == album.Id);
                    });
                }
                return albums;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IEnumerable<Album>> Get(int id)
        {
            try
            {
                var albums = await GetResponseAsync<Album>($"http://jsonplaceholder.typicode.com/users/{id}/albums");
                if (albums != null)
                {
                    albums.ToList().ForEach((album) =>
                    {
                        album.Photos = GetResponseAsync<Photo>($"http://jsonplaceholder.typicode.com/albums/{album.Id}/photos").Result;
                    });
                }
                return albums;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Response.StatusCode = 500;
                return null;
            }
        }

        private async Task<List<T>> GetResponseAsync<T>(string url)
        {
            var result = new List<T>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var obj = response.Content.ReadAsStringAsync().Result;
                        result = JsonConvert.DeserializeObject<List<T>>(obj);
                    }
                    else
                    {
                        _logger.LogError(response.ReasonPhrase);
                        throw new HttpRequestException($"Api call failed :: endpont - {url}");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
