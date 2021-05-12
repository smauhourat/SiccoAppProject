create procedure spDocumentationsByCustomerContractor
	@CustomerID int,
	@ContractorID int
as

		select
				*
		from
				vwDocumentations
		where
				CustomerID = @CustomerID
				and ContractorID = @ContractorID