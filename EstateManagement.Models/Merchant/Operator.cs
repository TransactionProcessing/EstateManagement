namespace EstateManagement.Models.Merchant
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Operator
    {
        #region Properties

        public String MerchantNumber { get; set; }

        public String Name { get; set; }

        public Guid OperatorId { get; set; }

        public String TerminalNumber { get; set; }

        public Boolean IsDeleted { get; set; }

        #endregion
    }
}