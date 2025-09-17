using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Areas.Members.Models
{
    public class PracticePrepare
    {
        [Required(ErrorMessage = "請輸入題目數量")]
        [Range(1, 20, ErrorMessage = "一次最多只能玩 20 題,或是您輸入了負數")]
        public int Count { get; set; }
    }
}
