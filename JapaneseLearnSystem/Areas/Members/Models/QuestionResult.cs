namespace JapaneseLearnSystem.Areas.Members.Models
{
    /// <summary>
    /// 使用者答題結果模型
    /// </summary>
    public class QuestionResult
    {
        // 題目編號（對應 QuestionInstance）
        public string QuestionInstanceID { get; set; }

        // 題目內容
        public string QuestionContent { get; set; }

        // 使用者的答案
        public string UserAnswer { get; set; }

        // 正確答案
        public string CorrectAnswer { get; set; }

        // 是否答對
        public bool IsCorrect => UserAnswer == CorrectAnswer;
    }
}
