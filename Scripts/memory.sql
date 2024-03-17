show status where variable_name = 'threads_connected';
show full processlist;
flush tables;
SELECT * FROM performance_schema.setup_instruments WHERE NAME LIKE '%memory/innodb%';

SELECT SUBSTRING_INDEX(event_name,'/',2) AS
code_area, FORMAT_BYTES(SUM(current_alloc))
AS current_alloc
FROM sys.x$memory_global_by_current_bytes
GROUP BY SUBSTRING_INDEX(event_name,'/',2)
ORDER BY SUM(current_alloc) DESC;

 SHOW STATUS;
 
 select event_name, current_alloc 
from sys.x$memory_global_by_current_bytes;

SHOW ENGINE INNODB STATUS;

SELECT ( @@read_buffer_size
+ @@read_rnd_buffer_size
+ @@sort_buffer_size
+ @@join_buffer_size
+ @@binlog_cache_size
+ @@thread_stack
+ @@tmp_table_size
+ 2*@@net_buffer_length
) / (1024 * 1024) AS MEMORY_PER_CON_MB;