
CREATE TABLE #Tagihan (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Nama NVARCHAR(50) NOT NULL,       
    JatuhTempo DATE NOT NULL,         
    Jumlah DECIMAL(18, 2) NOT NULL,   
    Dibayar DECIMAL(18, 2) NOT NULL DEFAULT 0 
);
INSERT INTO #Tagihan (Nama, JatuhTempo, Jumlah) 
VALUES 
    ('Tagihan#1', '2023-01-10', 165000),
    ('Tagihan#2', '2023-02-15', 80000),
    ('Tagihan#3', '2023-01-20', 130000),
    ('Tagihan#4', '2023-03-25', 416000),
    ('Tagihan#5', '2023-02-10', 95500),
    ('Tagihan#6', '2023-08-17', 523000);


SELECT 
    SUM(CASE WHEN JatuhTempo > '2023-03-25' THEN Jumlah ELSE 0 END) AS TotalUndue,
    SUM(CASE WHEN JatuhTempo <= '2023-03-25' THEN Jumlah ELSE 0 END) AS TotalOverdue
FROM #Tagihan
WHERE Dibayar < Jumlah;