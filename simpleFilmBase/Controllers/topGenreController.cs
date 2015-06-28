using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace simpleFilmBase.Controllers
{
    public class topGenreController : Controller
    {
        // GET: topGenre
        [Route("topGenre")]
        public ActionResult Index() //TODO: fixit
        {
            return View();
        }

        public ActionResult GenerateChart()
        {
            DataModels.d8u6uelvine6d6DB database = new DataModels.d8u6uelvine6d6DB();
            var genresList = (from movie_genre in database.movie_genre
                              join genre in database.genres on movie_genre.genre_id equals genre.id
                              group movie_genre.genre_id by genre.name into genres
                              select new
                              {
                                  genreName = genres.Key,
                                  genreCount = genres.Count()

                              });

            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(new Series("Data"));
            chart.Legends.Add(new Legend("Genres"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"]["PieLineColor"] = "Black";
            chart.Width = 640;
            chart.Height = 480;
            foreach (var elem in genresList)
            {
                int point = chart.Series["Data"].Points.AddXY(elem.genreName, elem.genreCount);
                DataPoint pt = chart.Series["Data"].Points[point];
                pt.LegendText = "#VALX: #VALY";
            }

            chart.Series["Data"].Label = "#PERCENT{P0}";
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            chart.Series["Data"]["PieLabelStyle"] = "Outside";
            chart.Series["Data"].Legend = "Genres";
            chart.Legends["Genres"].Docking = Docking.Bottom;

            var returnStream = new MemoryStream();
            chart.ImageType = ChartImageType.Png;
            chart.SaveImage(returnStream);
            returnStream.Position = 0;
            return new FileStreamResult(returnStream, "image/png");
        }
    }
}