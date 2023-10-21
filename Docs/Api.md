# Coms API
- [Coms API](#coms-api)
 - [Auth](#auth)
   - [Login](#login)
    - [Login Request](#login-request)
    - [Login Response](#login-response)

## Auth

###Login

```js
POST {{host}}/auth/login
```

#### Login Request

```json
{
	"username": "string",
	"password": "string"
}
```

#### Authentication Response

```js
200 OK
```