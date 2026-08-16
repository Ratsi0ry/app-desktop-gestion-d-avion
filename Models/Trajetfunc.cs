using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;
public class Trajetfunc
{
    public async Task<List<Trajet>> ListerTrajet()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Trajet
                .ToListAsync();
        }
    }
    public async Task<List<Trajet>> RechercheTrajet(Expression<Func<Trajet, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Trajet
                .Where(propriete)
                .ToListAsync();
        }
    }
}