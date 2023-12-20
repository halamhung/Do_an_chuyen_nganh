using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.Collections;

namespace HKQTravellingAuthenication.Models.Blog {
  [Table("Category")]
  public class Category
  {

      [Key]
      public int Id { get; set; }

      // Category cha (FKey)
      [Display(Name = "Danh mục cha")]
      public int? ParentCategoryId { get; set; }

      // Tiều đề Category
      [Required(ErrorMessage = "Phải có tên danh mục")]
      [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
      [Display(Name = "Tên danh mục")]
      public string Title { get; set; }

      // Nội dung, thông tin chi tiết về Category
      [DataType(DataType.Text)]
      [Display(Name = "Nội dung danh mục")]
      public string Content { set; get; }

      //chuỗi Url
      [Required(ErrorMessage = "Phải tạo url")]
      [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
      [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
      [Display(Name = "Url hiện thị")]
      public string Slug { set; get; }

      // Các Category con
      public ICollection<Category>? CategoryChildren { get; set; }

      [ForeignKey("ParentCategoryId")]
      [Display(Name = "Danh mục cha")]


      public Category? ParentCategory { set; get; }



    //tạo ra phương thức này để truyền tới Index của PostViewController để lấy ra các category con của danh mục
      public void ChildCategoryIDs(ICollection<Category> childcates, List<int> list)
      {
        if(childcates == null)
        childcates = this.CategoryChildren;
        foreach(Category category in childcates)
        {
          list.Add(category.Id);
          ChildCategoryIDs(category.CategoryChildren, list);
        }
      }
      public List<Category> ListParents(){
        List <Category> li = new List<Category>();
        var parent = this.ParentCategory;
        //dùng while để duyệt ngược lại con(2)->con(1, cha của con 2)-> cha 
        while(parent != null){
          li.Add(parent);
          parent = parent.ParentCategory;
        }
        li.Reverse();
        return li;
      }
  }
}