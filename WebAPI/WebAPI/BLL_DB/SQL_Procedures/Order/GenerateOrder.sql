CREATE PROCEDURE dbo.GenerateOrder
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE ID = @UserID)
    BEGIN
        RAISERROR ('User does not exist.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM BasketPositions WHERE UserID = @UserID)
    BEGIN
        RAISERROR ('Cannot generate an order with an empty basket.', 16, 1);
        RETURN;
    END

    DECLARE @OrderID INT;

    INSERT INTO Orders (UserID, Date, IsPaid)
    VALUES (@UserID, GETDATE(), 0);

    SET @OrderID = SCOPE_IDENTITY();

    INSERT INTO OrderPositions (OrderID, ProductID, Amount, Price)
    SELECT 
        @OrderID, 
        bp.ProductID, 
        bp.Amount, 
        p.Price
    FROM BasketPositions bp
    JOIN Products p ON bp.ProductID = p.ID
    WHERE bp.UserID = @UserID;

    DELETE FROM BasketPositions WHERE UserID = @UserID;
END;
