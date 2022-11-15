CREATE OR ALTER VIEW [dbo].[uvwMerchants]
AS
SELECT
	m.EstateId,
	m.MerchantId,
	m.Name as MerchantName,
	m.CreatedDateTime,
	ma.AddressLine1,
	ma.Town,
	ma.PostalCode,
	md.DeviceIdentifier,
	mc.Name as ContactName
from merchant m
inner join merchantaddress ma on ma.MerchantId = m.MerchantId and ma.EstateId = m.EstateId
inner join merchantdevice md on md.MerchantId = m.MerchantId and md.EstateId = m.EstateId
inner join merchantcontact mc on mc.MerchantId = m.MerchantId and mc.EstateId = m.EstateId