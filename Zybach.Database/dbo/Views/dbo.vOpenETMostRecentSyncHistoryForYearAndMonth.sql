if exists (select * from dbo.sysobjects where id = object_id('dbo.vOpenETMostRecentSyncHistoryForYearAndMonth'))
	drop view dbo.vOpenETMostRecentSyncHistoryForYearAndMonth
go

Create View dbo.vOpenETMostRecentSyncHistoryForYearAndMonth
as

select openetsh.OpenETSyncHistoryID, openetsh.OpenETSyncID, openetsh.OpenETSyncResultTypeID, openetsh.CreateDate, openetsh.UpdateDate, openetsh.GoogleBucketFileRetrievalURL, openetsh.ErrorMessage
from dbo.OpenETSyncHistory openetsh
join 
(
	select OpenETSyncID, max(CreateDate) CreateDate
	from dbo.OpenETSyncHistory
	group by OpenETSyncID
) mostRecent on openetsh.OpenETSyncID = mostRecent.OpenETSyncID and openetsh.CreateDate = mostRecent.CreateDate

go