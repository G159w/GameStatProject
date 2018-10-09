CREATE TABLE [dbo].[T_Fortnite] (
    [id]           BIGINT     IDENTITY (1, 1) NOT NULL,
    [soloTop1]     INT        NOT NULL,
    [soloTop10]    INT        NOT NULL,
    [soloTop25]    INT        NOT NULL,
    [duoTop1]      INT        NOT NULL,
    [duoTop5]      INT        NOT NULL,
    [duoTop12]     INT        NOT NULL,
    [squadTop1]    INT        NOT NULL,
    [squadTop3]    INT        NOT NULL,
    [squadTop6]    INT        NOT NULL,
    [matches]      INT        NOT NULL,
    [kd]           FLOAT (53) NOT NULL,
    [kills]        INT        NOT NULL,
    [wins]         INT        NOT NULL,
    [winPercent]   INT        NOT NULL,
    [user_game_id] BIGINT     NOT NULL,
    CONSTRAINT [PK_T_Fortnite] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_T_Fortnite_T_User_Game] FOREIGN KEY ([user_game_id]) REFERENCES [dbo].[T_User_Game] ([id])
);





