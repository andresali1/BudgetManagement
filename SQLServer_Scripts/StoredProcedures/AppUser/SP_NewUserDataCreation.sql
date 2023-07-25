USE BudgetManagement
GO

CREATE PROCEDURE SP_NewUserDataCreation
	@UserId INT
AS
BEGIN
	DECLARE @Cash NVARCHAR(50) = 'Efectivo',
			@BankAccounts NVARCHAR(50) = 'Cuentas de Banco',
			@Cards NVARCHAR(50) = 'Tarjetas';

	INSERT INTO AccountType (AT_Name, UserId, AT_Order)
	VALUES (@Cash, @UserId, 1),
		   (@BankAccounts, @UserId, 2),
		   (@Cards, @UserId, 3);

	INSERT INTO Account (A_Name, Balance, AccountTypeId)
	SELECT AT_Name, 0, Id
	FROM AccountType
	WHERE UserId = @UserId;

	INSERT INTO Category (C_Name, OperationTypeId, UserId)
	VALUES ('Libros', 2, @UserId),
		   ('Salario', 1, @UserId),
		   ('Mesada', 1, @UserId),
		   ('Crédito', 2, @UserId);
END
GO