package net.byleo.wls

import io.ktor.server.util.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.withContext
import net.byleo.wls.data.Database
import net.byleo.wls.data.dao
import net.byleo.wls.models.Description
import net.byleo.wls.models.Disturbance
import net.byleo.wls.models.Line
import java.sql.Connection
import java.sql.DriverManager

/*
This script migrates the old SQLite-Database of wls to the new MariaDB database
 */

const val DB_URL = "jdbc:sqlite:data.db"

fun connect(): Connection {
    return DriverManager.getConnection(DB_URL)
}

suspend fun getLines(conn: Connection, id: String): List<Line> = withContext(Dispatchers.IO) {
    return@withContext conn.prepareStatement(
        "SELECT * FROM lines l INNER JOIN disturbances_lines d ON l.id = d.line_id WHERE d.disturbance_id=?"
    ).use LineBuilder@{ lineStmt ->
        lineStmt.setString(1, id)
        lineStmt.executeQuery().use { lineRes ->
            val lines = mutableListOf<Line>()
            while (lineRes.next()) {
                lines.add(
                    Line(
                        id = lineRes.getString("id"),
                        type = lineRes.getInt("type")
                    )
                )
            }
            return@LineBuilder lines
        }
    }
}

suspend fun getDescriptions(conn: Connection, id: String) = withContext(Dispatchers.IO) {
    return@withContext conn.prepareStatement(
        "SELECT * FROM disturbance_descriptions WHERE disturbance_id=?"
    ).use DescBuilder@{ descStmt ->
        descStmt.setString(1, id)
        descStmt.executeQuery().use { descRes ->
            val descriptions = mutableListOf<Description>()
            while (descRes.next()) {
                descriptions.add(
                    Description(
                        description = descRes.getString("description"),
                        time = descRes.getTimestamp("time").toLocalDateTime()
                    )
                )
            }
            return@DescBuilder descriptions
        }
    }
}

fun main() {
    var i = 0
    Database.init()
    connect().use { conn ->
        conn.createStatement().use { stmt ->
            stmt.executeQuery("SELECT * FROM disturbances").use { res ->
                runBlocking {
                    while (res.next()) {
                        val id = res.getString("id")
                        val title = res.getString("title")
                        val type = res.getInt("type")
                        val startTime = res.getTimestamp("start_time").toLocalDateTime()
                        val endTime = res.getTimestamp("end_time")?.toLocalDateTime()
                        launch {
                            val lines = getLines(conn, id)
                            val descriptions = getDescriptions(conn, id)
                            val disturbance = Disturbance(
                                id = id,
                                title = title,
                                type = type,
                                lines = lines,
                                descriptions = descriptions,
                                startTime = startTime,
                                endTime = endTime
                            )
                            dao.insertDisturbance(disturbance)
                        }
                    }
                }
            }
        }
    }
}