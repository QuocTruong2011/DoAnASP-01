using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Models
{
    public class Account
    {
        public int AccountId { set; get; }
        public List<Cart> Cart { set; get; }
        public List<Invoice> Invoices { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        public string Usename { set; get; }
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
        public string PassWord { set; get; }
        [EmailAddress(ErrorMessage = "Email phải có @")]
        public string Email { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "số điện thoại phải đúng 10 số")]
        public string Phone { set; get; }
        public bool IsAdmin { set; get; }
        public string avatar { set; get; }
        [NotMapped]
        public IFormFile ImageFile { set; get; }
    }
}
