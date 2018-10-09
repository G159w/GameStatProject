CREATE TABLE [dbo].[T_Elite_Stat] (
    [user_game_id]        BIGINT NOT NULL,
    [combat_rank]         INT    NOT NULL,
    [trader_rank]         INT    NOT NULL,
    [explorer_rank]       INT    NOT NULL,
    [cqc_rank]            INT    NOT NULL,
    [federation_rank]     INT    NOT NULL,
    [empire_rank]         INT    NOT NULL,
    [combat_progress]     INT    NOT NULL,
    [trader_progress]     INT    NOT NULL,
    [explorer_progress]   INT    NOT NULL,
    [cqc_progress]        INT    NOT NULL,
    [federation_progress] INT    NOT NULL,
    [empire_progress]     INT    NOT NULL,
    [id]                  BIGINT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_T_Elite_Stat_1] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_Elite_Stat_T_User_Game] FOREIGN KEY ([user_game_id]) REFERENCES [dbo].[T_User_Game] ([id])
);



