using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Models // 請確認這是你的專案命名空間
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [EmailAddress(ErrorMessage = "請輸入有效的 Email 格式")]
        [Display(Name = "電子郵件 (帳號)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(100, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入電話")]
        [Display(Name = "電話")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "請輸入生日")]
        [DataType(DataType.Date)]
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }
    }
}