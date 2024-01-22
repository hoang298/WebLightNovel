using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Account;

namespace WebLightNovel.Models.Entity
{
    public partial class Connect_sql : IdentityDbContext<ApplicationUser>, IDataDbContext
    {
        public Connect_sql() : base("name=Connect_sql")
        {
            this.Configuration.LazyLoadingEnabled = false;//tắt chế độ tự động mapping các dữ liệu có quan hệ với nhau trong các bảng
        }
        public static Connect_sql Create()
        {
            return new Connect_sql();
        }
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<HistoryStory> HistoryStories { get; set; }
        public virtual DbSet<ImageChapter> ImageChapters { get; set; }
        public virtual DbSet<JoinRequest> JoinRequests { get; set; }
        public virtual DbSet<Letter> Letters { get; set; }
        public virtual DbSet<Notify> Notifies { get; set; }
        public virtual DbSet<PostStory> PostStories { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<StoriesGenre> StoriesGenres { get; set; }
        public virtual DbSet<StoriesView> StoriesViews { get; set; }
        public virtual DbSet<StoryFollow> StoryFollows { get; set; }
        public virtual DbSet<TranslationTeam> TranslationTeams { get; set; }
        public virtual DbSet<TranslationUser> TranslationUsers { get; set; }
        /*        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        */
        public virtual DbSet<Volume> Volumes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
            modelBuilder.Entity<ApplicationRole>()
                .HasMany(e => e.ApplicationUserRoles)
                .WithOptional(e => e.ApplicationRole)
                .HasForeignKey(e => e.IdentityRole_Id);

            modelBuilder.Entity<Author>()
                .HasMany(e => e.Stories)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Comment>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.StoriesGenres)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ImageChapter>()
                .Property(e => e.img_name)
                .IsUnicode(false);

            modelBuilder.Entity<ImageChapter>()
                .Property(e => e.pHashImage)
                .IsUnicode(false);

            modelBuilder.Entity<Notify>()
                .Property(e => e.url_notify)
                .IsUnicode(false);

            modelBuilder.Entity<PostStory>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<PostStory>()
                .Property(e => e.translationSource)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .Property(e => e.cover_image)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .HasMany(e => e.Comments)
                .WithOptional(e => e.Story)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TranslationTeam>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<TranslationTeam>()
                .HasMany(e => e.JoinRequests)
                .WithRequired(e => e.TranslationTeam)
                .HasForeignKey(e => e.trans_id);

            modelBuilder.Entity<TranslationTeam>()
                .HasMany(e => e.Stories)
                .WithRequired(e => e.TranslationTeam)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TranslationTeam>()
                .HasMany(e => e.TranslationUsers)
                .WithRequired(e => e.TranslationTeam)
                .WillCascadeOnDelete(false);          
            modelBuilder.Entity<Volume>()
                .Property(e => e.volumeImg)
                .IsUnicode(false);
            /*modelBuilder.Entity<Chapter>()
                .Property(e => e.story_id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute { IsUnique = true }));*/
        }
    }
}
