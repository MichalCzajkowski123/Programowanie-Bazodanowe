DROP PROCEDURE IF EXISTS dbo.GetUserBasket;
GO

CREATE PROCEDURE dbo.GetUserBasket
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        bp.ProductID,
        COALESCE(p.Name, 'Unknown') AS ProductName,
        bp.Amount
    FROM BasketPositions bp
    LEFT JOIN Products p ON bp.ProductID = p.ID
    WHERE bp.UserID = @UserID;
END;
