# Wiener Linien Störungshistorie

Derzeit in Entwicklung ...

Die Wiener Linien Störungshistorie speichert alle Störungen der Wiener Linien, um sie später auch noch einsehen zu können. Die Daten sind auf der Website einsehbar bzw. stehen auch über eine öffentlich zugängliche REST-API zu Verfügung.

Website: https://wls.byleo.net/

## API

Alle Daten sind über eine REST-API abrufbar.

Base-URL: https://wls.byleo.net/api/

Im Folgenden ist eine kleine Dokumentation zur API zu finden.

### Kodierungen

#### Störungstypen

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

#### Linientypen

- 0 für Bus
- 1 für Straßenbahn
- 2 für U-Bahn

### Störungen

#### Request

Syntax: `https://wls.byleo.net/api/disturbances?line=<lines>&type=<types>&active=<bool>&start=<date>&end=<date>&order=<attr_name>&desc=<bool>`

Beispiel: `https://wls.byleo.net/api/disturbances?line=U6,26&type=1,7,8,9&start=2022-01-01&order=type&desc=false`

= Alle Verkehrsunfälle, Polizei-, Feuerwehr- und Rettungseinsätze auf den Linien U6 und 26 ab dem 1.1.2022, aufsteigend sortiert nach dem Störungstyp.

| Parameter | Datentyp      | Beschreibung                                                 | Default |
| --------- | ------------- | ------------------------------------------------------------ | ------- |
| line      | Liste von str | Filtert die Störungen nach den angegebenen Linien            | alle    |
| type      | Liste von int | Filtert die Störungen nach den angegebenen Störungstypen (Kodierung siehe [oben](#Störungstypen)) | alle    |
| active    | bool          | Wenn true wird nach aktuellen Störungen gefiltert (also Störungen, die noch nicht beendet sind) | false   |
| start     | date          | Filtert alle Störungen, die vor dem angegebenen Datum begonnen haben, heraus | null    |
| end       | date          | Filtert alle Störungen, die nach dem angegebenen Datum begonnen haben, heraus | null    |
| order     | str           | Sortiert die Störungen nach dem angegebenen Attribut. Gültige Werte: start, end, type | start   |
| desc      | bool          | Wenn true werden die Störungen je nach order-Parameter absteigend sortiert, ansonsten aufsteigend | true    |

#### Response

Syntax:

```json
{
  "data": [
    {
      "descriptions": [
        {
          "description": "description",
          "time": "time"
        }
      ],
      "end_time": "end_time",
      "lines": [
        {
          "id": "id",
          "type": type
        }
      ],
      "start_time": "start_time",
      "title": "title"
    }
  ]
}
```

Beispiel:

```json
{
  "data": [
    {
      "descriptions": [
        {
          "description": "Wegen eines Feuerwehreinsatzes im Bereich Oberfeldgasse 51 fährt die Linie 26 nur zwischen Strebersdorf, Edmund-Hawranek-Platz und Josef-Baumann-Gasse. Die Störung dauert voraussichtlich bis 21:15 Uhr!",
          "time": "2022-10-27 20:32:01.779634"
        },
        {
          "description": "Nach einer Fahrtbehinderung kommt es auf der Linie 26 zu unterschiedlichen Intervallen.",
          "time": "2022-10-27 20:58:01.973886"
        }
      ],
      "end_time": "2022-10-27 21:48:01.816828",
      "lines": [
        {
          "id": "26",
          "type": 1
        }
      ],
      "start_time": "2022-10-27 20:27:03",
      "title": "26 : Feuerwehreinsatz"
    }
  ]
}
```

| Attribut     | Datentyp           | Beschreibung                                                 |
| ------------ | ------------------ | ------------------------------------------------------------ |
| data         | Liste von Objekten | Enthält alle Störungen                                       |
| descriptions | Liste von Objekten | Enthält alle Beschreibungen einer Störung                    |
| description  | str                | Beschreibung der Störung zu einem Zeitpunkt                  |
| time         | datetime           | Zeitpunkt an dem die Beschreibung gültig war                 |
| end_time     | datetime           | Zeitpunkt an dem die Störung zu ende war (null wenn noch nicht zu Ende) |
| lines        | Liste von Objekten | Enthält alle von der Störung betroffenen Linien              |
| id           | str                | Bezeichnung der Linie                                        |
| type         | int                | Typ der Linie (Kodierung siehe [oben](#Linientypen))         |
| start_time   | datetime           | Zeitpunkt an dem die Störung begonnen hat                    |
| title        | str                | Titel der Störung                                            |

