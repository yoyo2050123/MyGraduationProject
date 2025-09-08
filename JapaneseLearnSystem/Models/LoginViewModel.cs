using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Models
{
    
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "請輸入帳號")]   // 必填
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]   // 必填
        [DataType(DataType.Password)]            // 顯示為密碼框
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? ReturnUrl { get; set; }
    }
}
