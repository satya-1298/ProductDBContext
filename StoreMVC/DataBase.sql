CREATE DATABASE StoreDB
CREATE TABLE Store
(
    ProductId INT IDENTITY(1,1) PRIMARY KEY, 
    Code VARCHAR(50) UNIQUE NOT NULL,         
    Name VARCHAR(250) NOT NULL,            
    Description VARCHAR(4000),              
    ExpiryDate DATE CHECK (ExpiryDate >= GETDATE()), 
    Category VARCHAR(50) ,
    Image VARCHAR(MAX),                    
    Status VARCHAR(10) DEFAULT 'Active',      
    CreationDate DATETIME DEFAULT GETDATE()  
);
select * from Store
drop table Store
INSERT INTO Store(Code,Name,Description,ExpiryDate,Category,Image,Status,CreationDate) 
Values('fdsdfv','Satya','hjdfjgvbevjhc','2024-12-20','Category A','fefvev','Active','')


-----------------------------------StoreProcedure--------------------------------------------
 
 -----------------------------------------Creation------------------------------------
 CREATE PROCEDURE SPProduct
(

   @Code VARCHAR(50) ,         
   @Name VARCHAR(250) ,            
   @Description VARCHAR(max),              
   @ExpiryDate DATE , 
   @Category VARCHAR(50) ,
   @Image VARCHAR(MAX),                    
   @Status VARCHAR(10),      
   @CreationDate DATE
)
As Begin
--BEGIN TRY
--	INSERT INTO Store(Code,Name,Description,ExpiryDate,Category,Image,Status,CreationDate)VALUES
--	(@Code,@Name,@Description,@ExpiryDate,@Category,@Image,@Status,@CreationDate);
--END TRY
--BEGIN CATCH
--	SELECT
--			ERROR_PROCEDURE() AS ErrorProcedure
--END CATCH
--END
IF EXISTS (SELECT * FROM Store WHERE Code=@Code) 
	UPDATE Store SET Code=@Code,Name=@Name, Description=@Description,
	ExpiryDate=@ExpiryDate,Category=@Category,Image=@Image,Status=@Status ,CreationDate=@CreationDate WHERE Code=@Code;
ELSE 
	INSERT INTO Store(Code,Name,Description,ExpiryDate,Category,Image,Status,CreationDate)VALUES
		(@Code,@Name,@Description,@ExpiryDate,@Category,@Image,@Status,@CreationDate);
RETURN
END

Drop Procedure SPProduct
---------------------------------Retrieve Data------------------------------------
CREATE PROCEDURE SPRetrieveAllData
AS BEGIN

	SELECT*FROM Store

END

DROP PROCEDURE SPRetrieveAllData

EXEC SPRetrieveAllData