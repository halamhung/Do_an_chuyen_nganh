using HKQTravellingAuthenication.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace HKQTravellingAuthenication.Components
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public List<Category> categories { get; set; }
            public int level { get; set; } //chia cấp độ giữa cha và con 
            public string categoryslug { get; set; } //cho biết các category nào và highlight lên
        }

        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }
    }
}
