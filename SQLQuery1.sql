CREATE TABLE Games (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,              
    Developer NVARCHAR(100) NOT NULL,       
    Genre NVARCHAR(50) NOT NULL,              
    Date DATE NOT NULL,               
    GameMode NVARCHAR(20) NOT NULL,            
    Copies INT NOT NULL             
);

