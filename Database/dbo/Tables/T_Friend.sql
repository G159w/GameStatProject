CREATE TABLE [dbo].[T_Friend] (
    [user_id]   BIGINT NOT NULL,
    [friend_id] BIGINT NOT NULL,
    CONSTRAINT [PK_T_Friends] PRIMARY KEY CLUSTERED ([user_id] ASC, [friend_id] ASC),
    CONSTRAINT [FK_T_Friend_T_User] FOREIGN KEY ([user_id]) REFERENCES [dbo].[T_User] ([id]),
    CONSTRAINT [FK_T_Friend_T_User1] FOREIGN KEY ([friend_id]) REFERENCES [dbo].[T_User] ([id])
);





