from email.policy import default
from flask import Flask, g, request
from db_management import get_conn, execute_query


app = Flask(__name__)

def get_db():
    db = getattr(g, '_database', None)
    if not db:
        db = g._database = get_conn()
    return db

@app.route("/disturbances", methods=["GET"])
def api_get_disturbances():
    res = []

    # filters
    lines = request.args.get("line", type=str)
    types = request.args.get("type", type=str)
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
            
            
            d = {
                "title": disturbance[1],
                "descriptions": desc_list,
                "lines": lines_dict,
                "start_time": disturbance[3],
                "end_time": disturbance[4],
            }
            res.append(d)
    return res

@app.route("/lines", methods=["GET"])
def api_get_lines():
    res = {}
    lines_db = execute_query(get_db(), "SELECT * FROM lines ORDER BY type")
    for l in lines_db:
        res[l[0]] = l[1]
    return res

if __name__ == "__main__":
    app.run(debug=True)