using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyNewsWeb.Helper
{
    public static class HtmlHelperExtensions
    {
        public static string ImageOrDefault(this HtmlHelper helper, string filename)
        {
            var imagePath = uploadsDirectory + filename;
            var imageSrc = File.Exists(HttpContext.Current.Server.MapPath(imagePath))
                               ? imagePath : defaultImage;

            return imageSrc;
        }

        private static string defaultImage = "/Content/DefaultImage.png";
        private static string uploadsDirectory = "/Content/Uploads/";
    }
}