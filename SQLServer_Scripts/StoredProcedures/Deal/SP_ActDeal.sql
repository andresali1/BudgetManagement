USE BudgetManagement
GO

CREATE PROCEDURE SP_ActDeal
	@Id INT,
	@DealDate DATE,
	@Price INT,
	@PreviousPrice INT,
	@AccountId INT,
	@PreviousAccountId INT,
	@CategoryId INT,
	@Note NVARCHAR(1000) = NULL
AS
BEGIN
	--Revert previous transaction
	UPDATE Account
	SET Balance -= @PreviousPrice
	WHERE Id = @PreviousAccountId

	--Save the new Transaction
	UPDATE Account
	SET Balance += @Price
	WHERE Id = @AccountId

	
	UPDATE Deal SET
		Price = ABS(@Price),
		DealDate = @DealDate,
		CategoryId = @CategoryId,
		AccountId = @AccountId,
		Note = @Note
	WHERE Id = @Id
END