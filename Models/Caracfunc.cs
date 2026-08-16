using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
using System.Diagnostics.CodeAnalysis;
namespace back.Models;

public class Caracfunc
{
    public async Task<List<Caracteriser>> ListerAvionStatut()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Caracteriser
                .Include(car => car.Statut_avion)
                .Include(car => car.Avion)
                .ToListAsync();
        }
    }
    public async Task<List<Caracteriser>> RechercheAvionStatut(Expression<Func<Caracteriser, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Caracteriser
                .Include(car => car.Statut_avion)
                .Include(car => car.Avion)
                .Where(propriete)
                .ToListAsync();
        }
    }

    public async Task<Caracteriser> AjouterAvionStatut(Caracteriser caracteriser)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Caracteriser.Add(caracteriser);
            await bdd.SaveChangesAsync();
            return caracteriser;
        }
    }

    public async Task SupprimerAvionStatut(string fk_id_avion, string fk_code_statut)
    {
        using (var bdd = new Contextedb())
        {
            var existing = await bdd.Caracteriser.FindAsync(fk_code_statut, fk_id_avion);
            if (existing != null)
            {
                bdd.Caracteriser.Remove(existing);
                await bdd.SaveChangesAsync();
            }
        }
    }
}