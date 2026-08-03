using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using back.Data;
using System.Data.Common;
namespace back.Models;

public class Affecterfunc
{
    public async Task<List<Affecter>> PiloteVol()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Affecter
                .Include(af => af.fk_id_pilote)
                .Include(af => af.fk_id_vol)
                .ToListAsync();
            
        }
    }

    public async Task<List<Affecter>> RechercherPiloteVol(Expression<Func<Affecter, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Affecter
                .Include(af => af.fk_id_pilote)
                .Include(af => af.fk_id_pilote)
                .Where(propriete)
                .ToListAsync();
        }
    }
}