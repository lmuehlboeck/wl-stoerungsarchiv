package net.byleo.wls

import kotlinx.coroutines.runBlocking
import kotlinx.serialization.SerializationException
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.jsonArray
import kotlinx.serialization.json.jsonObject
import kotlinx.serialization.json.jsonPrimitive
import net.byleo.wls.data.Database
import net.byleo.wls.data.dao
import net.byleo.wls.models.Description
import net.byleo.wls.models.Disturbance
import net.byleo.wls.models.Line
import java.net.URL
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import javax.net.ssl.HttpsURLConnection

/*
This script
    - creates the database where all disturbances are stored, if it does not exist
    - creates the tables, if they do not exist
    - saves all new disturbances to the database
    - saves all new discriptions of a disturbance to the database
    - sets the end time of closed disturbances
 */

const val API_URL = "https://www.wienerlinien.at/ogd_realtime/trafficInfoList?name=stoerunglang"

fun disturbanceTypeFromTitle(title: String): Int {
    val key = title.split(" ").last()
    return mapOf(
        "Verspätungen" to 0,
        "Verkehrsunfall" to 1,
        "Fahrzeug" to 2,
        "Gleisschaden" to 3,
        "Weichenstörung" to 4,
        "Fahrleitungsgebrechen" to 5,
        "Signalstörung" to 6,
        "Rettungseinsatz" to 7,
        "Polizeieinsatz" to 8,
        "Feuerwehreinsatz" to 9,
        "Falschparker" to 10,
        "Demonstration" to 11,
        "Veranstaltung" to 12
    )[key] ?: 13
}

fun lineTypeFromApi(apiType: String): Int {
    return mapOf(
        "ptBusCity" to 0,
        "ptTram" to 1,
        "ptMetro" to 2
    )[apiType] ?: 3
}

fun updateDb(): Unit = runBlocking {
    val url = URL(API_URL)
    val data = with(url.openConnection() as HttpsURLConnection) {
        if(responseCode !in 200..299) {
            println("Database update canceled: API returns response code $responseCode")
            return@runBlocking
        }
        val responseText = inputStream.bufferedReader().readText()
        try {
            return@with Json.parseToJsonElement(responseText).jsonObject
        } catch (e: SerializationException) {
            println("Database update canceled: API returns invalid JSON")
            return@runBlocking
        }
    }

    val disturbances = data["data"]!!.jsonObject["trafficInfos"]!!.jsonArray
    val notClosedDisturbances = mutableListOf<String>()
    for(el in disturbances) {
        val disturbance = el.jsonObject
        val description = disturbance["description"]!!.jsonPrimitive.content
        val id = disturbance["name"]!!.jsonPrimitive.content
        notClosedDisturbances.add(id)
        dao.getDisturbanceById(id)?.also {
            if(it.descriptions.last().description != description) {
                dao.insertDescription(Description(description), id)
            }
        } ?: run {
            val title = disturbance["title"]!!.jsonPrimitive.content
            val lines = disturbance["attributes"]!!.jsonObject["relatedLineTypes"]!!.jsonObject.map {
                Line(
                    id = it.key.trim(),
                    type = lineTypeFromApi(it.value.jsonPrimitive.content)
                )
            }
            val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSSZ")
            val startTime = LocalDateTime.parse(
                disturbance["time"]!!.jsonObject["start"]!!.jsonPrimitive.content,
                formatter
            )
            dao.insertDisturbance(
                Disturbance(
                    id = id,
                    title = title,
                    type = disturbanceTypeFromTitle(title),
                    lines = lines,
                    descriptions = listOf(Description(description, startTime)),
                    startTime = startTime
                )
            )
        }
    }
    dao.closeDisturbancesExcept(notClosedDisturbances)
}