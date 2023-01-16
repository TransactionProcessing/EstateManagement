namespace EstateManagement.Models.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Product
    {
        #region Constructors

        public Product() {
            this.TransactionFees = new List<TransactionFee>();
        }

        #endregion

        #region Properties

        public String DisplayText { get; set; }

        public String Name { get; set; }

        public Guid ProductId { get; set; }

        public ProductType ProductType { get; set; }

        public List<TransactionFee> TransactionFees { get; set; }

        public Decimal? Value { get; set; }

        #endregion
    }
}