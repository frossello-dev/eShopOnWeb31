﻿using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class ProductSupplier : BaseEntity, IAggregateRoot
    {
        public int CatalogID { get; set; }
        public int SupplierID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
