CREATE TABLE [dbo].[T_R6_Stat] (
    [id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [user_game_id]    BIGINT        NOT NULL,
    [player_level]    BIGINT        NOT NULL,
    [ranked_wins]     BIGINT        NOT NULL,
    [ranked_losses]   BIGINT        NOT NULL,
    [ranked_wlr]      FLOAT (53)    NOT NULL,
    [ranked_kills]    BIGINT        NOT NULL,
    [ranked_deaths]   BIGINT        NOT NULL,
    [ranked_kd]       FLOAT (53)    NOT NULL,
    [ranked_playtime] VARCHAR (255) NOT NULL,
    [casual_wins]     BIGINT        NOT NULL,
    [casual_losses]   BIGINT        NOT NULL,
    [casual_wlr]      FLOAT (53)    NOT NULL,
    [casual_kills]    BIGINT        NOT NULL,
    [casual_deaths]   BIGINT        NOT NULL,
    [casual_kd]       FLOAT (53)    NOT NULL,
    [casual_playtime] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_T_R6_Stat] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_R6_Stat_T_User_Game] FOREIGN KEY ([user_game_id]) REFERENCES [dbo].[T_User_Game] ([id])
);



