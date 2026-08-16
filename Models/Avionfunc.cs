using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;

public class Avionfunc
{
    public async Task<List<Avion>> ListerAvions()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Avion
                .Include(av => av.fk_id_compagnie)
                .ToListAsync();
        }
    }
    public async Task<List<Avion>> RechercheAvion(Expression<Func<Avion, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Avion
                .Include(av => av.fk_id_compagnie)
                .Where(propriete)
                .ToListAsync();
        }
    }
}