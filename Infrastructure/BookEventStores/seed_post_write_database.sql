CREATE DATABASE post_write;

-- post_write.dbo.BookAggregate definition

-- Drop table

-- DROP TABLE post_write.dbo.BookAggregate;

CREATE TABLE post_write.dbo.BookAggregate (
	Id uniqueidentifier NOT NULL,
	Version int NOT NULL,
	CreatedDate datetime NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT BookAggregate_PK PRIMARY KEY (Id)
);

-- post_write.dbo.BookEvent definition

-- Drop table

-- DROP TABLE post_write.dbo.BookEvent;

CREATE TABLE post_write.dbo.BookEvent (
	Id uniqueidentifier NOT NULL,
	AggregateId uniqueidentifier NOT NULL,
	Version int NOT NULL,
	CreateDate datetime NOT NULL,
	[Data] nvarchar(4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] varchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT BookEvent_PK PRIMARY KEY (Id)
);
