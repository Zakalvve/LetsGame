SELECT  N'
            CREATE TRIGGER trg_' + t.Name + '_Update ON ' + ObjectName + '
            AFTER UPDATE 
            AS 
            BEGIN
                UPDATE  t
                SET LastUpdate = GETDATE()
                FROM ' + o.ObjectName + ' AS t
                        INNER JOIN inserted AS i
                            ON ' + 
            STUFF((SELECT ' AND t.' + QUOTENAME(c.Name) + ' = i.' + QUOTENAME(c.Name)
                    FROM    sys.index_columns AS ic
                            INNER JOIN sys.columns AS c
                                ON c.object_id = ic.object_id
                                AND c.column_id = ic.column_id
                    WHERE   ic.object_id = t.object_id
                    AND     ic.index_id = ix.index_id
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 4, '') + ';
            END;
            GO'
FROM    sys.tables AS t
        INNER JOIN sys.indexes AS ix
            ON ix.object_id = t.object_id
            AND ix.is_primary_key = 1
        CROSS APPLY (SELECT QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + '.' + QUOTENAME(t.name)) o (ObjectName)
WHERE   EXISTS 
        (   SELECT  1 
            FROM    sys.columns AS c 
            WHERE   c.Name = 'LastUpdate' 
            AND     c.object_id = t.object_id
        );