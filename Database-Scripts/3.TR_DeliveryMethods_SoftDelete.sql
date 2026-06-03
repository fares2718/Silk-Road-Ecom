CREATE TRIGGER TR_DeliveryMethods_SoftDelete
ON DeliveryMethods
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dm
    SET Available = 0
    FROM DeliveryMethods dm
    INNER JOIN deleted d
        ON dm.DeliveryMethodID = d.DeliveryMethodID;
END;
GO