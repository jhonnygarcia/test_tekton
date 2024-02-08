using Application._Common.Contracts;
using Domain.Main.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Main
{
    public interface IMainContext: IDbContext
    {
        DbSet<Product> Products { get; }
    }
}
