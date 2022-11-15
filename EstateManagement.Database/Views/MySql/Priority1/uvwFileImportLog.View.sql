CREATE OR REPLACE VIEW uvwFileImportLog
AS
SELECT 
	fileimportlog.FileImportLogId, 
	fileimportlog.ImportLogDateTime,
	DATE(fileimportlog.ImportLogDateTime) as ImportLogDate,
	TIME(fileimportlog.ImportLogDateTime) as ImportLogTime, 
	COUNT(fileimportlogfile.FileId) as FileCount,
	f.MerchantId
	
from fileimportlog
inner join fileimportlogfile on fileimportlogfile.FileImportLogId = fileimportlog.FileImportLogId
inner join file f on f.FileId = fileimportlogfile.FileId
group by fileimportlog.FileImportLogId, fileimportlog.ImportLogDateTime, f.MerchantId