CREATE TABLE [dbo].[T_User_Game] (
    [id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [user_id]  BIGINT        NOT NULL,
    [game_id]  BIGINT        NOT NULL,
    [username] VARCHAR (255) NOT NULL,
    [api_key]  VARCHAR (255) NULL,
    CONSTRAINT [PK_T_User_Game] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_User_Game_T_Game] FOREIGN KEY ([game_id]) REFERENCES [dbo].[T_Game] ([id]),
    CONSTRAINT [FK_T_User_Game_T_User] FOREIGN KEY ([user_id]) REFERENCES [dbo].[T_User] ([id]),
    CONSTRAINT [IX_T_User_Game] UNIQUE NONCLUSTERED ([game_id] ASC, [username] ASC)
);





