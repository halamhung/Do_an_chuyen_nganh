using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HKQTravellingAuthenication.Models.Blog
{
    [Table("PostCategory")]
    public class PostCategory //model trung gian
    {
        //ở đây ko dùng key để xác định khóa chính mà thiết lập trong db để tạo ra bảng từ 2 khóa postid và catepost
        public int PostID { set; get; } //khóa ngoại tham chiếu 

        public int CategoryID { set; get; } //khóa ngoại thham chiếu

        [ForeignKey("PostID")]
        public Post Post { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }
    }
}