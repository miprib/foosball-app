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
	GameID			int					not null	IDENTITY (1,1)
		PRIMARY KEY,	
    RightPlayerID   int					not null,
    Score           int,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID) ON DELETE CASCADE
);

CREATE TABLE tabLeftTournamentPlayer
(
	GameID			int					not null	IDENTITY (1,1)
		PRIMARY KEY,	
    LeftPlayerID    int					not null,
    Score           int,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID) ON DELETE CASCADE
        
);

CREATE TABLE tabTournamentGame
(
    GameID          int					not null	IDENTITY (1,1)
        PRIMARY KEY,
    Date            datetime            not null,
    TournamentID    int					not null
        REFERENCES tabTournament (TournamentID) ON DELETE CASCADE
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

INSERT INTO tabTournamentGame (Date, TournamentID)
	VALUES
	('2017-12-08', 1),
	('2017-12-08', 1),
	('2017-12-08', 1),
	('2017-12-08', 1),

	('2017-12-08', 1),
	('2017-12-08', 1),

	('2017-12-08', 1);

INSERT INTO tabRightTournamentPlayer (RightPlayerID, Score, TournamentID)
	VALUES
	(1, 3, 1),
	(3, 3, 1),
	(5, 3, 1),
	(7, 2, 1),

	(1, 1, 1),
	(5, 3, 1),

	(3, 3, 1);

INSERT INTO tabLeftTournamentPlayer (LeftPlayerID, Score, TournamentID)
	VALUES
	(2, 0, 1),
	(4, 0, 1),
	(6, 0, 1),
	(8, 3, 1),

	(3, 3, 1),
	(8, 1, 1),

	(5, 2, 1);
