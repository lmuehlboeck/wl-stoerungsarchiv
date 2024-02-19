package net.byleo.wls.models

import kotlinx.serialization.Serializable
import net.byleo.wls.util.DateSerializer
import org.jetbrains.exposed.sql.Table
import org.jetbrains.exposed.sql.javatime.datetime
import java.time.LocalDateTime

@Serializable
data class Description(
    val description: String,
    @Serializable(with = DateSerializer::class)
    val time: LocalDateTime = LocalDateTime.now()
)

object DisturbanceDescriptions: Table() {
    val description = text("description")
    val time = datetime("time")
    val disturbanceId = reference("disturbance_id", Disturbances)
}