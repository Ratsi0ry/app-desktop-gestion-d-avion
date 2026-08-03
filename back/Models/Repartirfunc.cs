using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using back.Data;
namespace back.Models;

public class Repartirfunc
{
    public async Task<List<Repartir>> ListerTrajetCompagnie()
    {
        using (var bdd = new Contextedb())
        {
          return await bdd.Repartir
            .Include(rep => rep.fk_id_compagnie)   
            .Include(rep => rep.fk_id_trajet)
            .ToListAsync();
        }
    }

    public async Task<List<Repartir>> RechercheTrajetCompagnie(Expression<Func<Repartir, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Repartir
                .Include(rep => rep.fk_id_compagnie)   
                .Include(rep => rep.fk_id_trajet)
                .Where(propriete)
                .ToListAsync();
        }
    }
}