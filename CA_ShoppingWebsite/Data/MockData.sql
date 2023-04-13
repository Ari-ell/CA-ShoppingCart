-- Below contains the the tables mock data
-- set-up for the shopping cart project

-- User table
CREATE TABLE IF NOT EXISTS User(
	UserId int AUTO_INCREMENT NOT NULL,
	Username varchar(50) NOT NULL,
	Name varchar(50) NOT NULL,
	Password varchar(300) NOT NULL,
	PRIMARY KEY (UserId)
);

INSERT into User
VALUES(1, 'tes1','test1','test1'),
    (2, 'test2','test2','test2'), 
    (3, 'test3','test3','test3'), 
    (4, 'test4','test4','test4');


-- Product table
CREATE TABLE IF NOT EXISTS Product(
    ProductId int NOT NULL,
    Name varchar(50) NOT NULL,
    Description varchar(250) NOT NULL,
    Img varchar(250) NOT NULL,
    Price int NOT NULL,
    ReviewRating int,
    PRIMARY KEY(ProductId)
);

INSERT INTO Product(ProductId, Name, Description, Img, Price) 
VALUES (1,'.NET Charts','Brings powerful charting capabilities to your .NET applications','/wwwroot/assets/images/',99),
    (2, '.NET PayPal', 'Integrate your .NET apps with PayPal the easy way!','/wwwroot/assets/images/', 69),
    (3, '.NET ML','Supercharged machine learning libraries','/wwwroot/assets/images/',299),
    (4, '.NET Analytics','Perform data mining and analytics easiliy in .NET','/wwwroot/assets/images/',299),
    (5, '.NET Logger','Logs and aggregates events easily in your .NET apps','/wwwroot/assets/images/',49),
    (6, '.NET Numerics','Powerful numerical methods for your .NET simulations','/wwwroot/assets/images/',299);


-- Review table
CREATE TABLE IF NOT EXISTS Review(
    UserId int,  
    ProductId int, 
    Rating int,
    PRIMARY KEY (UserId, ProductId)
);

INSERT INTO Review 
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
    Id int AUTO_INCREMENT PRIMARY KEY,
    PurchaseId varchar(38), -- GUID
    UserId int NOT NULL,  
    ProductId int NOT NULL,
    PurchaseQty int NOT NULL,
    PurchaseDate varchar(30) NOT NULL
);

INSERT INTO PurchaseOrder(UserId, ProductId, PurchaseQty, PurchaseDate) 
VALUES(1, 1, 2, '08 April 2019'),
    (1, 2, 1, '08 April 2019'),
    (1, 4, 1, '04 September 2019');

UPDATE PurchaseOrder SET PurchaseId=(SELECT uuid());

-- Purchase List table (Itemised list of each purchased item)
CREATE TABLE IF NOT EXISTS PurchaseList(
    Id int AUTO_INCREMENT PRIMARY KEY,
    ProductId int,
    PurchaseId varchar(38),
    ActivationCode varchar(38) Unique, -- GUID is 38 characters
    FOREIGN KEY(PurchaseId) 
        REFERENCES PurchaseOrder(PurchaseId)
        ON DELETE CASCADE
);

INSERT INTO PurchaseList(ProductId, PurchaseId)
VALUES (1, '80cff2a2-d9b2-11ed-80a4-5254006b6f85'),
    (1, '80cff2a2-d9b2-11ed-80a4-5254006b6f85'),
    (2, '80cff639-d9b2-11ed-80a4-5254006b6f85'),
    (4,'80cff758-d9b2-11ed-80a4-5254006b6f85');

UPDATE PurchaseList SET ActivationCode=(SELECT uuid());

-- Cart table
CREATE TABLE IF NOT EXISTS Cart(
    UserId int NOT NULL,
    ProductId int NOT NULL,
    ProductQty int NOT NULL,
    PRIMARY KEY(UserId, ProductId)
);
