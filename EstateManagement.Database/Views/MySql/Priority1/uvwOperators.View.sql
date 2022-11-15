CREATE OR REPLACE VIEW [dbo].[uvwOperators]
AS
SELECT 
	ContractId as OperatorId,
	Description as OperatorName
from contract