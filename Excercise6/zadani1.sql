--task 1 
--Row-Level Security (RLS)
--Azure SQL Data Warehouse doesn't support EXECUTE AS USER,
--D3aler1!password
--D3aler2@password
--begin tran;
--car
CREATE TABLE dbo.Car (
Id int NOT NULL PRIMARY KEY,
Name varchar(100)
);
GO
INSERT INTO Car VALUES (1, 'Car1');
INSERT INTO Car VALUES (2, 'Car2');
INSERT INTO Car VALUES (3, 'Car3');
INSERT INTO Car VALUES (4, 'Car4');
INSERT INTO Car VALUES (5, 'Car5');
INSERT INTO Car VALUES (6, 'Car6');
GO

--region
CREATE TABLE dbo.Region(
Id int NOT NULL PRIMARY KEY,
Name varchar(100)
);
GO
INSERT INTO Region VALUES (1, 'Region1');
INSERT INTO Region VALUES (2, 'Region2');
INSERT INTO Region VALUES (3, 'Region3');
GO

--dealer
CREATE TABLE dbo.Dealer(
Id int NOT NULL PRIMARY KEY,
LoginSql sysname
);
GO
--SELECT * from Dealer

INSERT INTO Dealer VALUES (1, 'Dealer1');
INSERT INTO Dealer VALUES (2, 'Dealer2');

--price
CREATE TABLE dbo.Price(
Id int identity(1,1),
IdCar int FOREIGN KEY REFERENCES Car(Id),
IdRegion int FOREIGN KEY REFERENCES Region(Id),
IdDealer int FOREIGN KEY REFERENCES Dealer(Id),
Cost int
);
GO

INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (1,1,1,100);
INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (3,3,1,300);
INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (5,2,1,500);

INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (2,2,2,200);
INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (4,1,2,400);
INSERT INTO Price(IdCar,IdRegion,IdDealer,Cost) VALUES (6,3,2,600);
GO

--select * from dbo.Price

GRANT SELECT ON Price TO Dealer1;  
GRANT SELECT ON Price TO Dealer2;  
GO
--procedure to add new record in price for given info
--CREATE PROCEDURE INSERT_PRICE(@IdCar int, @IdRegion int, @IdDealer int, @Cost int)
--AS
--Insert into Price(IdCar,IdRegion,IdDealer, Cost) values(@IdCar,@IdRegion,@IdDealer,@Cost);
--GO

--GRANT EXECUTE ON INSERT_PRICE TO Dealer1;
--GRANT EXECUTE ON INSERT_PRICE TO Dealer2;
--GO

CREATE SCHEMA Security;  
GO  

--CREATE FUNCTION Security.fn_securitypredicate(@DealerId AS int)  
--    RETURNS @T TABLE(fn_securitypredicate_result bit)
--WITH SCHEMABINDING  
--AS  
--BEGIN
--		declare @Dealer sysname;
--		declare @retVal bit;
--		SELECT @Dealer = LoginSql from dbo.Dealer where Id = @DealerId;
--		SET @retVal = (SELECT 1 WHERE (@Dealer = USER_NAME() OR USER_NAME() = 'dbo' ))
--		INSERT INTO @T(fn_securitypredicate_result) values (@retVal)		 
--		RETURN
--END
--GO

CREATE FUNCTION Security.fn_securitypredicate(@DealerId AS int)  
    RETURNS TABLE  
WITH SCHEMABINDING  
AS  
    RETURN SELECT 1 AS fn_securitypredicate_result
	WHERE USER_NAME() = 'dbo' OR EXISTS(SELECT LoginSql from dbo.Dealer d join dbo.Price p on (p.IdDealer = d.Id) where d.Id = @DealerId and d.LoginSql = USER_NAME() ) ;  
GO

CREATE SECURITY POLICY PriceFilter  
ADD FILTER PREDICATE Security.fn_securitypredicate(IdDealer)
ON dbo.Price  
WITH (STATE = ON); 
GO



--DROP SECURITY POLICY PriceFilter;
--DROP TABLE Car;
--DROP TABLE Region;
--DROP TABLE Dealer;
--DROP TABLE Price;
--DROP FUNCTION Security.fn_securitypredicate;
--DROP SCHEMA Security;

--rollback;