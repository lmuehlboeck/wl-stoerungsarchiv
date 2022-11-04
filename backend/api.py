from flask import Flask, g, request
from db_management import get_conn, execute_query
import datetime


app = Flask(__name__)

def get_db():
    db = getattr(g, '_database', None)
    if not db:
        db = g._database = get_conn()
    return db

def convert_date(date_str):
    if not date_str:
        return None
    try:
        return datetime.datetime.strptime(date_str, "%Y-%m-%d")
    except ValueError:
        raise ValueError(f"{date_str} is not a valid date in the format YYYY-MM-DD")

def generate_disturbance_object(disturbance):
    # list of descriptions
    desc_db = execute_query(get_db(), "SELECT description, time FROM disturbance_descriptions WHERE disturbance_id=?", (disturbance[0],))
    desc_list = [{"description":d[0], "time":d[1]} for d in desc_db]

    # list of lines
    lines_db = execute_query(get_db(), "SELECT line_id FROM disturbances_lines WHERE disturbance_id=?", (disturbance[0],))
    lines_list = [{"id":l[0], "type":execute_query(get_db(), "SELECT type FROM lines WHERE id=?", (l[0],))[0][0]} for l in lines_db]
    
    return {
        "id": disturbance[0],
        "title": disturbance[1],
        "descriptions": desc_list,
        "lines": lines_list,
        "start_time": disturbance[3],
        "end_time": disturbance[4],
    }

@app.route("/api/disturbances/<id>", methods=["GET"])
def api_get_disturbance_by_id(id):
    dist = execute_query(get_db(), "SELECT * FROM disturbances WHERE id=?", (id,))
    if dist:
        return {
            "data": generate_disturbance_object(dist[0])
        }
    else:
        return {"error": {"code": 404, "message": f"No disturbance with id {id} found"}}

@app.route("/api/disturbances", methods=["GET"])
def api_get_disturbances():
    data = []

    # args
    lines = request.args.get("line", type=str)
    types = request.args.get("type", type=str)
    active = request.args.get("active", type=str)
    try:
        from_date = convert_date(request.args.get("from"))
        to_date = convert_date(request.args.get("to"))
    except ValueError as exc:
        return {"error": {"code": 400, "message": str(exc)}}
    order = request.args.get("order", "start", type=str)
    desc = request.args.get("desc", type=str)

    # filters
    sql = "SELECT * FROM disturbances WHERE 1=1"
    args = []
    if lines:
        lines = lines.split(",")
        sql += f" AND id IN (SELECT disturbance_id FROM disturbances_lines WHERE line_id IN ({','.join(['?'] * len(lines))}))"
        args += lines
    if types:
        types = types.split(",")
        sql += f" AND type IN ({','.join(['?'] * len(types))})"
        args += types
    if active and active == "true":
        sql += " AND end_time IS NULL"
    elif active and active != "false":
        return {"error": {"code": 400, "message": f"{active} is not a valid boolean value"}}
    if from_date:
        sql += " AND start_time >= ?"
        args.append(from_date)
    if to_date:
        sql += " AND start_time <= ?"
        args.append(to_date.replace(hour=23, minute=59, second=59))

    # ordering
    if not order or order == "start":
        sql += " ORDER BY start_time"
    elif order == "end":
        sql += " ORDER BY end_time"
    elif order == "type":
        sql += " ORDER BY type"
    else:
        return {"error": {"code": 400, "message": f"Ordering by {order} is not valid"}}
    if not desc or desc == "true":
        sql += " DESC"
    elif desc != "false":
        return {"error": {"code": 400, "message": f"{desc} is not a valid boolean value"}}

    # generate disturbance dictionary
    disturbances = execute_query(get_db(), sql, tuple(args))
    if disturbances:
        for disturbance in disturbances:
            data.append(generate_disturbance_object(disturbance))
    return {
        "data": data
    }

@app.route("/api/lines", methods=["GET"])
def api_get_lines():
    data = {}
    # correct sorting - 1. type; 2. line number smallest to biggest - exception in metro rows because of incorrect casting
    lines_db = execute_query(get_db(), "SELECT * FROM lines ORDER BY type, CASE WHEN id LIKE 'U%' THEN id ELSE cast(id as INTEGER) END")
    data = [{"id":l[0], "type":l[1]} for l in lines_db]
    return {
        "data": data
    }

if __name__ == "__main__":
    app.run(debug=True)