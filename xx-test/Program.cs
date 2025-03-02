using xxtest;
using Mapster;
using MapsterMapper;
class Program
{
    static void Main()
    {      
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
   }
}
