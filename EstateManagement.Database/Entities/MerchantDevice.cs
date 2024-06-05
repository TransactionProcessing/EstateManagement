namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("merchantdevice")]
    public class MerchantDevice
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }

        public Guid DeviceId { get; set; }

        public String DeviceIdentifier { get; set; }
        
        public Guid MerchantId { get; set; }

        #endregion
    }
}