﻿IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pLandOwnerUsageReport'))
    drop procedure dbo.pLandOwnerUsageReport
go

create procedure dbo.pLandOwnerUsageReport
(
    @year int
)
as

begin
select a.AccountID, a.AccountName, a.AccountNumber, a.AcresManaged, a.Allocation, a.Purchased, a.Sold,
a.ProjectWater, a.Reconciliation, a.NativeYield, a.StoredWater,
a.Allocation + a.Purchased - a.Sold as TotalSupply, a.UsageToDate,
a.Allocation + a.Purchased - a.Sold - a.UsageToDate as CurrentAvailable,
a.NumberOfPostings, a.NumberOfTrades,
mrtr.TradeNumber as MostRecentTradeNumber
from
(
	select acc.AccountID, acc.AccountName, acc.AccountNumber, 
			isnull(pa.ProjectWater, 0) as ProjectWater,
			isnull(pa.Reconciliation, 0) as Reconciliation,
			isnull(pa.NativeYield, 0) as NativeYield,
			isnull(pa.StoredWater, 0) as StoredWater,
			isnull(pa.AcresManaged, 0) as AcresManaged,
			isnull(pa.Allocation, 0) as Allocation,
			isnull(wts.Purchased, 0) as Purchased,
			isnull(wts.Sold, 0) as Sold,
			isnull(pme.UsageToDate, 0) as UsageToDate,
			isnull(post.NumberOfPostings, 0) as NumberOfPostings,
			isnull(tr.NumberOfTrades, 0) as NumberOfTrades
	from dbo.Account acc
	left join
	(
		select acc.AccountID, 
				sum(p.ParcelAreaInAcres) as AcresManaged, 
				sum(pa.AcreFeetAllocated) as Allocation, 
				sum(case when pa.ParcelAllocationTypeID = 1 then pa.AcreFeetAllocated else 0 end) as ProjectWater,
				sum(case when pa.ParcelAllocationTypeID = 2 then pa.AcreFeetAllocated else 0 end) as Reconciliation,
				sum(case when pa.ParcelAllocationTypeID = 3 then pa.AcreFeetAllocated else 0 end) as NativeYield,
				sum(case when pa.ParcelAllocationTypeID = 4 then pa.AcreFeetAllocated else 0 end) as StoredWater
		from dbo.Account acc
		join dbo.AccountParcel up on acc.AccountID = up.AccountID
		join dbo.Parcel p on up.ParcelID = p.ParcelID
		join dbo.ParcelAllocation pa on p.ParcelID = pa.ParcelID and pa.WaterYear = @year
		group by acc.AccountID
	) pa on acc.AccountID = pa.AccountID
	left join
	(
		select acc.AccountID , sum(coalesce(pme.OverriddenEvapotranspirationRate, pme.EvapotranspirationRate)) as UsageToDate
		from dbo.Account acc
		join dbo.AccountParcel up on acc.AccountID = up.AccountID
		join dbo.Parcel p on up.ParcelID = p.ParcelID
		join dbo.ParcelMonthlyEvapotranspiration pme on p.ParcelID = pme.ParcelID and pme.WaterYear = @year
		group by acc.AccountID
	) pme on acc.AccountID = pme.AccountID
	left join 
	(
		select acc.AccountID, 
				sum(case when wtr.WaterTransferTypeID = 1 then wt.AcreFeetTransferred else 0 end) as Purchased,
				sum(case when wtr.WaterTransferTypeID = 2 then wt.AcreFeetTransferred else 0 end) as Sold
		from dbo.Account acc
		join dbo.WaterTransferRegistration wtr on acc.AccountID = wtr.AccountID and wtr.WaterTransferRegistrationStatusID = 2 -- only want registered transfers
		join dbo.WaterTransfer wt on wtr.WaterTransferID = wt.WaterTransferID and year(wt.TransferDate) = @year
		group by acc.AccountID
	) wts on acc.AccountID = wts.AccountID
	left join 
	(
		select acc.AccountID, count(post.PostingID) as NumberOfPostings
		from dbo.Account acc
		join dbo.Posting post on acc.AccountID = post.CreateAccountID
		where year(post.PostingDate) = @year
		group by acc.AccountID
	) post on acc.AccountID = post.AccountID
	left join
	(
		select acc.AccountID, count(post.TradeID) as NumberOfTrades
		from dbo.Account acc
		join dbo.Trade post on acc.AccountID = post.CreateAccountID
		where year(post.TradeDate) = @year
		group by acc.AccountID
	) tr on acc.AccountID = tr.AccountID
) a
left join
(
	select th.TradeID, th.TradeNumber, th.AccountID, th.TransactionDate, rank() over(partition by th.AccountID order by th.TransactionDate desc, th.TradeNumber desc) as Ranking
	from
	(
		select t.TradeID, t.TradeNumber, t.CreateAccountID as AccountID, coalesce(p.PostingDate, t.TradeDate, o.OfferDate) as TransactionDate
		from dbo.Offer o
		join dbo.Trade t on o.TradeID = t.TradeID
		join dbo.Posting p on t.PostingID = p.PostingID
		union
		select t.TradeID, t.TradeNumber, p.CreateAccountID as AccountID, coalesce(p.PostingDate, t.TradeDate, o.OfferDate) as TransactionDate
		from dbo.Offer o
		join dbo.Trade t on o.TradeID = t.TradeID
		join dbo.Posting p on t.PostingID = p.PostingID
		union
		select t.TradeID, t.TradeNumber, wtr.AccountID, wtr.StatusDate as TransactionDate
		from dbo.WaterTransferRegistration wtr
		join dbo.WaterTransfer wt on wtr.WaterTransferID = wt.WaterTransferID
		join dbo.Offer o on wt.OfferID = o.OfferID
		join dbo.Trade t on o.TradeID = t.TradeID
		where wtr.StatusDate is not null
	) th
) mrtr on a.AccountID = mrtr.AccountID and mrtr.Ranking = 1


end