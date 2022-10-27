from flask import Flask, g, request
from db_management import get_conn, execute_query
import datetime


app = Flask(__name__)

def get_db():
    db = getattr(g, '_database', None)
    if not db:
        db = g._database = get_conn()
    return db

def to_date(date_str):
    if not date_str:
        return None
    try:
        return datetime.datetime.strptime(date_str, "%Y-%m-%d").replace(hour=23, minute=59, second=59)
    except ValueError:
        raise ValueError(f"{date_str} is not a valid date in the format YYYY-MM-DD")

@app.route("/disturbances", methods=["GET"])
def api_get_disturbances():
    data = []

    # args
    lines = request.args.get("line", type=str)
    types = request.args.get("type", type=str)
    try:
        start_date = to_date(request.args.get("start"))
        end_date = to_date(request.args.get("end"))
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
    if start_date:
        sql += " AND start_time >= ?"
        args.append(start_date)
    if end_date:
        sql += " AND start_time <= ?"
        args.append(end_date)

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
            # list of descriptions
            desc_db = execute_query(get_db(), "SELECT description FROM disturbance_descriptions WHERE disturbance_id=?", (disturbance[0],))
            desc_list = [d[0] for d in desc_db]

            # dictionary of lines
            lines_db = execute_query(get_db(), "SELECT line_id FROM disturbances_lines WHERE disturbance_id=?", (disturbance[0],))
            lines_dict = {}
            for l in lines_db:
                lines_dict[l[0]] = execute_query(get_db(), "SELECT type FROM lines WHERE id=?", (l[0],))[0][0]
            
            
            data.append({
                "title": disturbance[1],
                "descriptions": desc_list,
                "lines": lines_dict,
                "start_time": disturbance[3],
                "end_time": disturbance[4],
            })
    return {
        "data": data
    }

@app.route("/lines", methods=["GET"])
def api_get_lines():
    data = {}
    lines_db = execute_query(get_db(), "SELECT * FROM lines ORDER BY type")
    for l in lines_db:
        data[l[0]] = l[1]
    return {
        "data": data
    }

if __name__ == "__main__":
    app.run(debug=True)