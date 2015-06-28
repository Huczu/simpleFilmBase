using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleFilmBase.Controllers
{
    public class searchController : Controller
    {
        public class Film
        {
            public int id { get; set; }
            public List<string> genresList { get; set; }
        }
        // GET: search
        [Route("search")]
        public ActionResult Index()
        {
            List<DataModels.genre> genres = GenerateGenres();
            string voteString = Request["vote"];
            string genresString = Request["genres"];
            List<DataModels.genre> selectedGenres = new List<DataModels.genre>();
            DataModels.d8u6uelvine6d6DB database = new DataModels.d8u6uelvine6d6DB();
            if(!String.IsNullOrEmpty(genresString))
            {
                selectedGenres = (from genre in genres
                                  where genresString.Split(',').Contains(genre.id.ToString())
                                  select genre).ToList();
            }
            ViewBag.vote = voteString;
            ViewBag.genres = genres;
            ViewBag.genresSelected = selectedGenres;
            if (!String.IsNullOrEmpty(voteString) && selectedGenres.Count > 0)
            {
                float vote = float.Parse(voteString, CultureInfo.InvariantCulture);
                var selectedGenresIds = (from elem in selectedGenres select elem.id).ToList();
                var movieIdList = (from genre in
                                     (from gen in database.genres where selectedGenresIds.Contains(gen.id) select gen)
                                 join movie_genre in database.movie_genre on genre.id equals movie_genre.genre_id
                                 group movie_genre.moviegenremovieidfkey.id by movie_genre.movie_id into film
                                 where film.Count() == selectedGenres.Count 
                                 select new
                                 {
                                     movie_id = film.Key
                                 }).ToList();
                var moviesList = (from movies in movieIdList
                              join movie in database.movies on movies.movie_id equals movie.id
                              where movie.vote_average > vote
                              orderby movie.vote_average descending
                              select movie).ToList();

                return View(moviesList);
            }
            else if(!String.IsNullOrEmpty(voteString))
            {
                float vote = float.Parse(voteString, CultureInfo.InvariantCulture);
                var moviesList = (from movie in database.movies
                                  where movie.vote_average > vote
                                  select movie).ToList();

                return View(moviesList);
            }
            else if(!String.IsNullOrEmpty(genresString))
            {
                var selectedGenresIds = (from elem in selectedGenres select elem.id).ToList();
                var movieIdList = (from genre in
                                       (from gen in database.genres where selectedGenresIds.Contains(gen.id) select gen)
                                   join movie_genre in database.movie_genre on genre.id equals movie_genre.genre_id
                                   group movie_genre.moviegenremovieidfkey.id by movie_genre.movie_id into film
                                   where film.Count() == selectedGenres.Count
                                   select new
                                   {
                                       movie_id = film.Key
                                   }).ToList();
                var moviesList = (from movies in movieIdList
                                 join movie in database.movies on movies.movie_id equals movie.id
                                 orderby movie.vote_average descending
                                 select movie).ToList();

                return View(moviesList);
            }
            return View();
        }
        private List<DataModels.genre> GenerateGenres()
        {
            DataModels.d8u6uelvine6d6DB database = new DataModels.d8u6uelvine6d6DB();
            var genresList = (from genre in database.genres select genre).ToList();
            return genresList;
        }

    }
}