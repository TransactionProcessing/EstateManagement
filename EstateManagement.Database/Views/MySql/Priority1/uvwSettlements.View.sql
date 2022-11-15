CREATE OR REPLACE VIEW uvwSettlements
AS 
SELECT
        s.SettlementDate,
	s.IsCompleted,
	FORMAT(s.SettlementDate, 'dddd') as DayOfWeek,
	WEEKOFYEAR(s.SettlementDate) as WeekNumber,
	FORMAT(s.SettlementDate, 'MMMM') as Month,
	MONTH(s.SettlementDate) as MonthNumber,
	YEAR(s.SettlementDate) as YearNumber,
	f.CalculatedValue,
	f.TransactionId,
	t.EstateId,
	t.MerchantId,
	m.Name as MerchantName,
	cptf.Description as FeeDescription,
	CASE t.OperatorIdentifier
		WHEN 'Voucher' THEN REPLACE(c.Description, ' Contract', '')
		ELSE COALESCE(t.OperatorIdentifier, '')
	END as OperatorIdentifier,
	CAST(IFNULL(tar.Amount,0) as decimal) as Amount,
	f.IsSettled
from settlement s 
inner join merchantsettlementfee f on s.SettlementId = f.SettlementId
inner join transaction t on t.TransactionId = f.TransactionId
inner join merchant m on t.MerchantId = m.MerchantId
left outer join contractproducttransactionfee cptf on f.FeeId = cptf.TransactionFeeId
left outer join transactionadditionalrequestdata tar on tar.TransactionId = t.TransactionId AND tar.MerchantId = t.MerchantId and tar.EstateId = t.EstateId
inner join contract c on c.ContractId = t.ContractId