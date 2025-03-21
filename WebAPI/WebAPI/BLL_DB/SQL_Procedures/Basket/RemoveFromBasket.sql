
CREATE PROCEDURE dbo.RemoveFromBasket
    @UserID INT,
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM BasketPositions WHERE UserID = @UserID AND ProductID = @ProductID)
            RETURN; 

        DELETE FROM BasketPositions WHERE UserID = @UserID AND ProductID = @ProductID;
    END TRY
    BEGIN CATCH
        PRINT ERROR_MESSAGE();
    END CATCH
END;

