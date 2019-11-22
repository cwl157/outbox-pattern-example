CREATE TABLE [dbo].[Outbox](
	[OutboxId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED([OutboxId] ASC),
	[RequestMessage] [nvarchar](max) NOT NULL,
	[QueuedDate] [datetime] NOT NULL DEFAULT (getdate()),
	[ProcessedDate] [datetime] NULL,
	[ProcessedStatus] [nvarchar](256) NOT NULL DEFAULT ('queued'),
	[ErrorMessage] [nvarchar](max) NULL
 )
 GO

CREATE TABLE [dbo].[Book] (
	[BookId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED([BookId] ASC),
	[Title] [nvarchar](512) NOT NULL,
	[Author] [nvarchar](512) NOT NULL
)
GO

CREATE PROCEDURE [dbo].[GetBooks]
AS
BEGIN
	SELECT Title, Author
	FROM Book
END
GO

CREATE PROCEDURE [dbo].[GetQueuedOutbox]
AS
BEGIN
	SELECT OutboxId, RequestMessage, QueuedDate, ProcessedDate, ProcessedStatus, ErrorMessage
	FROM Outbox
	WHERE ProcessedStatus = 'queued'
END
GO

CREATE PROCEDURE [dbo].[InsertOutbox] @RequestMessage nvarchar(max)
AS
BEGIN
	INSERT INTO Outbox(RequestMessage) VALUES (@RequestMessage)
END
GO

CREATE PROCEDURE [dbo].[SaveBook] @Title nvarchar(512), @Author nvarchar(512)
AS
BEGIN
	INSERT INTO Book (Title, Author) VALUES (@Title, @Author)
END
GO

CREATE PROCEDURE [dbo].[UpdateOutboxRecord] @outboxId INT, @ProcessedDate DATETIME, @ProcessedStatus NVARCHAR(256), @ErrorMessage NVARCHAR(max)
AS
BEGIN
	UPDATE Outbox
	SET ProcessedDate = @ProcessedDate,
		ProcessedStatus = @ProcessedStatus,
		ErrorMessage = @ErrorMessage
	WHERE OutboxId = @outboxId
END
GO