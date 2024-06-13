package net.byleo.wls

import io.ktor.http.*
import io.ktor.server.application.*
import io.ktor.server.netty.*
import io.ktor.server.plugins.cors.routing.*
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.buffer
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.flow.launchIn
import kotlinx.coroutines.flow.onEach
import net.byleo.wls.data.Database
import net.byleo.wls.plugins.*
import net.byleo.wls.util.Scheduler
import kotlin.time.Duration
import kotlin.time.Duration.Companion.seconds

fun main(args: Array<String>) {
    EngineMain.main(args)
}

fun Application.module() {
    install(CORS) {
        allowHeader(HttpHeaders.ContentType)
        anyHost()
    }

    val driverClassName = environment.config.property("ktor.storage.driverClassName").getString()
    val jdbcURL = environment.config.property("ktor.storage.jdbcURL").getString()
    Database.init(driverClassName, jdbcURL)

    val updateInterval = environment.config.property("ktor.application.updateIntervalSec")
        .getString().toIntOrNull() ?: 60
    val scheduler = Scheduler {
        try {
            updateDb()
            println("Updated database successfully!")
        } catch (e: Exception) {
            println("Something went wrong while updating the database:")
            e.printStackTrace()
        }
    }
    scheduler.scheduleExecution(updateInterval.toLong(), java.util.concurrent.TimeUnit.SECONDS)

    configureSerialization()
    configureRouting()
}
