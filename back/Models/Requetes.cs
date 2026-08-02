using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using back.Data;
namespace back.Models;

public class Requetes<Table, Keytype> where Table : class
{

    //lister tout le contenu de la table
    public List<Table> Lister()
    {
        using (var bdd= new Contextedb())
        {
            return bdd.Set<Table>().ToList();//besoin de Include()
        }
    }

    public Table? Resercher_cle(Keytype id)
    {
        using (var bdd = new Contextedb())
        {
            return bdd.Set<Table>().Find(id);
        }
    }

    public void Ajout(Table nouvligne)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Set<Table>().Add(nouvligne);
            bdd.SaveChanges();
        }
    }

    public void Modification(Table lignemodif)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Set<Table>().Update(lignemodif);
            bdd.SaveChanges();
        }
    }

    public void Suppression(Table supligne)
    {
        using (var bdd = new Contextedb())
        {
            bdd.Set<Table>().Remove(supligne);
            bdd.SaveChanges();
        }
    }

    public List<Table> Recherche(Func<Table, bool> propriete)
    {
        using (var bdd = new Contextedb())
        {
            return bdd.Set<Table>().Where(propriete).ToList();
        }
    }
}
