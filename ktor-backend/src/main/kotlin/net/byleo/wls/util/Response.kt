package net.byleo.wls.util

import kotlinx.serialization.Serializable
import net.byleo.wls.models.Disturbance
import net.byleo.wls.models.Line

@Serializable
data class MessageResponse(
    val message: String
)

@Serializable
data class DisturbancesResponse(
    val disturbances: List<Disturbance>
)

@Serializable
data class DisturbanceResponse(
    val disturbance: Disturbance
)

@Serializable
data class LinesResponse(
    val lines: List<Line>
)