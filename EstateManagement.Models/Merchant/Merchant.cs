﻿namespace EstateManagement.Models.Merchant
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Merchant
    {
        #region Properties

        public List<Address> Addresses { get; set; }

        public List<Contact> Contacts { get; set; }
        
        public Dictionary<Guid, String> Devices { get; set; }

        public Int32 EstateReportingId { get; set; }

        public Int32 MerchantReportingId { get; set; }

        public Guid MerchantId { get; set; }

        public String MerchantName { get; set; }

        public String Reference { get; set; }

        public List<Operator> Operators { get; set; }

        public List<SecurityUser> SecurityUsers { get; set; }

        public SettlementSchedule SettlementSchedule { get; set; }

        public DateTime NextSettlementDueDate { get; set; }

        public DateTime NextStatementDate { get; set; }

        #endregion
    }
}