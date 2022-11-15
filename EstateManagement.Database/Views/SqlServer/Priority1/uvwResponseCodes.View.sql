CREATE OR ALTER VIEW [dbo].[uvwResponseCodes]
AS
SELECT
	RIGHT('000'+CAST(responsecode AS VARCHAR(4)),4) as ResponseCode,
	Description
from [responsecodes]