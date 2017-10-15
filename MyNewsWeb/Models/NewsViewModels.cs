using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyNewsWeb.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(30)]
        [Display(Name = "News Image")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "News Type")]
        public string NewsType { get; set; }

        [Display(Name = "Author")]
        public int UserInfoId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NewsDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public IDictionary<int, string> Authors { get; set; }
        public IDictionary<string, string> NewsTypes { get; set; }
    }

    public class NewsUserCommentsViewModel : NewsViewModel
    {
        public CommentViewModel[] Comments { get; set; }
    }

    public class CommentViewModel
    {
        public string Comment { get; set; }
    }
}