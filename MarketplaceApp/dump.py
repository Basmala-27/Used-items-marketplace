import sqlite3

def escape_val(val):
    if val is None:
        return "NULL"
    elif isinstance(val, (int, float)):
        return str(val)
    elif isinstance(val, str):
        # Handle SQLite boolean strings if any (EF Core might store 'True'/'False' or 1/0)
        # But usually EF Core SQLite provider stores boolean as INTEGER 1 or 0
        escaped = val.replace("'", "''")
        return f"N'{escaped}'"
    elif isinstance(val, bytes):
        return "0x" + val.hex()
    else:
        escaped = str(val).replace("'", "''")
        return f"N'{escaped}'"

conn = sqlite3.connect('Marketplace.db')
cursor = conn.cursor()
cursor.execute("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' AND name NOT LIKE '__EFMigrationsHistory'")
tables = [r[0] for r in cursor.fetchall()]

with open('dump.sql', 'w', encoding='utf-8') as f:
    f.write("EXEC sp_MSforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\";\nGO\n")
    
    for table in tables:
        cursor.execute(f"PRAGMA table_info({table})")
        columns = [row[1] for row in cursor.fetchall()]
        
        cursor.execute(f"SELECT * FROM {table}")
        rows = cursor.fetchall()
        if not rows:
            continue
            
        f.write(f"PRINT 'Importing {table}...';\n")
        f.write("BEGIN TRY\n")
        f.write(f"    SET IDENTITY_INSERT [{table}] ON;\n")
        f.write("END TRY\nBEGIN CATCH\nEND CATCH\n")
        
        # Batch inserts to avoid massive files or slow execution
        batch_size = 100
        for i in range(0, len(rows), batch_size):
            batch = rows[i:i+batch_size]
            for row in batch:
                cols_str = ", ".join([f"[{c}]" for c in columns])
                vals_str = ", ".join([escape_val(v) for v in row])
                f.write(f"INSERT INTO [{table}] ({cols_str}) VALUES ({vals_str});\n")
            f.write("GO\n")
            
        f.write("BEGIN TRY\n")
        f.write(f"    SET IDENTITY_INSERT [{table}] OFF;\n")
        f.write("END TRY\nBEGIN CATCH\nEND CATCH\nGO\n")

    f.write("EXEC sp_MSforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\";\nGO\n")

print("Dump created.")
