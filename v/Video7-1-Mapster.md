# Object Mapping using Mapster
[Back][1]

## Separate project
```bash
dotnet new console -o xx-test
cd xx-test
dotnet add package Mapster
```
## Base Codes
```cs
public record User(
  int Id,
  string FirstName,
  string LastName
);

public record UserResponse(
  int Id,
  string FirstName,
  string LastName
);

public static class UserGenerator
{
  static public User GenerateRandom()
  {
    return new User(1,"First", "Last");
  }
}
```
Program.cs
```cs
  User user = UserGenerator.GenerateRandom();
  UserResponse ur = new UserResponse(
    user.Id,
    user.FirstName,
    user.LastName
  );

  Console.WriteLine(user);
  Console.WriteLine(ur);
```

## Basic Usage

Program.cs
```cs
  using Mapster;

  User user = UserGenerator.GenerateRandom();
  UserResponse ur = user.Adapt<UserResponse>();

  Console.WriteLine(user);
  Console.WriteLine(ur);

```

## Using TypeAdapterConfig

```cs
public record UserResponse(
  int Id,
  string Fullname
);
```
Program.cs
```cs
  using Mapster;

  User user = UserGenerator.GenerateRandom();
  var config = new TypeAdapterConfig();
  config.NewConfig<User,UserResponse>()
    .Map(dest => dest.Fullname, src => $"{src.FirstName} {src.LastName}");
  UserResponse ur = user.Adapt<UserResponse>(config);
  Console.WriteLine(user);
  Console.WriteLine(ur);

```

## Using Global Config

Program.cs
```cs
  using Mapster;
  
  User user = UserGenerator.GenerateRandom();
  //Verson 1 - Working
  var config = TypeAdapterConfig.GlobalSettings;
  config.NewConfig<User,UserResponse>()
    .Map(dest => dest.Fullname, src => $"{src.FirstName} {src.LastName}");
  
  //Verson 2 - Working
  User user = UserGenerator.GenerateRandom();
  TypeAdapterConfig<User,UserResponse>.NewConfig()
    .Map(dest => dest.Fullname, src => $"{src.FirstName} {src.LastName}");

  UserResponse ur = user.Adapt<UserResponse>();
  Console.WriteLine(user);
  Console.WriteLine(ur);
```

## Using Rules

Program.cs
```cs
  using Mapster;
  
  User user = UserGenerator.GenerateRandom();
  var config = TypeAdapterConfig.GlobalSettings;
  //Works Only if the 3rd argument is satisfied
  config.NewConfig<User,UserResponse>()
    .Map(
      dest => dest.Fullname,
      src => $"{src.FirstName} {src.LastName}",
      src => src.FirstName.StartsWith("a", StringComparison.OrdinalIgnoreCase)
    );
  
  UserResponse ur = user.Adapt<UserResponse>();
  Console.WriteLine(user);
  Console.WriteLine(ur);
```

## Chaining

Program.cs
```cs
  using Mapster;
  
  User user = UserGenerator.GenerateRandom();
  //Mapping 1
  TypeAdapterConfig<User,UserResponse>.NewConfig()
    .Map(dest => dest.Fullname, src => $"{src.FirstName} {src.LastName}");

  //Mapping 2
  var config = TypeAdapterConfig.GlobalSettings;
  //Use ForType instead of NewConfig to chain
  config.ForType<User,UserResponse>()
    .Map(dest => dest.Id, src => src.Id + 1);
  

  UserResponse ur = user.Adapt<UserResponse>();

  Console.WriteLine(user);
  Console.WriteLine(ur);

```

## Combining 2 object to 1 mapping

```cs
public record UserResponse(
  int Id,
  string FirstName,
  string LastName,
  string TraceId
);
```
Program.cs
```cs
  using Mapster;
  
      var traceId = Guid.NewGuid();
      var user = UserGenerator.GenerateRandom();
      var config = TypeAdapterConfig.GlobalSettings;
      config.NewConfig<(User user, Guid traceId),UserResponse>()
        .Map(
          dest => dest.TraceId,
          src => src.traceId
        )
        //Working if this is omitted
        .Map(
          dest => dest,
          src => src.user
        )      
        ;
      
      UserResponse ur = (user, traceId).Adapt<UserResponse>();
      Console.WriteLine(user);
      Console.WriteLine(ur);

```

## After Mapping
```cs
      var traceId = Guid.NewGuid();
      var user = UserGenerator.GenerateRandom();
      var config = TypeAdapterConfig.GlobalSettings;
      config.NewConfig<(User user, Guid traceId),UserResponse>()
        .Map(
          dest => dest.TraceId,
          src => src.traceId
        )
        .AfterMapping(
          dest => Console.WriteLine(dest)
        )      
        ;
      
      UserResponse ur = (user, traceId).Adapt<UserResponse>();
      Console.WriteLine(user);
      Console.WriteLine(ur);
```

## Using IMapping

```cs
  using MapsterMapper;

  User user = UserGenerator.GenerateRandom();
  IMapper mapper = new Mapper();
  UserResponse ur = mapper.Map<UserResponse>(user);

  Console.WriteLine(user);
  Console.WriteLine(ur);
```
```cs
using Mapster;
using MapsterMapper;
      User user = UserGenerator.GenerateRandom();

      var config = new TypeAdapterConfig();
      config.NewConfig<User,UserResponse>()
        .Map(dest => dest.Fullname, src => $"{src.FirstName} {src.LastName}");
      IMapper mapper = new Mapper(config);
      UserResponse ur = mapper.Map<UserResponse>(user);

      Console.WriteLine(user);
      Console.WriteLine(ur);

```
```cs
using Mapster;
using MapsterMapper;

      User user = UserGenerator.GenerateRandom();

      var traceId = Guid.NewGuid();
      TypeAdapterConfig.GlobalSettings
        .NewConfig<(User user, Guid traceId),UserResponse>()
          .Map(
            dest => dest.TraceId,
            src => src.traceId
          )
          .AfterMapping(
            dest => Console.WriteLine(dest)
          )      
        ;
      IMapper mapper = new Mapper();
      UserResponse ur = mapper.Map<UserResponse>((user,traceId));

      Console.WriteLine(user);
      Console.WriteLine(ur);
```
[Top][0] | [Back to main][1]

[0]:#object-mapping-using-mapster
[1]:../../readme.md
