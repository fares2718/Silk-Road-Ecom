INSERT INTO States(CountryID,StateName)
SELECT CountryID,(CountryName+'S-1') FROM Countries

INSERT INTO States(CountryID,StateName)
SELECT CountryID,(CountryName+'S-2') FROM Countries