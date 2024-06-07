CREATE OR ALTER PROCEDURE spBuildHistoricTransactions
AS

insert into TransactionsHistory(MerchantReportingId, ContractProductReportingId,ContractReportingId,OperatorReportingId,
TransactionId,
AuthorisationCode,
DeviceIdentifier,
IsAuthorised,
IsCompleted,
ResponseCode,
ResponseMessage,
TransactionDate,
TransactionDateTime, 
TransactionNumber,
TransactionReference,
TransactionTime,
TransactionSource,
TransactionType,
TransactionReportingId,
TransactionAmount,
Hour)
select merchant.MerchantReportingId,
ISNULL(contractproduct.ContractProductReportingId,0) as ContractProductReportingId,
ISNULL(contract.ContractReportingId,0) as ContractReportingId,
ISNULL(operator.OperatorReportingId,0) as OperatorReportingId,
t.TransactionId,
t.AuthorisationCode,
t.DeviceIdentifier,
t.IsAuthorised,
t.IsCompleted,
t.ResponseCode,
t.ResponseMessage,
t.TransactionDate,
t.TransactionDateTime, 
t.TransactionNumber,
t.TransactionReference,
t.TransactionTime,
t.TransactionSource,
t.TransactionType,
t.TransactionReportingId,
t.TransactionAmount,
t.Hour
from TodaysTransactions t
where TransactionReportingId not in (select distinct TransactionReportingId from TodaysTransactions)

delete from TodaysTransactions

GO



