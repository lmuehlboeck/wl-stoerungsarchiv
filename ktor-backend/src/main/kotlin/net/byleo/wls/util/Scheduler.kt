package net.byleo.wls.util

import java.util.concurrent.Executors
import java.util.concurrent.TimeUnit

class Scheduler(private val task: Runnable) {
    private val executor = Executors.newScheduledThreadPool(1)

    fun scheduleExecution(delay: Long, unit: TimeUnit) {
        val taskWrapper = Runnable {
            task.run()
        }
        executor.scheduleWithFixedDelay(taskWrapper, 0, delay, unit)
    }


    fun stop() {
        executor.shutdown()
        try {
            executor.awaitTermination(1, TimeUnit.HOURS)
        } catch (_: InterruptedException) {}
    }
}