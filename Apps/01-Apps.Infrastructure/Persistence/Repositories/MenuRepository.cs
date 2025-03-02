using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.MenuAggregate;

namespace Apps.Infrastructure.Persistence.Repositories;
public class MenuRepository: IMenuRepository
{
    // //Temporary storage: In-Memory
    // // Make static so that it is shared across all instances of UserRepository, not new instance per request
    // private static readonly List<Menu> _menu = new();
    private readonly AppsDBContext _dbContext;

    public MenuRepository(AppsDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Menu menu)
    {
        // _menu.Add(menu);
        _dbContext.Add(menu);
        _dbContext.SaveChanges();
    }


}
