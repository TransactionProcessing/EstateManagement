namespace EstateManagement.Models.Estate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Estate
    {
        #region Properties

        public Int32 EstateReportingId { get; set; }

        public Guid EstateId { get; set; }

        public String Name { get; set; }

        public String Reference { get; set; }

        public List<Operator> Operators { get; set; }

        public List<SecurityUser> SecurityUsers { get; set; }

        #endregion
    }
}