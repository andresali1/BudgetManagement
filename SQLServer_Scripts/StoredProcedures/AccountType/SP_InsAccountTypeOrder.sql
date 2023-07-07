USE BudgetManagement
GO

CREATE PROCEDURE SP_InsAccountTypeOrder
	@AT_Name NVARCHAR(50),
	@UserId INT
AS
BEGIN
	DECLARE @Order INT;

	SELECT @Order = COALESCE(MAX(AT_Order), 0) + 1
	FROM AccountType
	WHERE UserId = @UserId;

	INSERT INTO AccountType(AT_Name, UserId, AT_Order)
	VALUES (@AT_Name, @UserId, @Order);

	SELECT SCOPE_IDENTITY();
END
