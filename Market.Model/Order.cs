//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Market.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderStatus = new HashSet<OrderStatu>();
        }
    
        public int Id { get; set; }
        public string SellerId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string BuyerId { get; set; }
        public bool BuyerFeedbacked { get; set; }
        public bool SellerFeedbacked { get; set; }
        public string MiddlemanId { get; set; }
        public bool BuyerChecked { get; set; }
        public bool SellerChecked { get; set; }
        public decimal Sum { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> WithdrawAmount { get; set; }
        public int OfferId { get; set; }
    
        public virtual AccountInfo AccountInfo { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }
        public virtual UserProfile UserProfile2 { get; set; }
        public virtual UserProfile UserProfile3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStatu> OrderStatus { get; set; }
    }
}
