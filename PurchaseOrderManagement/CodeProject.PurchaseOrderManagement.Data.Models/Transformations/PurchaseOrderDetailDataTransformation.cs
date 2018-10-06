﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.PurchaseOrderManagement.Data.Transformations
{
    public class PurchaseOrderDetailDataTransformation
    {
		public int PurchaseOrderDetailId { get; set; }
		public int PurchaseOrderId { get; set; }
		public int ProductId { get; set; }
		public int ProductMasterId { get; set; }
		public string ProductNumber { get; set; }
		public string Description { get; set; }
		public double UnitPrice { get; set; }
		public int OrderQuantity { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
