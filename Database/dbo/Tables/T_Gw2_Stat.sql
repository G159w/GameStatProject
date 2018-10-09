CREATE TABLE [dbo].[T_Gw2_Stat] (
    [id]                 BIGINT NOT NULL,
    [pvp_rank]           INT    NOT NULL,
    [pvp_rank_points]    INT    NOT NULL,
    [pvp_rank_rollovers] INT    NOT NULL,
    [wins]               INT    NOT NULL,
    [losses]             INT    NOT NULL,
    [desertions]         INT    NOT NULL,
    [byes]               INT    NOT NULL,
    [forfeits]           INT    NOT NULL,
    [user_game_id]       BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.T_Gw2_Stat] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_Gw2_Stat_T_User_Game] FOREIGN KEY ([user_game_id]) REFERENCES [dbo].[T_User_Game] ([id])
);





