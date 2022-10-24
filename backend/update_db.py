"""
This script
    - creates the database where all disturbances are stored, if it does not exist
    - creates the tables, if they do not exist
    - saves all new disturbances to the database
    - saves all new discriptions of a disturbance to the database
    - sets the end time of closed disturbances
"""

import sqlite3, requests
from datetime import datetime


API_URL = "https://www.wienerlinien.at/ogd_realtime/trafficInfoList?name=stoerunglang"

conn = sqlite3.connect("data.db")

def execute_query(query, args=(), one_res=False):
    cur = conn.execute(query, args)
    res = cur.fetchall()
    conn.commit()
    cur.close()
    return (res[0] if one_res else res) if res else None


def get_disturbance_type(title):
    key = title.split(" ")[-1]
    return {
        "Verspätungen": 0,
        "Verkehrsunfall": 1,
        "Fahrzeug": 2,
        "Gleisschaden": 3,
        "Weichenstörung": 4,
        "Fahrleitungsgebrechen": 5,
        "Signalstörung": 6,
        "Rettungseinsatz": 7,
        "Polizeieinsatz": 8,
        "Feuerwehreinsatz": 9,
        "Falschparker": 10,
        "Demonstration": 11,
        "Veranstaltung": 12
    }.get(key, 13)


def get_line_type(type):
    return {
        "ptBusCity": 0,
        "ptTram": 1,
        "ptMetro": 2
    }.get(type, 3)


def main():
    # create database and tables if they do not exist
    with open("schema.sql") as f:
        conn.executescript(f.read())
    
    res = requests.get(API_URL)
    if not res.ok:
        print("Connection to API failed")
        return
    
    disturbances = res.json()["data"]["trafficInfos"]
    for disturbance in disturbances:
        if execute_query("SELECT * FROM disturbances WHERE end_time IS NULL AND id=?", (disturbance["name"],)):
            # disturbance is already saved - check for description updates
            desc_old = execute_query("SELECT description FROM disturbance_descriptions WHERE disturbance_id=? ORDER BY time DESC", (disturbance["name"],), True)[0]
            if desc_old != disturbance["description"]:
                execute_query("INSERT INTO disturbance_descriptions(disturbance_id, description, time) VALUES(?, ?, ?)",
                    (disturbance["name"], disturbance["description"], datetime.now()))
        else:
            # disturbance is new
            execute_query("INSERT INTO disturbances(id, title, type, start_time) VALUES(?, ?, ?, ?)",
                (disturbance["name"], disturbance["title"], get_disturbance_type(disturbance["title"]), datetime.fromisoformat(disturbance["time"]["start"][0:-5])))
            execute_query("INSERT INTO disturbance_descriptions(disturbance_id, description, time) VALUES(?, ?, ?)",
                (disturbance["name"], disturbance["description"], datetime.now()))
            for line, type in disturbance["attributes"]["relatedLineTypes"].items():
                execute_query("INSERT OR IGNORE INTO lines(id, type) VALUES (?, ?)",
                    (line, get_line_type(type)))
                execute_query("INSERT INTO disturbances_lines(disturbance_id, line_id) VALUES (?, ?)", 
                    (disturbance["name"], line))
    # close all deleted disturbances
    open_disturbances = execute_query("SELECT id FROM disturbances WHERE end_time IS NULL")
    for open_disturbance in open_disturbances:
        if not any(d["name"] == open_disturbance[0] for d in disturbances):
            execute_query("UPDATE disturbances SET end_time=? WHERE id=?", (datetime.now(), open_disturbance[0]))
        

if __name__ == "__main__":
    main()