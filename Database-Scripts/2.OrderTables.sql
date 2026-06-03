-- ==========================================
-- SHIPPING ADDRESSES
-- ==========================================

CREATE TABLE ShippingAddresses
(
    ShippingAddressID INT PRIMARY KEY IDENTITY(1,1),

    CustomerID NVARCHAR(450) NOT NULL,

    FullName NVARCHAR(100) NOT NULL,
    Street NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    PostalCode NVARCHAR(20) NOT NULL,
    Country NVARCHAR(100) NOT NULL,

    CONSTRAINT FK_ShippingAddresses_AspNetUsers
        FOREIGN KEY (CustomerID)
        REFERENCES AspNetUsers(Id)
        ON DELETE CASCADE
);

-- ==========================================
-- DELIVERY PROVIDERS
-- ==========================================

CREATE TABLE DeliveryProviders
(
    ProviderID UNIQUEIDENTIFIER PRIMARY KEY
        DEFAULT NEWID(),

    ProviderName NVARCHAR(255) NOT NULL,

    Available BIT NOT NULL DEFAULT 1,

    CONSTRAINT UQ_DeliveryProviders_ProviderName
        UNIQUE (ProviderName)
);

-- ==========================================
-- DELIVERY METHODS
-- ==========================================

CREATE TABLE DeliveryMethods
(
    DeliveryMethodID INT PRIMARY KEY IDENTITY(1,1),

    ProviderID UNIQUEIDENTIFIER NOT NULL,

    MethodName NVARCHAR(100) NOT NULL,

    Description NVARCHAR(255) NULL,

    DeliveryTime NVARCHAR(100) NOT NULL,

    Price DECIMAL(18,2) NOT NULL,

    Available BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_DeliveryMethods_DeliveryProviders
        FOREIGN KEY (ProviderID)
        REFERENCES DeliveryProviders(ProviderID)
        ON DELETE NO ACTION,

    CONSTRAINT CK_DeliveryMethods_Price
        CHECK (Price >= 0)
);

-- ==========================================
-- ORDERS
-- ==========================================

CREATE TABLE Orders
(
    OrderID UNIQUEIDENTIFIER PRIMARY KEY
        DEFAULT NEWID(),

    CustomerID NVARCHAR(450) NOT NULL,

    -- Address snapshot
    ShippingFullName NVARCHAR(100) NOT NULL,
    ShippingStreet NVARCHAR(255) NOT NULL,
    ShippingCity NVARCHAR(100) NOT NULL,
    ShippingPostalCode NVARCHAR(20) NOT NULL,
    ShippingCountry NVARCHAR(100) NOT NULL,

    -- Delivery snapshot
    DeliveryProviderName NVARCHAR(255) NOT NULL,
    DeliveryMethodName NVARCHAR(100) NOT NULL,
    DeliveryPrice DECIMAL(18,2) NOT NULL,

    SubTotal DECIMAL(18,2) NOT NULL,

    Total AS (SubTotal + DeliveryPrice) PERSISTED,

    OrderDate DATETIME2 NOT NULL
        DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Orders_AspNetUsers
        FOREIGN KEY (CustomerID)
        REFERENCES AspNetUsers(Id),

    CONSTRAINT CK_Orders_SubTotal
        CHECK (SubTotal >= 0),

    CONSTRAINT CK_Orders_DeliveryPrice
        CHECK (DeliveryPrice >= 0)
);

-- ==========================================
-- ORDER ITEMS
-- ==========================================

CREATE TABLE OrderItems
(
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),

    OrderID UNIQUEIDENTIFIER NOT NULL,

    ProductID INT NOT NULL,

    -- Product snapshot
    ProductName NVARCHAR(255) NOT NULL,

    Quantity INT NOT NULL,

    UnitPrice DECIMAL(18,2) NOT NULL,

    LineTotal AS (Quantity * UnitPrice) PERSISTED,

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderID)
        REFERENCES Orders(OrderID)
        ON DELETE CASCADE,

    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductID)
        REFERENCES Products(ProductID),

    CONSTRAINT CK_OrderItems_Quantity
        CHECK (Quantity > 0),

    CONSTRAINT CK_OrderItems_UnitPrice
        CHECK (UnitPrice >= 0)
);
