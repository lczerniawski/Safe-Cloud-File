using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer_Example.Domains.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Pole email jest wymagane!")]
        [EmailAddress(ErrorMessage = "Należy podać poprawny adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole hasło jest wymagane!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
