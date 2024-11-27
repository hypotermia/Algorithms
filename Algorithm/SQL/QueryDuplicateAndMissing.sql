-- duplicate check 
select * from (SELECT field_name,
           ROW_NUMBER() OVER (PARTITION BY field_name ORDER BY field_name) AS RowNum
    FROM TableA) x
    where RowNum = 1 
-- end here

--menemukan data yang missing dari table B
select * from TableA a 
left join tableb b on a.id = b.id 
where b.id is null
--end here 

--using CTE and eliminate duplicate from tableA 
WITH TableA_Rank AS (
    SELECT field_name,
           ROW_NUMBER() OVER (PARTITION BY field_name ORDER BY field_name) AS RowNum
    FROM TableA
),
TableB_Rank AS (
    SELECT field_name,
           ROW_NUMBER() OVER (PARTITION BY field_name ORDER BY field_name) AS RowNum
    FROM TableB
)
SELECT A.field_name
FROM TableA_Rank A
LEFT JOIN TableB_Rank B ON A.field_name = B.field_name
WHERE B.field_name IS NULL
  AND A.RowNum = 1;