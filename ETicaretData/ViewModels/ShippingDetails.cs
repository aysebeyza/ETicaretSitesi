﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretData.ViewModels
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="Lütfen Boş geçeyiniz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen Boş geçeyiniz.")]
        public string AddressTitle { get; set; }
        [Required(ErrorMessage = "Lütfen Boş geçeyiniz.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Lütfen Boş geçeyiniz.")]
        public string City { get; set; }
    }
}
