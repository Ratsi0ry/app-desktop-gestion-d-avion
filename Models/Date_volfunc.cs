using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using back.Data;
namespace back.Models;
public class Date_volfunc
{
    public async Task<List<Date_vol>> ListerDateVol()
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Date_vol
                .ToListAsync();
        }
    }
    public async Task<List<Date_vol>> RechercheDateVol(Expression<Func<Date_vol, bool>> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return await bdd.Date_vol
                .Where(propriete)
                .ToListAsync();
        }
    }
    public async Task<Date_vol> AjouterDateVol(Date_vol date_vol)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Date_vol.Add(date_vol);
            await bdd.SaveChangesAsync();
            return date_vol;
        }
    }
}