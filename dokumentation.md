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
  - `type` ist der Typ der Störung
    - 0 für Verspätungen
    - 1 für Verkehrsunfall
    - 2 für schadhaftes Fahrzeug
    - 3 für Gleisschaden
    - 4 für Weichenschaden
    - 5 für Fahrleitungsgebrechen
    - 6 für Signalstörung
    - 7 für Rettungseinsatz
    - 8 für Polizeieinsatz
    - 9 für Feuerwehreinsatz
    - 10 für Falschparker
    - 11 für Demonstration
    - 12 für Veranstaltung
    - 13 für Sonstige
  - `start_time` ist der Zeitpunkt, an dem die Störung begonnen hat
  - `end_time` ist der Zeitpunkt, an dem die Störung komplett behoben ist - ist NULL wenn die Störung noch aktuell ist und wird erst ausgefüllt, sobald die Störung am Wiener Linien Server gelöscht ist
- Tabelle `disturbance_descriptions`: Hier sind alle Beschreibungen einer Störungen gespeichert. Da sich die Beschreibung einer Störung im Laufe der Zeit ändert, werden manchmal mehrere Beschreibungen zu einer Störung gespeichert (z. B. von kein Betrieb zu Verspätungen)

  - `disturbance_id` ist die `id` der zugehörigen Störung
  - `description` ist die Beschreibung
  - `time` ist der Zeitpunkt, an dem die Beschreibung aktuell gültig war
- Tabelle `lines`: Hier werden alle (jemals) betroffenen Linien gespeichert

  - `id` ist die eindeutige Identifikation für die Linie (z.B. 31, 32A, U6, ...)
  - `type` ist der Typ der Linie
    - 0 für Bus
    - 1 für Straßenbahn
    - 2 für U-Bahn
- Tabelle `disturbances_lines`: Hier werden die Linien zu den Störungen gespeichert (um many-many Beziehung zu ermöglichen)

  - `disturbances_id` ist die `id` der Störung
  - `line_id` ist die `id` der Linie


Die Datenbank wird über das Create-Script `backend/scheme.sql` erstellt.
