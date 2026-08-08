using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;

namespace back.Models;

public class Billetfunc
{
    public async Task<List<Billet>> ListerBillet()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Billet
                .Include(bi => bi.fk_id_vol)
                .Include(bi => bi.fk_passeport)
                .ToListAsync();
        }

    }
    public async Task<List<Billet>> RechercheBillet(Expression<Func<Billet, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Billet
                .Include(bi => bi.fk_id_vol)
                .Include(bi => bi.fk_passeport)
                .Where(propriete)
                .ToListAsync();
        }
    }
}