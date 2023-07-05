USE BudgetManagement
GO

-- SELECT * FROM TradeType
INSERT INTO TradeType (T_Description) VALUES
('Ingresos'),
('Gastos')
GO

-- SELECT * FROM Trade
INSERT INTO Trade (UserId, TradeDate, Price, TradeTypeId, Note) VALUES
('Juan', '20211109', 1500.99, 1, 'Ésta es una transacción de prueba'),
('Juan', '20211108', 300.00, 2, null),
('Camila', '20210507', 200, 2, 'Vamos a borrar este registro'),
('Camila', '20210512', 850.95, 1, 'Pago para las pruebas')
GO