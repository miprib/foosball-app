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
    UserID			int					not null
        REFERENCES tabUser (UserID),
	Winner      nvarchar(50)			-- Galima null, nes reikia tournament sukurti pirmiau uz zaidimus. Veliau bus paupdatinas table :)
);

CREATE TABLE tabRightTournamentPlayer 
(
	GameID			int					not null
		PRIMARY KEY,	
    RightPlayerID   int					not null,
    Score           int,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID)
);

CREATE TABLE tabLeftTournamentPlayer
(
	GameID			int					not null
		PRIMARY KEY,	
    LeftPlayerID    int					not null,
    Score           int,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID)
        
);

CREATE TABLE tabTournamentGame
(
    GameID          int					not null
        PRIMARY KEY,
    Date            datetime            not null,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID)
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

INSERT INTO tabTournament (TournamentID, UserID, Winner)
	VALUES
	(1, 2, 'PSI');

INSERT INTO tabTournamentGame (GameID, Date, TournamentID)
	VALUES
	(1, '2017-12-08', 1),
	(2, '2017-12-08', 1),
	(3, '2017-12-08', 1),
	(4, '2017-12-08', 1),

	(5, '2017-12-08', 1),
	(6, '2017-12-08', 1),

	(7, '2017-12-08', 1);

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
