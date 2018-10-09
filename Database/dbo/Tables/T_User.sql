CREATE TABLE [dbo].[T_User] (
    [id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [email]         VARCHAR (255) NOT NULL,
    [password_salt] VARCHAR (255) NOT NULL,
    [password_hash] VARCHAR (255) NOT NULL,
    [register_date] DATETIME      NOT NULL,
    [username]      VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_T_User] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_T_User] UNIQUE NONCLUSTERED ([email] ASC),
    CONSTRAINT [IX_T_User_1] UNIQUE NONCLUSTERED ([username] ASC)
);



