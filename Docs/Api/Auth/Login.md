# Login
```
POST {{host}}/auth/login
```

#### Login Request
```json
{
  "username": "test",
  "password": "asdasd....12edqed"
}
```

```js
200 Ok
```

#### Login Response

```json
{
  "Id": "12345678-abcd-1234-56ab-abcdefgh",
  "username": "test",
  "FistName": "Test",
  "LastName": "User",
  "Email": "e@mail.com",
  "Token": "asdasd....12edqed",
}
```