﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Sizes")]

    public class Size:CommonAbstract
    {
        public Size() {
            this.ProductDetails = new HashSet<ProductDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
        public ICollection<ProductDetail> ProductDetails { get; set; }

    }
}