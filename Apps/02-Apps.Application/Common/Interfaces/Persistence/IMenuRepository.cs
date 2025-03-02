using Apps.Domain.Entities;
using Apps.Domain.MenuAggregate;

namespace Apps.Application.Common.Interfaces.Persistence;
public interface IMenuRepository
{
    void Add(Menu menu);
}
