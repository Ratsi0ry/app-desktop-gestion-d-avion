using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using back.Data;
namespace back.Models;

public class Volfunc
{
    public async Task<List<Vol>> ListerVol()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Vol
                .Include(v => v.fk_date_depart)
                .Include(v => v.fk_id_avion)
                .Include(v => v.fk_id_trajet)
                .ToListAsync();
        }
    }
    public async Task<List<Vol>> RechercheVol(Expression<Func<Vol, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await  bdd.Vol
            .Include(v => v.fk_date_depart)
            .Include(v => v.fk_id_avion)
            .Include(v => v.fk_id_trajet)
            .Where(propriete)
            .ToListAsync();
        }
    }
}