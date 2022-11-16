namespace EstateManagement.Database.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("statementline")]
public class StatementLine
{
    public Guid StatementId { get; set; }

    public Guid EstateId { get; set; }

    public Guid MerchantId { get; set; }

    public DateTime ActivityDateTime { get; set; }

    public Int32 ActivityType { get; set; }

    public String? ActivityDescription { get; set; }

    public Decimal InAmount { get; set; }
    public Decimal OutAmount { get; set; }

    public Guid TransactionId { get; set; }
}