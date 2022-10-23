CREATE TABLE IF NOT EXISTS disturbances (
    id TEXT PRIMARY KEY NOT NULL,
    title TEXT NOT NULL,
    type INTEGER NOT NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME
);

CREATE TABLE IF NOT EXISTS disturbance_descriptions (
    disturbance_id TEXT NOT NULL,
    description TEXT NOT NULL,
    time DATETIME NOT NULL,
    FOREIGN KEY(disturbance_id) REFERENCES disturbances(id)
);

CREATE TABLE IF NOT EXISTS lines (
    id TEXT PRIMARY KEY NOT NULL,
    type INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS disturbances_lines (
    disturbance_id TEXT NOT NULL,
    line_id TEXT NOT NULL,
    FOREIGN KEY(disturbance_id) REFERENCES disturbances(id),
    FOREIGN KEY(line_id) REFERENCES lines(id),
    PRIMARY KEY (disturbance_id, line_id)
);