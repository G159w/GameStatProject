CREATE TABLE [dbo].[T_Lol_Stat] (
    [id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [user_game_id]   BIGINT       NOT NULL,
    [soloTier]       VARCHAR (50) NOT NULL,
    [soloRank]       VARCHAR (50) NOT NULL,
    [soloNameLeague] VARCHAR (50) NOT NULL,
    [soloWins]       INT          NOT NULL,
    [soloLosses]     INT          NOT NULL,
    [soloLP]         INT          NOT NULL,
    [flexTier]       VARCHAR (50) NOT NULL,
    [flexRank]       VARCHAR (50) NOT NULL,
    [flexNameLeague] VARCHAR (50) NOT NULL,
    [flexWins]       INT          NOT NULL,
    [flexLosses]     INT          NOT NULL,
    [flexLP]         INT          NOT NULL,
    CONSTRAINT [PK_T_Lol_Stat] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_Lol_Stat_T_User_Game] FOREIGN KEY ([user_game_id]) REFERENCES [dbo].[T_User_Game] ([id])
);

