# Wiener Linien Störungshistorie - Dokumentation

Verfasser: **Leo Mühlböck**

Datum: **08.10.2022**

## Einführung

Die Störungen der Wiener Linien sind ein praktischer Bestandteil der Wiener Linien Open-Data Schnittstelle. Es werden live Daten zu Ausfällen, Verspätungen, Unfällen usw. zur Verfügung gestellt. Jedoch bleiben die Daten nur so lange bestehen, bis die jeweilige Störung behoben ist. Eine nützliche Funktion wäre, die Daten auch nachher noch einsehen zu können.

## Projektbeschreibung

Die Wiener Linien Störungshistorie ist eine Web-Applikation, welche die Störungen aus der Wiener Linien API ausliest, aufzeichnet und auf einer Website darstellt.

Das Backend besteht aus zwei Teilen: 

- Ein Python-Skript, das in periodischem Zeitabstand die Störungen aus der API fetched, mit Daten in einer SQLite-Datenbank abgleicht und gegebenenfalls aktualisiert.
- Eine Rest-API mit dem Python-Package Flask, das die Daten aus der Datenbank öffentlich zur Verfügung stellt.

Das Frontend ist eine VueJS-Website, welche die Daten aus der Rest-API ausliest und übersichtlich darstellt.

## Das Backend

### Datenbankstruktur

Die SQLite3-Datenbank hat folgenden Aufbau:

![datenbankstruktur](./datenbankstruktur.png)

- Tabelle `disturbances`: Hier sind alle Störungen gespeichert

  - `id` wird aus der API übernommen, um die Störungen bei Aktualisierung mit dem Wiener Linien Server wieder eindeutig identifizieren zu können
  - `title` ist der Titel der Störung
  - `start_time` ist der Zeitpunkt, an dem die Störung begonnen hat
  - `end_time` ist der Zeitpunkt, an dem die Störung komplett behoben ist - wird ausgefüllt, sobald die Störung am Wiener Linien Server gelöscht ist
- Tabelle `descriptions`: Hier sind alle Beschreibungen der Störungen gespeichert. Da sich die Beschreibung einer Störung im Laufe der Zeit ändert, werden manchmal mehrere Beschreibungen zu einer Störung gespeichert (z. B. von kein Betrieb zu Verspätungen)

  - `disturbance_id` ist die `id` der zugehörigen Störung
  - `description` ist die Beschreibung
  - `time` ist der Zeitpunkt, an dem die Beschreibung aktuell gültig war
- Tabelle `lines`: Hier werden betroffene Linien gespeichert

  - `id` ist eine eindeutige Identifikation für die Linie
  - `name` ist der Name (Nummer) der Linie (z.B. 31, 32A, U6, ...)
  - `type` ist der Typ der Linie (`ptBusCity` = Bus, `ptTram` = Straßenbahn, `ptMetro` = U-Bahn)
- Tabelle `disturbances_lines`: Hier werden die Linien zu den Störungen gespeichert (um many-many Beziehung zu ermöglichen)

  - `disturbances_id` ist die `id` der Störung
  - `line_id` ist die `id` der Linie


Die Datenbank wird über das Create-Script `backend/scheme.sql` erstellt.
