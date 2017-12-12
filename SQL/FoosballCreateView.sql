-- Views
CREATE VIEW viewResults
    AS SELECT A.TournamentID, A.GameID, A.LeftPlayerID, A.Score AS 'LeftScore', B.RightPlayerID, B.Score AS 'RightScore' 
	FROM tabLeftTournamentPlayer AS A, tabRightTournamentPlayer AS B
	WHERE A.GameID = B.GameID;