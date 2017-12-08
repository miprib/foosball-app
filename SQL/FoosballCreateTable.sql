DROP TABLE IF EXISTS tabTournamentGame;
DROP TABLE IF EXISTS tabLeftTournamentPlayer;
DROP TABLE IF EXISTS tabRightTournamentPlayer;
DROP TABLE IF EXISTS tabTournament;
DROP TABLE IF EXISTS tabUser;
DROP VIEW  IF EXISTS viewResults;

CREATE TABLE tabUser 
(
    UserID      int						not null 
        PRIMARY KEY,
    Name        nvarchar(50)            not null
);

CREATE TABLE tabTournament 
(
    TournamentID    int					not null
        PRIMARY KEY,
    Winner      nvarchar(50),	-- Galima null, nes reikia tournament sukurti pirmiau uz zaidimus. Veliau bus paupdatinas table :)
    UserID      int	-- Same shit
        REFERENCES tabUser (UserID)
);

CREATE TABLE tabRightTournamentPlayer 
(
	GameID			int					not null
		PRIMARY KEY,	
    RightPlayerID   int					not null,
    Score           int                 not null,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID)
);

CREATE TABLE tabLeftTournamentPlayer
(
	GameID			int					not null
		PRIMARY KEY,	
    LeftPlayerID    int					not null,
    Score           int                 not null,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID)
        
);

CREATE TABLE tabTournamentGame
(
    GameID          int					not null
        PRIMARY KEY,
    Date            datetime            not null,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID),
    LeftPlayerID    int					not null,
    RightPlayerID   int					not null,
);

--INSERTS
INSERT INTO tabUser (UserID, Name)
	VALUES
	(1, 'Mitasiunas'),
	(2, 'Studentai'),
	(3, 'PSI'),
	(4, 'Gyvenimas'),
	(5, 'Bitcoin'),
	(6, 'Euras'),
	(7, 'Miegas'),
	(8, 'Mokslai');

INSERT INTO tabTournament (TournamentID, Winner, UserID)
	VALUES
	(1, 'PSI', 3);

INSERT INTO tabTournamentGame (GameID, Date, TournamentID, LeftPlayerID, RightPlayerID)
	VALUES
	(1, '2017-12-08', 1, 1, 2),
	(2, '2017-12-08', 1, 3, 4),
	(3, '2017-12-08', 1, 5, 6),
	(4, '2017-12-08', 1, 7, 8),

	(5, '2017-12-08', 1, 1, 3),
	(6, '2017-12-08', 1, 5, 8),

	(7, '2017-12-08', 1, 3, 5);

INSERT INTO tabRightTournamentPlayer (GameID, RightPlayerID, Score, TournamentID)
	VALUES
	(1, 1, 3, 1),
	(2, 3, 3, 1),
	(3, 5, 3, 1),
	(4, 7, 2, 1),

	(5, 1, 1, 1),
	(6, 5, 3, 1),

	(7, 3, 3, 1);

INSERT INTO tabLeftTournamentPlayer (GameID, LeftPlayerID, Score, TournamentID)
	VALUES
	(1, 2, 0, 1),
	(2, 4, 0, 1),
	(3, 6, 0, 1),
	(4, 8, 3, 1),

	(5, 3, 3, 1),
	(6, 8, 1, 1),

	(7, 5, 2, 1);
