using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;

public class Volfunc
{
    public async Task<List<Vol>> ListerVol()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Vol
                .Include(v => v.Date_vol)
                .Include(v => v.Avion)
                .Include(v => v.Trajet)
                .ToListAsync();
        }
    }
    public async Task<List<Vol>> RechercheVol(Expression<Func<Vol, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await  bdd.Vol
            .Include(v => v.Date_vol)
            .Include(v => v.Avion)
            .Include(v => v.Trajet)
            .Where(propriete)
            .ToListAsync();
        }
    }
    public async Task<Vol> AjouterVol(Vol vol)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Vol.Add(vol);
            await bdd.SaveChangesAsync();
            return vol;
        }
    }

    public async Task ModifierVol(Vol vol)
    {
        using (var bdd = new Contextedb())
        {
            var existing = await bdd.Vol.FindAsync(vol.id_vol);
            if (existing != null)
            {
                existing.status_vol = vol.status_vol;
                existing.fk_date_depart = vol.fk_date_depart;
                existing.fk_id_trajet = vol.fk_id_trajet;
                existing.fk_id_avion = vol.fk_id_avion;
                await bdd.SaveChangesAsync();
            }
        }
    }
}