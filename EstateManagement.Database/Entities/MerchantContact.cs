﻿namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantcontact")]
    public class MerchantContact
    {
        #region Properties

        public Guid ContactId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public String? EmailAddress { get; set; }
        
        public Guid MerchantId { get; set; }

        public String Name { get; set; }

        public String? PhoneNumber { get; set; }

        #endregion
    }
}