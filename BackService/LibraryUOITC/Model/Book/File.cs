using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Model.Book
{
    public class File
    {
        [Required]
        public IFormFile file { get; set; }
    }
}
