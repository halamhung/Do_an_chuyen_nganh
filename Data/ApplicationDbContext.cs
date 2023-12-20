using HKQTravellingAuthenication.Models;
using HKQTravellingAuthenication.Models.Contact;
using HKQTravellingAuthenication.Models.Blog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using HKQTravellingAuthenication.Models.Tour;

namespace HKQTravellingAuthenication.Data
{
    //HKQTravellingAuthenication.Data.ApplicationDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<AppUser>()
                .HasIndex(u => u.NewCitizenIdentification)
                .IsUnique();
            builder.Entity<AppUser>()
                .HasIndex(u => u.OldCitizenIdentification)
                .IsUnique();

            builder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Slug)
                .IsUnique();
            });
            //dùng fluent api để trỏ tới 2 thuộc tính để trở thành 2 khóa chính
            builder.Entity<PostCategory>(entity =>
            {
                entity.HasKey(c => new { c.PostID, c.CategoryID });//thiết lập trường khóa chính 
            });
            builder.Entity<Post>(entity =>
            {
                entity.HasIndex(p => p.Slug) //thiết lập chỉ mục
                    .IsUnique();
            });

            builder.Entity<StartLocations>()
                .HasIndex(u => u.StartLocationName)
                .IsUnique();
            builder.Entity<EndLocations>()
                .HasIndex(u => u.EndLocationName)
                .IsUnique();
            builder.Entity<TourTypes>()
                .HasIndex(u => u.TourTypeName)
                .IsUnique();

            builder.Entity<Tours>()
            .Property(e => e.CreationDate)
            .HasDefaultValueSql("GETDATE()"); // Thiết lập giá trị mặc định là GETDATE()

            builder.Entity<Bookings>()
            .HasOne(p => p.users)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); //Các lịch sử bookings sẽ bị xóa khi người dùng xóa

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StartLocations> startLocations { get; set; }
        public DbSet<EndLocations> endLocations { get; set; }
        public DbSet<Discounts> discounts { get; set; }
        public DbSet<TourTypes> tourTypes { get; set; }
        //public DbSet<Rules> rules { get; set; }
        public DbSet<Tours> tours { get; set; }
        public DbSet<TourImages> tourImages { get; set; }
        //public DbSet<TourDays> tourDays { get; set; }
        public DbSet<Bookings> bookings { get; set; }
        public DbSet<Payments> payments { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
    }
}
