using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyNewsWeb.Models
{
    public class NewsIndexViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string NewsType { get; set; }
        public int UserInfoId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NewsDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}