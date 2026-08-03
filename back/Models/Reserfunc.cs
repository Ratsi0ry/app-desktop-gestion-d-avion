using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using back.Data;
namespace back.Models;

public class Reserfunc
{
    public async Task<List<Reservation>> ListerReservation()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Reservation 
                .Include(re => re.fk_id_vol)
                .Include(re => re.fk_numero_place)
                .Include(re => re.fk_numero_place)
                .ToListAsync();
        }
    }
    public async Task<List<Reservation>> RechercheReservation(Expression<Func<Reservation, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Reservation
                .Include(re => re.fk_id_vol)
                .Include(re => re.fk_numero_place)
                .Include(re => re.fk_passeport)
                .Where(propriete)
                .ToListAsync();
        }
    }
}