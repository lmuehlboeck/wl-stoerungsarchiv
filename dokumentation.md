# Wiener Linien Störungshistorie - Dokumentation

Verfasser: **Leo Mühlböck**

Datum: **25.10.2022**

## Einführung

Die Störungen der Wiener Linien sind ein praktischer Bestandteil der Wiener Linien Open-Data Schnittstelle. Es werden live Daten zu Ausfällen, Verspätungen, Unfällen usw. zur Verfügung gestellt. Jedoch bleiben die Daten nur so lange bestehen, bis die jeweilige Störung behoben ist. Eine nützliche Funktion wäre, die Daten auch nachher noch einsehen zu können.

## Projektbeschreibung

Die Wiener Linien Störungshistorie ist eine Applikation, welche die Störungen aus der Wiener Linien API ausliest, aufzeichnet und auf einer Website darstellt.

Das Backend besteht aus zwei Teilen: 

- Ein Python-Skript, das in periodischem Zeitabstand die Störungen aus der Wiener Linien API fetched, mit Daten in einer SQLite-Datenbank abgleicht und gegebenenfalls aktualisiert.
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
    - 4 für Weichenstörung
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

### Störungsaufzeichnung

Für die Aufzeichnung der Störungen ist das Python-Skript `backend/update_db.py` zuständig. Es wird auf einem Server in periodischen Zeitabständen ausgeführt (2-5 Minuten - je größer desto mehr leidet die Genauigkeit von `end_time`). Eine Möglichkeit die periodische Ausführung umzusetzen ist weiter unten angeführt.

Die Funktionsweise ist simpel: Es werden die aktuellen Störungen aus der Wiener Linien API geholt und mit der lokalen Datenbank abgeglichen. Dabei gibt es drei Fälle:

1. Eine Störung ist neu (also noch nicht in der Datenbank): Sie wird mit den wichtigsten Daten in `disturbances` gespeichert, die zugehörige Beschreibung in `disturbance_descriptions` und die betroffenen Linien in `disturbances_lines` (wenn es eine noch nicht bekannte Linie ist auch in `lines`)
2. Eine Störung ist aktuell (bereits in der Datenbank): Die Beschreibung der Störung wird mit der aktuellsten in der Datenbank abgeglichen. Wenn diese nicht gleich sind, wird die aktuelle Beschreibung als neuer Eintrag in `disturbance_descriptions` gespeichert. 
3. Eine Störung ist vorbei (in der Datenbank aber nicht mehr in der API): Die `end_time` der betroffenen Störung wird gesetzt.

#### Periodische Ausführung mit cron

Auf Linux-Systemen eine periodische Ausführung mittels cron-Tabs realisiert werden. Meistens ist cron schon installiert - doch zur Sicherheit, hier der Installationsbefehl:

```bash
sudo apt install cron
```

Um einen cron-Tab einzurichten, wird zunächst folgender Befehl ausgeführt:

```bash
crontab -e
```

Es öffnet sich eine Datei im Texteditor. Am Ende dieser Datei wird folgende Zeile angefügt:

```
*/2 * * * * cd /pfad/zum/backend && /pfad/zu/python3 /pfad/zu/update_db.py
```

Es sind natürlich alle Pfade entsprechend anzupassen. Die erste Zahl (hier 2) gibt den Zeitabstand zwischen den Ausführungen in Minuten an.

Datei speichern & schließen.

### API

Damit das Frontend auf die Daten in der Datenbank zugreifen kann, ist die REST-API `api.py` notwendig. Sie wurde mittels dem Python-Package Flask umgesetzt. Die Dokumentation der API-Endpunkte ist in der README zu finden.

## Quellen

*Wiener Linien realtime | Schnittstellendokumentation*. Available at: https://www.wienerlinien.at/ogd_realtime/doku/ogd/wienerlinien-echtzeitdaten-dokumentation.pdf (Accessed: October 24, 2022). 

*SQLite cheat sheet* (2020) *SQLite Tutorial*. Available at: https://www.sqlitetutorial.net/sqlite-cheat-sheet/ (Accessed: October 27, 2022). 

*Using SQLite 3 with Flask - Flask Documentation (2.2)*. Available at: https://flask.palletsprojects.com/en/2.2.x/patterns/sqlite3/ (Accessed: October 27, 2022). 

https://stackoverflow.com/
