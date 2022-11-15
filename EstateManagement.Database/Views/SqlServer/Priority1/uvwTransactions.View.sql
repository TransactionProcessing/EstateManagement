CREATE OR ALTER VIEW [dbo].[uvwTransactions]
AS
SELECT
	t.TransactionId,
	t.TransactionDateTime,
	t.TransactionDate,
	FORMAT(t.TransactionDate, 'dddd') as DayOfWeek,
	DATEPART(wk, t.TransactionDate) as WeekNumber,
	FORMAT(t.TransactionDate, 'MMMM') as Month,
	DATEPART(MM, t.TransactionDate) as MonthNumber,
	YEAR(t.TransactionDate) as YearNumber,
	t.EstateId,
	t.MerchantId,
	t.IsAuthorised,
	t.IsCompleted,
	t.ResponseCode,
	t.TransactionType,
	CAST(ISNULL(tar.Amount,0) as decimal) as Amount,
	CASE t.OperatorIdentifier
		WHEN 'Voucher' THEN REPLACE(c.Description, ' Contract', '')
		ELSE COALESCE(t.OperatorIdentifier, '')
	END as OperatorIdentifier,
	t.ContractId
from [transaction] t
inner join contract c on c.ContractId = t.ContractId
left outer join transactionadditionalrequestdata tar on tar.TransactionId = t.TransactionId AND tar.MerchantId = t.MerchantId and tar.EstateId = t.EstateId