package net.byleo.wls.data

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import io.ktor.server.config.*
import kotlinx.coroutines.Dispatchers
import net.byleo.wls.models.DisturbanceDescriptions
import net.byleo.wls.models.DisturbancesLines
import net.byleo.wls.models.Disturbances
import net.byleo.wls.models.Lines
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.experimental.newSuspendedTransaction
import org.jetbrains.exposed.sql.transactions.transaction

object Database {
    fun init(
        driverClassName: String = "org.mariadb.jdbc.Driver",
        jdbcURL: String = "jdbc:mariadb://localhost:3306/wls?user=root&password=schueler"
    ) {
        val database = Database.connect(createHikariDataSource(url = jdbcURL, driver = driverClassName))

        transaction(database) {
            SchemaUtils.create(Disturbances, DisturbanceDescriptions, Lines, DisturbancesLines)
        }
    }

    private fun createHikariDataSource(
        url: String,
        driver: String
    ) = HikariDataSource(HikariConfig().apply {
        driverClassName = driver
        jdbcUrl = url
        maximumPoolSize = 10
        isAutoCommit = true
        transactionIsolation = "TRANSACTION_REPEATABLE_READ"
        validate()
    })

    suspend fun <T> dbQuery(block: suspend () -> T): T = newSuspendedTransaction(Dispatchers.IO) {
        block()
    }
}