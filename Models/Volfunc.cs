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
}