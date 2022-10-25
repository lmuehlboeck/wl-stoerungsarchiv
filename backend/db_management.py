import sqlite3

conn = sqlite3.connect("data.db")

# create database and tables if they do not exist
with open("schema.sql") as f:
    conn.executescript(f.read())

def execute_query(query, args=(), one_res=False):
    cur = conn.execute(query, args)
    res = cur.fetchall()
    conn.commit()
    cur.close()
    return (res[0] if one_res else res) if res else None