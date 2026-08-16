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
                .Include(av => av.Compagnie)
                .ToListAsync();
        }
    }
    public async Task<List<Avion>> RechercheAvion(Expression<Func<Avion, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Avion
                .Include(av => av.Compagnie)
                .Where(propriete)
                .ToListAsync();
        }
    }
    public async Task<Avion> AjouterAvion(Avion avion)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Avion.Add(avion);
            await bdd.SaveChangesAsync();
            return avion;
        }
    }

    public async Task ModifierAvion(Avion avion)
    {
        using (var bdd = new Contextedb())
        {
            var existing = await bdd.Avion.FindAsync(avion.id_avion);
            if (existing != null)
            {
                existing.nom_avion = avion.nom_avion;
                existing.fk_id_compagnie = avion.fk_id_compagnie;
                await bdd.SaveChangesAsync();
            }
        }
    }
}