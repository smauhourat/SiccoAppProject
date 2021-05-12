CREATE VIEW vwCustomerAuditors

AS


	SELECT 
			CustomerAuditor.CustomerAuditorID,
			CustomerAuditor.CustomerID,
			Customer.CompanyName,
			CustomerAuditor.UserId,
			AspNetUsers.UserName,
			AspNetUsers.LastName,
			AspNetUsers.FirstName,
			AspNetUsers.Email
	FROM
			CustomerAuditor
			INNER JOIN AspNetUsers ON CustomerAuditor.UserId = AspNetUsers.Id 
			INNER JOIN Customer ON CustomerAuditor.CustomerID = Customer.CustomerID