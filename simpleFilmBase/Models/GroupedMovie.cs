using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace simpleFilmBase.Models
{
    public class GroupedMovie
    {
        public string title { get; set; }
        public string original_title { get; set; }
        public float? movie_average { get; set; }
        public List<string> genre { get; set; }
        public string desc { get; set; }
    }
}