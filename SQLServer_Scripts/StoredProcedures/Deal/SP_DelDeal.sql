USE BudgetManagement
GO

CREATE PROCEDURE SP_DelDeal
	@Id INT
AS
BEGIN
	DECLARE @Price INT,
			@AccountId INT,
			@OperationTypeId INT,
			@OperationDesc NVARCHAR(50),
			@TimesFactor INT = 1;

	SELECT @Price = Price, @AccountId = AccountId,
		   @OperationTypeId = C.OperationTypeId,
		   @OperationDesc = O.T_Description
	FROM Deal D
	INNER JOIN Category C ON C.Id = D.CategoryId
	INNER JOIN OperationType O ON O.Id = C.OperationTypeId
	WHERE D.Id = @Id;

	IF(@OperationDesc = 'Gasto')
		SET @TimesFactor = -1;

	SET @Price = @Price * @TimesFactor;

	UPDATE Account
	SET Balance -= @Price
	WHERE Id = @AccountId

	DELETE FROM Deal WHERE Id = @Id
END