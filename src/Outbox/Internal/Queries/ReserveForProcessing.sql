IF EXISTS(SELECT 1 FROM Outbox WHERE IsProcessed = 0 AND IsSequential = 0 AND (IsProcessing = 0 OR (IsProcessing = 1 AND SYSDATETIMEOFFSET() >= ExpiredAt)))
BEGIN
WITH forReservation AS (
    SELECT TOP (@MaxLimit) * FROM Outbox
    WITH (UPDLOCK, ROWLOCK, READPAST)
    WHERE IsProcessed = 0 AND IsSequential = 0 AND (IsProcessing = 0 OR (IsProcessing = 1 AND SYSDATETIMEOFFSET() >= ExpiredAt))
)
UPDATE forReservation
SET IsProcessing = 1, ReservedAt = SYSDATETIMEOFFSET(), ExpiredAt = DATEADD(SECOND, (@ReservationSeconds), SYSDATETIMEOFFSET())
OUTPUT INSERTED.*
END