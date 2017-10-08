using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyNewsWeb.Models
{
    public  class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public string IdentityId { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [StringLength(20)]
        public string Suburb { get; set; }

        [StringLength(10)]
        public string State { get; set; }

        [StringLength(10)]
        public string PostCode { get; set; }

        [StringLength(30)]
        public string FileName { get; set; }
        
        public virtual ICollection<GoodNew> GoodNews { get; set; }
    }

    public class GoodNew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int UserInfoId { get; set; }

        public string Content { get; set; }
        [StringLength(20)]
        public string NewsType { get; set; }
        [StringLength(30)]
        public string FileName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NewsDate { get; set; }

        [ForeignKey("UserInfoId")]      // add here
        public virtual UserInfo UserInfo { set; get; }
    }
}