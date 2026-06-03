CREATE TRIGGER TR_DeliveryProviders_SoftDelete
ON DeliveryProviders
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dp
    SET Available = 0
    FROM DeliveryProviders dp
    JOIN deleted d
        ON dp.ProviderID = d.ProviderID;

    UPDATE dm
    SET Available = 0
    FROM DeliveryMethods dm
    JOIN deleted d
        ON dm.ProviderID = d.ProviderID;
END;
GO