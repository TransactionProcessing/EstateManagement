CREATE OR REPLACE VIEW uvwFile
AS
SELECT 
	f.FileId as FileId,
	f.FileReceivedDateTime, 
    DATE(f.FileReceivedDateTime) as FileReceivedDate,
    TIME(f.FileReceivedDateTime) as FileReceivedTime,
	f.IsCompleted,
	merchant.Name as MerchantName,
	OriginalFileName as FileName,
	estatesecurityuser.EmailAddress as EmailAddress, 
	count(fileline.LineNumber) as LineCount, 
	SUM(case fileline.Status WHEN 'P' THEN 1 ELSE 0 END) as PendingCount,
	SUM(case fileline.Status WHEN 'F' THEN 1 ELSE 0 END) as FailedCount,
	SUM(case fileline.Status WHEN 'S' THEN 1 ELSE 0 END) as SuccessCount
from fileimportlogfile
inner join merchant on merchant.MerchantId = fileimportlogfile.MerchantId
inner join estatesecurityuser on estatesecurityuser.SecurityUserId = fileimportlogfile.UserId
inner join file f on f.FileId = fileimportlogfile.FileId
inner join fileline on fileline.FileId = f.FileId
group by merchant.Name, OriginalFileName, estatesecurityuser.EmailAddress, f.FileId, f.FileReceivedDateTime, f.IsCompleted