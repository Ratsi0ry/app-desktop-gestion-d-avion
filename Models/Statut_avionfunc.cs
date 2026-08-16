using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;
public class Statut_avionfunc
{
    public async Task<List<Statut_avion>> ListerStatutAvion()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Statut_avion
                .ToListAsync();
        }
    }
    public async Task<List<Statut_avion>> RechercheStatutAvion(Expression<Func<Statut_avion, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Statut_avion
                .Where(propriete)
                .ToListAsync();
        }
    }
    public async Task<Statut_avion> AjouterStatutAvion(Statut_avion statut)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Statut_avion.Add(statut);
            await bdd.SaveChangesAsync();
            return statut;
        }
    }
}