namespace EstateManagement.Models.MerchantStatement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MerchantStatement
    {
        public Int32 StatementNumber { get; set; } // TODO: How is this allocated??

        public Boolean IsCreated { get; set; }

        public Boolean IsGenerated { get; set; }

        public Guid EstateId { get; set; }
        public Guid MerchantStatementId { get; set; }
        public Guid MerchantId { get; set; }

        public DateTime StatementGeneratedDateTime { get; set; }
        public DateTime StatementCreatedDateTime { get; set; }

        private List<StatementLine> StatementLines;

        public void AddStatementLine(StatementLine statementLine)
        {
            this.StatementLines.Add(statementLine);
        }

        public List<StatementLine> GetStatementLines()
        {
            return this.StatementLines.OrderBy(s =>s.DateTime).ToList();
        }

        public MerchantStatement()
        {
            this.StatementLines = new List<StatementLine>();
        }
    }
}