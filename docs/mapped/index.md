<div class="alert alert-warning text-center" role="alert">
    🚧 This section is still a work in progress 🚧
</div>

# Mapping to objects

An alpha version of the `KiwiQuery.Mapped` library will soon be available (in something like a few months). It will
provide ORM-like features on top of this library. The goal is to replace commands like this one :

```csharp
db.InsertInto("USER")
  .Value("USERNAME", user.UserName)
  .Value("EMAIL", user.Email)
  .Value("PASSWORD_HASH", user.PasswordHash)
  .Value("PASSWORD_SALT", user.PasswordSalt)
  .Value("FIRSTNAME", user.FirstName)
  .Value("LASTNAME", user.LastName)
  .Apply()
```

by something like :

```csharp
db.InsertInto<User>().Values(user).Apply();
```
