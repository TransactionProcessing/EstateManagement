namespace EstateManagement.Database.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("statementheader")]
    public class StatementHeader
    {
        public Guid StatementId { get; set; }

        public Guid MerchantId { get; set; }

        public DateTime StatementCreatedDate { get; set; }

        public DateTime StatementGeneratedDate { get; set; }
    }
}
