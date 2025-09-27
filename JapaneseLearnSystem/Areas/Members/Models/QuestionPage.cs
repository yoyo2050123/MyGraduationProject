using JapaneseLearnSystem.Models;

namespace JapaneseLearnSystem.Areas.Members.Models
{
    public class QuestionPage
    {
        public string QuestionInstanceID { get; set; } = string.Empty;
        public string QuestionContent { get; set; } = string.Empty;
        public List<QuestionOption> Options { get; set; } = new();
        public string? SelectedOptionID { get; set; }  // 使用者選的答案
    }

}
