-- Below contains the the tables mock data
-- set-up for the shopping cart project

-- User table
CREATE TABLE IF NOT EXISTS User(
	UserId varchar(30),
	Username varchar(50) NOT NULL UNIQUE,
	Name varchar(50) NOT NULL,
	Password varchar(300) NOT NULL,
	PRIMARY KEY (UserId)
);

INSERT into User(UserId, Username, Name, Password)
VALUES('U001','John001','John Doe1','Password'),
    ('U002','John002','John Doe2','Password'), 
    ('U003','Jane003','Jane Doe3','Password'), 
    ('U004','Jane004','Jane Doe4','Password');

-- Product table
CREATE TABLE IF NOT EXISTS Product(
    ProductId varchar(30),
    Name varchar(50) NOT NULL,
    Description varchar(250) NOT NULL,
    Img varchar(250) NOT NULL,
    Price int NOT NULL,
    ReviewRating int,
    PRIMARY KEY(ProductId)
);

INSERT INTO Product(ProductId, Name, Description, Img, Price) 
VALUES ('P001', '.NET Charts','Brings powerful charting capabilities to your .NET applications','pie-chart.png',99),
    ('P002', '.NET PayPal', 'Integrate your .NET apps with PayPal the easy way!','paypal.png', 69),
    ('P003', '.NET ML','Supercharged .NET machine learning libraries','machine-learning.png',299),
    ('P004', '.NET Analytics','Perform data mining and analytics easiliy in .NET','statistics.png',299),
    ('P005', '.NET Logger','Logs and aggregates events easily in your .NET apps','log.png',49),
    ('P006', '.NET Numerics','Powerful numerical methods for your .NET simulations','maths.png',299);

-- Review table
CREATE TABLE IF NOT EXISTS Review(
    UserId varchar(30),  
    ProductId varchar(30), 
    Rating int,
    PRIMARY KEY (UserId, ProductId),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

-- drop table Review;
INSERT INTO Review (UserID, ProductId, Rating)
VALUES('U001','P001',4),
    ('U001','P002',4),
    ('U002','P003',4),
    ('U002','P005',3),
    ('U003','P002',4),
    ('U003','P006',1),
    ('U004','P003',4),
    ('U004','P002',1);

-- Purchase Order table
CREATE TABLE IF NOT EXISTS PurchaseOrder(
    PurchaseId varchar(38), -- GUID
    UserId varchar(30) NOT NULL,  
    ProductId varchar(30) NOT NULL,
    PurchaseQty int NOT NULL,
    PurchaseDate varchar(30) NOT NULL,
    PRIMARY KEY (PurchaseId),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

INSERT INTO PurchaseOrder(PurchaseId, UserId, ProductId, PurchaseQty, PurchaseDate) 
VALUES('80cff2a2-d9b2-11ed-80a4-5254006b6f85', 'U001', 'P001', 2, '08 Apr 2019'),
    ('80cff639-d9b2-11ed-80a4-5254006b6f85', 'U001', 'P002', 1, '08 Apr 2019'),
    ('80cff758-d9b2-11ed-80a4-5254006b6f85', 'U001', 'P004', 1, '04 Sep 2019');

-- Purchase List table (Itemised list of each purchased item)
CREATE TABLE IF NOT EXISTS PurchaseList(
    Id int AUTO_INCREMENT,
    PurchaseId varchar(38),
    ActivationCode varchar(38) Unique, -- GUID is 38 characters
    PRIMARY KEY (Id),
    FOREIGN KEY(PurchaseId)
        REFERENCES PurchaseOrder(PurchaseId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

INSERT INTO PurchaseList(PurchaseId)
VALUES ('80cff2a2-d9b2-11ed-80a4-5254006b6f85'),
    ('80cff2a2-d9b2-11ed-80a4-5254006b6f85'),
    ('80cff639-d9b2-11ed-80a4-5254006b6f85'),
    ('80cff758-d9b2-11ed-80a4-5254006b6f85');

UPDATE PurchaseList SET ActivationCode=(SELECT uuid());;

-- CartItem table
CREATE TABLE IF NOT EXISTS CartItem(
    CartItemId int AUTO_INCREMENT,
    UserId varchar(30) NOT NULL,
    ProductId varchar(30) NOT NULL,
    Quantity int NOT NULL,
    PRIMARY KEY (CartItemId),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

INSERT INTO CartItem(UserId, ProductId, Quantity)
VALUES ('U001','P001',1),
    ('U002','P002',2),
    ('U003','P003',3),
    ('U004','P004',4),
    ('U001','P005',5),
    ('U002','P006',6);
