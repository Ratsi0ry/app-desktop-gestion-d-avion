using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using back.Data;
using System.Reflection.Metadata;
using System.Diagnostics.CodeAnalysis;
namespace back.Models;

public class Possederfunc
{
    public async Task<List<Posseder>> ListerPlaceAvion()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Posseder
                .Include(pos => pos.fk_id_avion)
                .Include(pos => pos.fk_numero_place)
                .ToListAsync();
        }
    }
    public async Task<List<Posseder>> RechercherPlaceAvion(Expression<Func<Posseder, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Posseder
            .Include(pos => pos.fk_id_avion)
            .Include(pos => pos.fk_numero_place)
            .Where(propriete)
            .ToListAsync();
        }
    }
}