# Login
```
POST {{host}}/auth/login
```

#### Login Request
```json
{
  "username" : "jdl",
  "password": "asdasd....12edqed",
  "firstName": "Juan",
  "lastName": "Dela Cruz",
  "email": "jdl@rock.com"
}
```

```js
200 Ok
```

#### Login Response

```json
{
  "Msg": "",
  "Success": true,
  "Line": {
    "Id": "12345678-abcd-1234-56ab-abcdefgh",
    "username": "test",
    "FistName": "Test",
    "LastName": "User",
    "Email": "e@mail.com",
    "Token": "asdasd....12edqed",
  }
}
```