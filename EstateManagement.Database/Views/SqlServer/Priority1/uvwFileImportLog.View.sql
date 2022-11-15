CREATE OR ALTER VIEW uvwFileImportLog
AS
SELECT 
	fileimportlog.FileImportLogId, 
	fileimportlog.ImportLogDateTime,
	[ImportLogDate] = CONVERT(DATE,fileimportlog.ImportLogDateTime),
	[ImportLogTime] = CONVERT(TIME, fileimportlog.ImportLogDateTime), 
	COUNT(fileimportlogfile.FileId) as FileCount,
	f.MerchantId
	
from fileimportlog
inner join fileimportlogfile on fileimportlogfile.FileImportLogId = fileimportlog.FileImportLogId
inner join [file] f on f.FileId = fileimportlogfile.FileId
group by fileimportlog.FileImportLogId, fileimportlog.ImportLogDateTime, f.MerchantId