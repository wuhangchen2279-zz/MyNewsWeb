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
                               ? imagePath : defaultProfileImg;

            return imageSrc;
        }

        public static string ImageOrDefaultNews(this HtmlHelper helper, string filename)
        {
            var imagePath = uploadsDirectory + filename;
            var imageSrc = File.Exists(HttpContext.Current.Server.MapPath(imagePath))
                               ? imagePath : defaultNewsImg;

            return imageSrc;
        }

        private static string defaultNewsImg = "/Content/DefaultNewsImage.png";
        private static string defaultProfileImg = "/Content/DefaultProfileImage.png";
        private static string uploadsDirectory = "/Content/Uploads/";
    }
}