﻿using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.ProductEntity
{
    public class ProductComment:BaseEntity
    {
        #region properties
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }
        #endregion

        #region relations
        public Product Product { get; set; }
        public User User { get; set; }
        #endregion
    }
}
