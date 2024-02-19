package net.byleo.wls.routes

import io.ktor.server.application.*
import io.ktor.server.response.*
import io.ktor.server.routing.*
import net.byleo.wls.data.dao
import net.byleo.wls.util.LinesResponse

fun Route.lineRouting() {
    route("/lines") {
        get {
            call.respond(LinesResponse(dao.getLines()))
        }
    }
}