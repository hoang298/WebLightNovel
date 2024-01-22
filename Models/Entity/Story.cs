using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    public partial class Story
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Story()
        {
            Comments = new HashSet<Comment>();
            HistoryStories = new HashSet<HistoryStory>();
            StoriesGenres = new HashSet<StoriesGenre>();
            StoriesViews = new HashSet<StoriesView>();
            Volumes = new HashSet<Volume>();
            StoryFollows = new HashSet<StoryFollow>();
        }

        [Key]
        public int story_id { get; set; }

        [Required]
        [StringLength(255)]
        public string title { get; set; }

        public int author_id { get; set; }

        public int? artist_id { get; set; }

        [StringLength(255)]
        public string cover_image { get; set; }

        public string synopsis { get; set; }

        public int state { get; set; }

        public int translation_id { get; set; }

        public int type { get; set; }

        public int? total_views { get; set; }

        public int? follow_views { get; set; }

        public int? total_word { get; set; }

        public int? newChapter_id { get; set; }

        public DateTime? timeUpdate { get; set; }

        [StringLength(255)]
        public string chapter_name { get; set; }

        public int? day_views { get; set; }

        public int? month_views { get; set; }

        public int? comment_views { get; set; }
        public virtual Artist Artist { get; set; }

        public virtual Author Author { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryStory> HistoryStories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoriesGenre> StoriesGenres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoriesView> StoriesViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> Volumes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoryFollow> StoryFollows { get; set; }

        public virtual TranslationTeam TranslationTeam { get; set; }
    }
}
