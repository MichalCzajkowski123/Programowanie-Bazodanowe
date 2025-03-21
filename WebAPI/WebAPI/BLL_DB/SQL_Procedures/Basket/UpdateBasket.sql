DROP PROCEDURE IF EXISTS dbo.UpdateBasketAmount;
GO

CREATE PROCEDURE dbo.UpdateBasketAmount
    @UserID INT,
    @ProductID INT,
    @NewAmount INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM BasketPositions WHERE UserID = @UserID AND ProductID = @ProductID)
    BEGIN
        RAISERROR ('Basket position not found. Add product first.', 16, 1);
        RETURN;
    END

    UPDATE BasketPositions
    SET Amount = @NewAmount
    WHERE UserID = @UserID AND ProductID = @ProductID;
END;