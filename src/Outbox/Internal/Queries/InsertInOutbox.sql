INSERT INTO Outbox(RawData,MessageType,Topic,PartitionBy,IsSequential,Metadata)
VALUES(@RawData,@MessageType,@Topic,@PartitionBy,@IsSequential,@Metadata)