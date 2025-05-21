# BulbulaAPI

Detta är ett API som är byggt med Azure Functions och hanterar gästbokningar för ett Airbnb.

---

- **Plattform:** Azure Functions (.NET Isolated)
- **Databas:** Azure Cosmos DB
- **Distribution:** Azure Function App
- **Testverktyg:** Insomnia
- **Säkerhet:** API:t är skyddat med Function Key

---

API:t följer REST-principer och innehåller följande CRUD-operationer:

| Metod   | Endpoint                | Beskrivning                     |
|---------|-------------------------|----------------------------------|
| GET     | `/api/bookings`         | Hämta alla bokningar             |
| POST    | `/api/bookings`         | Skapa en ny bokning              |
| PUT     | `/api/bookings/{id}`    | Uppdatera en bokning med ID      |
| DELETE  | `/api/bookings/{id}`    | Radera en bokning med ID         |

Alla endpoints kräver att du lägger till `?code=dinFunctionKey` i URL:en.

---

## Autentisering

API:t använder **Function key** för att kontrollera åtkomst.

Så här anropar du en endpoint:
GET https://airbnbbookingapi.azurewebsites.net/api/bookings?code=DIN_FUNCTION_KEY

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
