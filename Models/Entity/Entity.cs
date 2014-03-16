using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace XPGroup.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Parent category is required")]
        public int ParentId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Sort order")]
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be integer")]
        public int SortOrder { get; set; }

        public virtual ICollection<Attribute> Attributes { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Parent category is required")]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public string OtherImages { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public string Attributes { get; set; }

        public DateTime Date { get; set; }

        public bool IsShowHomePage { get; set; }

        public bool IsShowBanner { get; set; }

        public virtual Category Category { get; set; }
    }

    public class NewsCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Parent category is required")]
        public int ParentId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Sort order")]
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be integer")]
        public int SortOrder { get; set; }

        public virtual ICollection<News> NewsList { get; set; }
    }

    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Parent category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "News title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime Date { get; set; }

        public virtual NewsCategory Category { get; set; }
    }

    public class HtmlText
    {
        [Key]
        public int HtmlId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Content { get; set; }
    }

    public class Attribute
    {
        [Key]
        public int AttributeId { get; set; }

        [Required]
        [DisplayName("Attribute name")]
        public string Name { get; set; }

        public string Value { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<AttributeOption> Attributes { get; set; }
    }

    public class AttributeOption
    {
        [Key]
        public int AttributeOptionId { get; set; }

        public int AttributeId { get; set; }

        [Required]
        public string Value { get; set; }

        public virtual Attribute Attribute { get; set; }
    }

    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }

    public class UserInRoles
    {
        [Key]
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual UserRole Role { get; set; }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Telephone { get; set; }

        public string MobilePhone { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string Zipcode { get; set; }
    }

    public class DbWeb : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<NewsCategory> NewsCategories { get; set; }

        public DbSet<News> Newss { get; set; }

        public DbSet<HtmlText> HtmlTexts { get; set; }

        public DbSet<Attribute> Attributes { get; set; }

        public DbSet<AttributeOption> AttributeOptions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserInRoles> UsersInRoles { get; set; }
    }
}