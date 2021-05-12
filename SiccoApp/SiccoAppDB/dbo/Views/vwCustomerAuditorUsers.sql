
CREATE VIEW vwCustomerAuditorUsers

AS

SELECT
		Customer.CustomerID,
		Customer.CompanyName,
		Customer.TaxIdNumber,
		Customer.CountryID,
		Customer.StateID,
		Customer.City,
		Customer.Address,
		Customer.PhoneNumber,
		Customer.Active,
		Customer.CreationDate,
		Customer.CreationUser,
		Customer.ModifiedDate,
		Customer.ModifiedUser,
		CustomerAuditor.CustomerAuditorID,
		AspNetUsers.Id,
		AspNetUsers.UserName,
		AspNetUsers.Discriminator,
		AspNetUsers.Email,
		AspNetUsers.LastName,
		AspNetUsers.FirstName
FROM
		Customer
		INNER JOIN CustomerAuditor ON Customer.CustomerID = CustomerAuditor.CustomerID
		INNER JOIN AspNetUsers ON CustomerAuditor.UserId = AspNetUsers.Id