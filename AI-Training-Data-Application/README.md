# AI Training Data Application

## Einführung

Dies ist eine Webanwendung für die Verwaltung von Trainingsdaten für KI-Modelle. Die Anwendung ermöglicht das Hinzufügen, Auflisten und Kategorisieren von Textdaten.

## Datenbank

Die Datenbank wird als SQLite-Datenbank in der Datei `TextDb.sqlite` gespeichert. Der Dateiname kann in der `appsettings.json` geändert werden.

### Datenbank erstellen

Führen Sie die folgenden Befehle aus, um die Datenbank zu erstellen und die Migrationen anzuwenden:

1. Datenbankmigrationen erstellen (falls noch nicht vorhanden):
    ```sh
    dotnet ef migrations add InitialCreate
    ```

2. Datenbank aktualisieren:
    ```sh
    dotnet ef database update
    ```

## Ports

Die Anwendung verwendet die folgenden Ports:

- HTTP: `5099`
- HTTPS: `7100`

Stellen Sie sicher, dass diese Ports in Ihrer Entwicklungsumgebung verfügbar sind und nicht von anderen Anwendungen verwendet werden.

Je nachdem, ob Sie HTTP oder HTTPS verwenden, müssen Sie die `BackendAddress` in der `appsettings.json` Datei entsprechend aktualisieren.

## Konfiguration der `appsettings.json`

Die `appsettings.json` Datei muss entsprechend konfiguriert sein. Hier ist ein Beispiel für eine typische Konfiguration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=TextDb.sqlite"
  },
  "AllowedHosts": "*",
  "BackendAddress": "https://localhost:7100/"
}
```