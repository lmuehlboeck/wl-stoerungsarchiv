package net.byleo.wls.models

import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable
import org.jetbrains.exposed.dao.id.IdTable
import org.jetbrains.exposed.sql.Table

@Serializable
data class Line(
    val id: String,
    val type: Int,
    @SerialName("display_name") val displayName: String = id
)

object Lines: IdTable<String>() {
    val lineId = varchar("id", 255)
    val type = integer("type")
    val displayName = varchar("display_name", 255)

    override val id = lineId.entityId()
    override val primaryKey = PrimaryKey(id)
}

object DisturbancesLines: Table() {
    val disturbanceId = reference("disturbance_id", Disturbances)
    val lineId = reference("line_id", Lines)
}