namespace EstateManagement.Database.Entities;

using System.ComponentModel.DataAnnotations.Schema;

[Table("statementline")]
public class StatementLine
{
    public Int32 StatementReportingId { get; set; }

    public DateTime ActivityDateTime { get; set; }

    public DateTime ActivityDate { get; set; }

    public Int32 ActivityType { get; set; }

    public String? ActivityDescription { get; set; }

    public Decimal InAmount { get; set; }
    public Decimal OutAmount { get; set; }

    public Int32 TransactionReportingId { get; set; }
}