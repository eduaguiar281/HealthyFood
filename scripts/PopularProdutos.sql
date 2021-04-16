declare @totalCriar int = 0
declare @rowId int = 1

declare @nomeProduto nvarchar(70)
declare @codigoBarras nvarchar(30)
declare @preco decimal(18,2)

declare @max int = 1000000000
declare @min int = 100000


while (@rowId <= @totalCriar)
BEGIN
   SET @nomeProduto = CONCAT('Produto ', @rowId)
   SET @codigoBarras = CONVERT(nvarchar, ABS(CHECKSUM(NEWID()) % (@max - @min - 1)) + @min)
   SET @preco = RAND()*(30-5)+5
   INSERT INTO Produtos (CodigoBarras, Descricao, Preco, TipoProduto, DataCriacao, DataAlteracao) VALUES (@codigoBarras, @nomeProduto, @preco, 'Bebida', GETUTCDATE(), GETUTCDATE())
   set @rowId = @rowId +1
END

SELECT * FROM PRODUTOS