
select * from INFORMATION_SCHEMA.PROCESSLIST 
WHERE `User` = 'mlv' 
AND `Host` like '80.29.25.30%';
