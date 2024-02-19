package net.byleo.wls.routes

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import net.byleo.wls.data.dao
import net.byleo.wls.util.DisturbanceResponse
import net.byleo.wls.util.DisturbancesResponse
import net.byleo.wls.util.MessageResponse
import net.byleo.wls.util.DisturbanceFilter
import net.byleo.wls.util.OrderType
import java.time.LocalDate
import java.time.LocalTime
import java.time.format.DateTimeFormatter
import java.time.format.DateTimeParseException

fun Route.disturbanceRouting() {
    route("/disturbances") {
        get {
            val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")

            val lines = call.request.queryParameters["lines"]?.split(",") ?: emptyList()
            val types = call.request.queryParameters["types"]?.split(",")?.map {
                it.toIntOrNull() ?: return@get call.respond(
                    HttpStatusCode.BadRequest,
                    MessageResponse("Invalid type code $it")
                )
            } ?: emptyList()
            val active = call.request.queryParameters["active"]?.toBooleanStrictOrNull() ?: false
            val from = call.request.queryParameters["from"]?.let {
                try { LocalDate.parse(it, formatter).atTime(LocalTime.MIN) } catch (e: DateTimeParseException) {
                    return@get call.respond(
                        HttpStatusCode.BadRequest,
                        MessageResponse("Invalid from date $it")
                    )
                }
            } ?: LocalDate.now().atTime(LocalTime.MIN)
            val to = call.request.queryParameters["to"]?.let {
                try { LocalDate.parse(it, formatter).atTime(LocalTime.MAX) } catch (e: DateTimeParseException) {
                    return@get call.respond(
                        HttpStatusCode.BadRequest,
                        MessageResponse("Invalid to date $it")
                    )
                }
            } ?: LocalDate.now().atTime(LocalTime.MAX)
            val order = call.request.queryParameters["order"]?.let { type ->
                OrderType.entries.firstOrNull { it.name.equals(type, ignoreCase = true) } ?: return@get call.respond(
                    HttpStatusCode.BadRequest,
                    MessageResponse("Invalid order type $type")
                )
            } ?: OrderType.START
            val desc = call.request.queryParameters["desc"]?.toBooleanStrictOrNull() ?: true

            call.respond(
                DisturbancesResponse(
                    dao.getDisturbances(DisturbanceFilter(
                        lines = lines,
                        types = types,
                        active = active,
                        from = from,
                        to = to,
                        order = order,
                        desc = desc
                    ))
                )
            )
        }
        get("{id}") {
            val id = call.parameters["id"]!!
            val disturbance = dao.getDisturbanceById(id) ?: return@get call.respond(
                HttpStatusCode.NotFound,
                MessageResponse("No disturbance with id $id found"),
            )
            call.respond(DisturbanceResponse(disturbance))
        }
    }
}