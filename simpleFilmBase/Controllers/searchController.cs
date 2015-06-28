using System;
using System.Collections.Generic;
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
                float vote = float.Parse(voteString);
                /*List<DataModels.movie> moviesList = (from movie in database.movies
                                                     join movie_genre in database.movie_genre on movie.id equals movie_genre.movie_id into firstIds
                                                     from movieGenreId in firstIds.DefaultIfEmpty()
                                                     join genre in database.genres on movieGenreId.genre_id equals genre.id
                                                     where movie.vote_average > vote
                                                     select movie).ToList(); //more joins!*/
                var movieGenreList = (from movie in database.movies
                                      join movie_genre in database.movie_genre on movie.id equals movie_genre.movie_id into firstIds
                                      from movieGenreId in firstIds.DefaultIfEmpty()
                                      join genre in database.genres on movieGenreId.genre_id equals genre.id
                                      where movie.vote_average > vote
                                      group genre by movie.title into grupa
                                      where selectedGenres.All(x => grupa.Contains(x))
                                      //where selectedGenres.All(x => grupa.Contains(x))
                                      select new Film()
                                      {
                                          id = int.Parse(grupa.Key.ToString()),
                                          genresList = (from elem in grupa select elem.name).ToList()
                                      }
                                      ).ToList();

                return View();
            }
            else if(!String.IsNullOrEmpty(voteString))
            {
                float vote = float.Parse(voteString);
                var moviesList = (from movie in database.movies
                                  where movie.vote_average > vote
                                  select movie).ToList();

                return View(moviesList);
            }
            else if(!String.IsNullOrEmpty(genresString))
            {
                var movieGenreList = (from movie in database.movies
                                      join movie_genre in database.movie_genre on movie.id equals movie_genre.movie_id into firstIds
                                      from movieGenreId in firstIds.DefaultIfEmpty()
                                      join genre in database.genres on movieGenreId.genre_id equals genre.id
                                      group genre by movie.id into grupa
                                      where selectedGenres.All(x => grupa.Contains(x))
                                      //where selectedGenres.All(x => grupa.Contains(x))
                                      select new Film()
                                      {
                                          id = int.Parse(grupa.Key.ToString()),
                                          genresList = (from elem in grupa select elem.name).ToList()
                                      }
                                      ).ToList();

                return View();
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