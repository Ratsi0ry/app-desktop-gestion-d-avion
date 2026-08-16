using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;
public class Compagniefunc
{
    public async Task<List<Compagnie>> ListerCompagnie()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Compagnie
                .ToListAsync();
        }
    }
    public async Task<List<Compagnie>> RechercheCompagnie(Expression<Func<Compagnie, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Compagnie
                .Where(propriete)
                .ToListAsync();
        }
    }
}