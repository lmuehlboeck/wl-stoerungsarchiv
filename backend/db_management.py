import sqlite3


def get_conn():
    conn = sqlite3.connect("data.db")
    # create database and tables if they do not exist
    with open("schema.sql") as f:
        conn.executescript(f.read())
    return conn


def execute_query(conn, query, args=()):
    try:
        cur = conn.execute(query, args)
        res = cur.fetchall()
        conn.commit()
        cur.close()
        return res if res else None
    except:
        print("SQL query could not be executed")
        return None