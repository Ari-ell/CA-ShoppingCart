-- Below contains the the tables mock data
-- set-up for the shopping cart project

-- User table
CREATE TABLE IF NOT EXISTS User(
	UserId int AUTO_INCREMENT,
	Username varchar(50) NOT NULL UNIQUE,
	Name varchar(50) NOT NULL,
	Password varchar(300) NOT NULL,
	PRIMARY KEY (UserId)
);

INSERT into User(Username,Name,Password)
VALUES('test1','test1','test1'),
    ('test2','test2','test2'), 
    ('test3','test3','test3'), 
    ('test4','test4','test4');

-- Product table
CREATE TABLE IF NOT EXISTS Product(
    ProductId int,
    Name varchar(50) NOT NULL,
    Description varchar(250) NOT NULL,
    Img varchar(250) NOT NULL,
    Price int NOT NULL,
    ReviewRating int,
    PRIMARY KEY(ProductId)
);

INSERT INTO Product(ProductId, Name, Description, Img, Price) 
VALUES (1, '.NET Charts','Brings powerful charting capabilities to your .NET applications','pie-chart.png',99),
    (2, '.NET PayPal', 'Integrate your .NET apps with PayPal the easy way!','paypal.png', 69),
    (3, '.NET ML','Supercharged .NET machine learning libraries','machine-learning.png',299),
    (4, '.NET Analytics','Perform data mining and analytics easiliy in .NET','statistics.png',299),
    (5, '.NET Logger','Logs and aggregates events easily in your .NET apps','log.png',49),
    (6, '.NET Numerics','Powerful numerical methods for your .NET simulations','maths.png',299);

-- Review table
CREATE TABLE IF NOT EXISTS Review(
    ReviewId int AUTO_INCREMENT,
    UserId int,  
    ProductId int, 
    Rating int,
    PRIMARY KEY (ReviewId, UserId, ProductId),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

-- drop table Review;
INSERT INTO Review (UserID, ProductId, Rating)
VALUES(1,1,4),
    (1,2,4),
    (2,3,4),
    (2,5,3),
    (3,2,4),
    (3,6,1),
    (4,3,4),
    (4,2,1);

-- Purchase Order table
CREATE TABLE IF NOT EXISTS PurchaseOrder(
    PurchaseId varchar(38), -- GUID
    UserId int NOT NULL,  
    ProductId int NOT NULL,
    PurchaseQty int NOT NULL,
    PurchaseDate varchar(30) NOT NULL,
    PRIMARY KEY (PurchaseId),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

INSERT INTO PurchaseOrder(PurchaseId, UserId, ProductId, PurchaseQty, PurchaseDate) 
VALUES('80cff2a2-d9b2-11ed-80a4-5254006b6f85', 1, 1, 2, '08 Apr 2019'),
    ('80cff639-d9b2-11ed-80a4-5254006b6f85', 1, 2, 1, '08 Apr 2019'),
    ('80cff758-d9b2-11ed-80a4-5254006b6f85', 1, 4, 1, '04 Sep 2019');

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

-- Cart table
CREATE TABLE IF NOT EXISTS Cart(
    CartId int AUTO_INCREMENT,
    UserId int NOT NULL,
    PRIMARY KEY(CartId),
    FOREIGN KEY(UserId) REFERENCES User(UserId) ON DELETE CASCADE
);

INSERT INTO Cart(UserId, CartId)
VALUES (1, 1),
    (2, 2),
    (3, 3),
    (4, 4);

-- CartItem table
CREATE TABLE IF NOT EXISTS CartItem(
    CartItemId int AUTO_INCREMENT,
    CartId int NOT NULL,
    ProductId int NOT NULL,
    Quantity int NOT NULL,
    PRIMARY KEY (CartItemId),
    FOREIGN KEY (CartId) REFERENCES Cart(CartId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId) ON DELETE CASCADE
);

INSERT INTO CartItem(CartItemId, CartId, ProductId, Quantity)
VALUES (1, 1, 1, 3),
    (2, 1, 2, 5),
    (3, 1, 3, 1),
    (4, 2, 2, 2),
    (5, 3, 3, 3),
    (6, 4, 4, 4);