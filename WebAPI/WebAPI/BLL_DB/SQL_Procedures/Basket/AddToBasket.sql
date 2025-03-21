CREATE PROCEDURE AddToBasket
    @UserID INT,
    @ProductID INT,
    @Amount INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE ID = @UserID)
    BEGIN
        RAISERROR ('User does not exist.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Products WHERE ID = @ProductID)
    BEGIN
        RAISERROR ('Product does not exist.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM BasketPositions WHERE UserID = @UserID AND ProductID = @ProductID)
    BEGIN

        UPDATE BasketPositions
        SET Amount = Amount + @Amount
        WHERE UserID = @UserID AND ProductID = @ProductID;
    END
    ELSE
    BEGIN

        INSERT INTO BasketPositions (UserID, ProductID, Amount)
        VALUES (@UserID, @ProductID, @Amount);
    END
END;
