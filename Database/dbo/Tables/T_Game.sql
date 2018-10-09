CREATE TABLE [dbo].[T_Game] (
    [id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [short_name]       VARCHAR (255) NOT NULL,
    [display_name]     VARCHAR (255) NOT NULL,
    [api_key_required] BIT           NOT NULL,
    CONSTRAINT [PK_T_Game] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_T_Game] UNIQUE NONCLUSTERED ([short_name] ASC),
    CONSTRAINT [IX_T_Game_1] UNIQUE NONCLUSTERED ([display_name] ASC)
);



