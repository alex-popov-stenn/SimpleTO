begin
IF NOT EXISTS (
    SELECT * FROM sys.tables t
    JOIN sys.schemas s ON (t.schema_id = s.schema_id)
    WHERE s.name = 'dbo' AND t.name = 'Outbox')

	CREATE TABLE Outbox(
	Id		                uniqueidentifier not null default newsequentialid() PRIMARY KEY,
	DateTimestamp			datetimeoffset(7) not null default SYSDATETIMEOFFSET(),
	RawData					NVARCHAR(MAX) not null,
    MessageType             NVARCHAR(255) not null,
    Topic 				    NVARCHAR(255) not null,
    PartitionBy             NVARCHAR(255) null,
	IsProcessed				INT DEFAULT 0,
    IsSequential            INT DEFAULT 0,
    Metadata                NVARCHAR(MAX) null,
    ReservedAt              datetimeoffset(7) null,
    ExpiredAt               datetimeoffset(7) null,
    IsProcessing            INT DEFAULT 0
)

IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Outbox_IsProcessed_IsProcessing')
	CREATE NONCLUSTERED  INDEX IX_Outbox_IsProcessed_IsProcessing ON Outbox (IsProcessed, IsProcessing)

end;