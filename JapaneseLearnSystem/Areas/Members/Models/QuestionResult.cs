using System.Collections.Generic;

namespace JapaneseLearnSystem.Areas.Members.Models
{
    /// <summary>
    /// 使用者答題結果模型
    /// </summary>
    public class QuestionResult
    {
        // 題目序號（可選，前端用於排序）
        public int Index { get; set; }

        // 題目編號（對應 QuestionInstance）
        public string QuestionInstanceID { get; set; } = string.Empty;

        // 題目內容
        public string QuestionContent { get; set; } = string.Empty;

        // 題型，例如 Reading / Meaning
        public string QuestionType { get; set; } = string.Empty;

        // 該題所有選項（方便前端標示正確答案）
        public List<string> Options { get; set; } = new();

        // 使用者選的答案 OptionID
        public string UserAnswer { get; set; } = string.Empty;

        // 正確答案 OptionID
        public string CorrectAnswer { get; set; } = string.Empty;

        // 是否答對
        public bool IsCorrect => UserAnswer == CorrectAnswer;

        // 顯示用：使用者選項內容
        public string UserAnswerContent { get; set; } = string.Empty;

        // 顯示用：正確答案內容
        public string CorrectAnswerContent { get; set; } = string.Empty;

        public string WordID { get; set; } // 新增，方便統計學習單字數
    }
}
