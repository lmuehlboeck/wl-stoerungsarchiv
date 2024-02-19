package net.byleo.wls.util

import java.time.LocalDateTime

data class DisturbanceFilter(
    val lines: List<String> = emptyList(),
    val types: List<Int> = emptyList(),
    val active: Boolean = false,
    val from: LocalDateTime = LocalDateTime.now(),
    val to: LocalDateTime = LocalDateTime.now(),
    val order: OrderType = OrderType.START,
    val desc: Boolean = true
)

enum class OrderType {
    START, END, TYPE
}