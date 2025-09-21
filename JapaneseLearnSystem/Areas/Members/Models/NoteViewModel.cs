using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Models
{
    public class NoteViewModel
    {
        public int NoteID { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string OriginalArticle { get; set; } = null!;

        public string Reading { get; set; } = null!;
        public string Translate { get; set; } = null!;

        [Required]
        [Display(Name = "JLPT Level")]
        public int JLPTLevelID { get; set; }

        public string? JLPTLevelName { get; set; }  // 用於顯示文字

        public string? MemberID { get; set; } // 從登入會員取得，允許為 null
    }
}
