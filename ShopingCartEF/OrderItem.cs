//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopingCartEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class OrderItem
    {
        [Key]
        public int OrtherID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public Nullable<long> Price { get; set; }
        public Nullable<int> Quantily { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Orther Orther { get; set; }
    }
}
