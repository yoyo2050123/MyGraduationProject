using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Models
{
    public class NoteViewModel
    {
        public int NoteID { get; set; }

        [Required]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "原文或是單字")]
        public string OriginalArticle { get; set; } = null!;
        [Display(Name = "讀音")]
        public string Reading { get; set; } = null!;
        [Display(Name = "翻譯")]
        public string Translate { get; set; } = null!;

        [Required]
        [Display(Name = "JLPT Level")]
        public int JLPTLevelID { get; set; }

        public string? JLPTLevelName { get; set; }  // 用於顯示文字

        public string? MemberID { get; set; } // 從登入會員取得，允許為 null

        public DateTime? CreateNoteAt { get; set; }


    }
}
