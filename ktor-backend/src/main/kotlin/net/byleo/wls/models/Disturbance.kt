package net.byleo.wls.models

import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable
import net.byleo.wls.util.DateSerializer
import org.jetbrains.exposed.dao.id.IdTable
import org.jetbrains.exposed.sql.Table
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

@Serializable
data class Disturbance(
    val id: String,
    val title: String,
    val type: Int,
    val lines: List<Line>,
    val descriptions: List<Description>,
    @Serializable(with = DateSerializer::class)
    @SerialName("start_time") val startTime: LocalDateTime,
    @Serializable(with = DateSerializer::class)
    @SerialName("end_time") val endTime: LocalDateTime? = null
)

object Disturbances: IdTable<String>() {
    val disturbanceId = varchar("id", 255)
    val title = varchar("title", 1023)
    val type = integer("type")
    val startTime = datetime("start_time")
    val endTime = datetime("end_time").nullable()

    override val id = disturbanceId.entityId()
    override val primaryKey = PrimaryKey(id)
}