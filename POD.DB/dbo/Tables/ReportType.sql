CREATE TABLE [dbo].[ReportType] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [ReportTypeName] NVARCHAR (50)    NOT NULL,
    [ReportTypeID]   INT              NOT NULL,
    CONSTRAINT [PK_ReportType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

