using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simpleFilmBase.Controllers
{
    public class HomeController : Controller
    {
        public List<DataModels.movie> GetTopMoviesRanking()
        {
            DataModels.d8u6uelvine6d6DB database = new DataModels.d8u6uelvine6d6DB();
            List<DataModels.movie> moviesList = (from movie in database.movies orderby movie.vote_average descending, movie.release_date ascending select movie).Take(20).ToList();
            return moviesList;                                  
        }

        public ActionResult Index()
        {
            return View(GetTopMoviesRanking());
        }
        
    }
}