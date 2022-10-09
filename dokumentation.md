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

