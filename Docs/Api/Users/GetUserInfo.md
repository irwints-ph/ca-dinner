# User Info
```
POST {{host}}/users/info
Authorization: Bearer {{token}}
```

#### User Info Request

```js
200 Ok
```

#### User Info Response

```json
{
{
  "Msg": null,
  "Success": true,
  "Line": {
    "Id": 0000-00-00-00-0000,
    "Fullname": "Super Root",
    "ModuleName": "User Maintenance"
  },
  "Data": [
      {
        "ModuleId": 1,
        "ModuleName": "Home",
        "ParentId": {},
        "IconClass": "fa-home",
        "MenuLevel": 1,
        "PageName": "home",
        "FolderPath": "/",
        "SortOrder": 1,
        "IsParent": "N"
      },
      {
        "ModuleId": 10,
        "ModuleName": "Tools",
        "ParentId": {},
        "IconClass": "fa-tools",
        "MenuLevel": 1,
        "PageName": "",
        "FolderPath": "",
        "SortOrder": 100,
        "IsParent": "Y"
      }
    ],
}
}
```