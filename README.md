# BulbulaAPI

Detta är ett API som är byggt med Azure Functions och Cosmos DB. DEt hanterar gästbokningar för ett Airbnb eftersom kalkylblad är så 2010.

## Funktioner:

-  Låter dig **POSTA**, **HÄMTA**, **UPPDATERA** och **RADERA** bokningar.
- Sparar allt i Cosmos DB, för att vi är moderna och molnburna.
- Skyddas av Function Keys. Inga nycklar? Inget API för dig.
- Följer REST-principer. Det är 2025, inte stenåldern.
- Testat med Insomnia.

## Teknik:

- **Plattform:** Azure Functions (.NET Isolated)
- **Databas:** Azure Cosmos DB
- **Distribution:** Azure Function App
- **Testverktyg:** Insomnia
- **Säkerhet:** API:t är skyddat med Function Key

---

##  Köra lokalt:

1. Kloning (av repot, inget annat):

``bash
git clone https://github.com/TantBella/BulbulaAPI.git``

Detta behöver du innan du börjar leka hotellchef:

    - Azure Functions Core Tools  
    - Visual Studio 2022 eller VS Code  
    - Insomnia (eller valfritt verktyg där man kan stirra på JSON)  
    - En Cosmos DB-sträng och nej, du får inte min.

2. Lägg till local.settings.json:

``{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "CosmosConnectionString": "din-hemliga-cosmos-sträng"
  }
}``

3. Kör projektet via Visual Studio eller func start
4. Skicka anrop med tex Insomnia, Postman eller curl.
 
---

## API-endpoints

API:t följer REST-principer och innehåller följande CRUD-operationer:

| Metod   | Endpoint                | Beskrivning                     |
|---------|-------------------------|----------------------------------|
| GET     | `/api/bookings`         | Hämta alla bokningar             |
| POST    | `/api/bookings`         | Skapa en ny bokning              |
| PUT     | `/api/bookings/{id}`    | Uppdatera en bokning med ID      |
| DELETE  | `/api/bookings/{id}`    | Radera en bokning med ID         |

Alla endpoints kräver att du lägger till `?code=dinFunctionKey` i URL:en.

### Exempel på POST-endpoint: 
POST /api/bookings?code=din-key  
Skapar en ny bokning. För gäster som inte vill boka via "ett visst känt företag".

Exempel på JSON-body:

{
  "guestName": "Tant Bella",
  "checkInDate": "2025-11-26",
  "checkOutDate": "2025-12-11",
  "guests": 4,
}

---

## Autentisering

API:t använder **Function key** för att kontrollera åtkomst.

Exempel på hur du anropar en endpoint:

GET: https://airbnbbookingapi.azurewebsites.net/api/bookings?code=DIN_FUNCTION_KEY

Du hittar Function Key i Azure Portal under:  
Function App > Functions > App keys 

---

## Testning

Testning har skett med insomnia  
Funktionerna returnerar följande HTTP-statuskoder:

        200 OK – Lyckad hämtning
        201 Created – Lyckad skapning
        204 No Content – Lyckad radering
        400 Bad Request – Felaktig inmatning
        404 Not Found – Bokning hittades ej
        401 Unauthorized – Saknar function key
