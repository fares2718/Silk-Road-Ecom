-----------------------------------------------------------
-- 1) Create Database
-----------------------------------------------------------
IF DB_ID('SilkRoadDB') IS NULL
BEGIN
    CREATE DATABASE SilkRoadDB;
END
GO

USE SilkRoadDB;
GO

-----------------------------------------------------------
-- 2) Drop existing objects if they already exist
--    (Useful while rebuilding during course development)
-----------------------------------------------------------

-- Drop foreign keys first if needed by dropping tables in dependency order


-----------------------------------------------------------
-- 3) Create Categories Table
-----------------------------------------------------------
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryDescription NVARCHAR(255) NULL,

    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID),
    CONSTRAINT UQ_Categories_CategoryName UNIQUE (CategoryName)
)
GO

-----------------------------------------------------------
-- 4) Create Products Table
-----------------------------------------------------------
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    CategoryID INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,

    CONSTRAINT PK_Products PRIMARY KEY (ProductID),
    CONSTRAINT UQ_Products_ProductName UNIQUE (ProductName),
    CONSTRAINT CK_Products_ProductPrice CHECK (Price >= 0)
);
GO

ALTER TABLE Products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (CategoryID) 
REFERENCES Categories(CategoryID)
ON DELETE NO ACTION
ON UPDATE NO ACTION;

-----------------------------------------------------------
-- 5) Create ProductImages Table
-----------------------------------------------------------

CREATE TABLE ProductImages (
    ImageID INT IDENTITY(1,1) NOT NULL,
    ProductID INT NOT NULL,
    ImageURL NVARCHAR(255) NOT NULL,

    CONSTRAINT PK_ProductImages PRIMARY KEY (ImageID),
    CONSTRAINT UQ_ProductImages_ImageURL UNIQUE (ImageURL)
);
GO

ALTER TABLE ProductImages
ADD CONSTRAINT FK_ProductImages_Products
FOREIGN KEY (ProductID) 
REFERENCES Products(ProductID)
ON DELETE CASCADE
ON UPDATE NO ACTION;
GO


-----------------------------------------------------------
--6) Create Indexes
--    Useful for joins, filtering, and performance
-----------------------------------------------------------

CREATE INDEX IX_Categories_CategoryName ON Categories(CategoryName);

CREATE INDEX IX_Products_CategoryID ON Products(CategoryID);

CREATE INDEX IX_Products_Name ON Products(ProductName);

CREATE INDEX IX_ProductImages_ProductID ON ProductImages(ProductID);

-----------------------------------------------------------