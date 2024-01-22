using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Account
{
    public class UserModel
    {
        [Required]
        [StringLength(128)]
        public string user_id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        public string avatar { get; set; }
        public DateTime dateJoin { get; set; }
    }
    public class UserHeaderModel
    {
        [Required]
        [StringLength(128)]
        public string user_id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        public string avatar { get; set; }
        public int total_follow { get; set; }
        public int total_notify { get; set; }
        public UserHeaderModel()
        {
            user_id = "";
            name = "";
            avatar = "";
            total_follow = 0;
            total_notify = 0;
        }
    }
}