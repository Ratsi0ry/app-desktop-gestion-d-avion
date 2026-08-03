using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                .Include(car => car.fk_code_statut)
                .Include(car => car.fk_id_avion)
                .ToListAsync();
        }
    }

    public async Task<List<Caracteriser>> RechercheAvionStatut(Expression<Func<Caracteriser, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Caracteriser
                .Include(car => car.fk_code_statut)
                .Include(car => car.fk_id_avion)
                .Where(propriete)
                .ToListAsync();
        }
    }
}