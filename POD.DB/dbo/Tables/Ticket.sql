CREATE TABLE [dbo].[Ticket] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [OrganizationID] UNIQUEIDENTIFIER NOT NULL,
    [SerialNumber]   NVARCHAR(250)     NOT NULL,
    [TicketNumber]   INT              NOT NULL,
    [Plant]          VARCHAR (10)     NULL,
    [DeliveryDate]   DATETIME         NULL,
    [HaulierCode]    NVARCHAR(250)     NULL,
    [HaulierReg]     NVARCHAR(250)     NULL,
    [QuoteNumber]    NVARCHAR(250)     NULL,
    [InvoiceNumber]  NVARCHAR(250)     NULL,
    [PayerAccount]   NVARCHAR(250)     NULL,
    [DeletionInd]    VARCHAR (2)      NULL,
    CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Ticket_Organization] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[Organization] ([ID])
);

