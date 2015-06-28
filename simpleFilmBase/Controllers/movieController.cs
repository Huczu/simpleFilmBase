using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using simpleFilmBase.Models;

namespace simpleFilmBase.Controllers
{
    [RoutePrefix("movie")]
    public class movieController : Controller
    {
        // GET: movie
        public string GetDescription(string title)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.omdbapi.com/?plot=full&t=" + title );
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    dynamic deserialize = JsonConvert.DeserializeObject(reader.ReadToEnd());
                    if (deserialize.Plot != null)
                        return deserialize.Plot;
                    else
                        return "No description";
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                throw;
            }
        }
        [Route("{id:int}")]
        public ActionResult movie(int id)
        {
            DataModels.d8u6uelvine6d6DB database = new DataModels.d8u6uelvine6d6DB();
            
            var singleMovie = (from movie in database.movies
                               where movie.id == id
                               select new GroupedMovie()
                               {
                                   title = movie.title,
                                   original_title = movie.original_title,
                                   movie_average = movie.vote_average
                               }).FirstOrDefault();

            var genreList = (from genre in database.genres
                             join movie_genre in database.movie_genre on genre.id equals movie_genre.genre_id into firstIds
                             from genreId in firstIds.DefaultIfEmpty()
                             where genreId.movie_id == id
                             select genre.name).ToList();

            singleMovie.genre = new List<string>();
            foreach (string elem in genreList)
                singleMovie.genre.Add(elem);
            
            singleMovie.desc = GetDescription(singleMovie.title);

            return View(singleMovie);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}