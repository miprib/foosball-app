-- Views
CREATE VIEW viewResults
    AS SELECT A.TournamentID, A.GameID, A.LeftPlayerID, A.Score AS 'Left Score', B.RightPlayerID, B.Score AS 'Right Score' 
	FROM tabLeftTournamentPlayer AS A, tabRightTournamentPlayer AS B
	WHERE A.GameID = B.GameID;