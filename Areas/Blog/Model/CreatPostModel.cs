using System.ComponentModel.DataAnnotations;
using HKQTravellingAuthenication.Models.Blog;

namespace HKQTravellingAuthenication.Areas.Blog.Model
{

       public class CreatePostModel : Post {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}
