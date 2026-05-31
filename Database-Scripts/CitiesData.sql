INSERT INTO Cities(StateID,CityName)
SELECT StateID,(StateName+'C-1') FROM States

INSERT INTO Cities(StateID,CityName)
SELECT StateID,(StateName+'C-2') FROM States
