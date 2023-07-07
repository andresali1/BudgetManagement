USE BudgetManagement
GO

CREATE PROCEDURE SP_InsDeal
	@UserId INT,
	@DealDate DATE,
	@Price INT,
	@AccountId INT,
	@CategoryId INT,
	@Note NVARCHAR(1000) = NULL
AS
BEGIN
	INSERT INTO Deal (UserId, DealDate, Price, AccountId, CategoryId, Note)
	VALUES (@UserId, @DealDate, ABS(@Price), @AccountId, @CategoryId, @Note);

	UPDATE Account SET
	Balance += @Price
	WHERE Id = @AccountId;

	SELECT SCOPE_IDENTITY();
END