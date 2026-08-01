//fichier cs pour piloter la base de donnees

//chemins a utiliser
using Microsoft.EntityFrameworkCore;
using back.Models;
using System.Security.Cryptography.X509Certificates;

namespace back.Data; //emplacement actuel

public class Contextedb : DbContext
{
    //pilote de la table CLIENT cree precedemment dans Models
    
    public DbSet<Vol> Vol {get; set; } = null!;
    public DbSet<Avion> Avion {get; set; } = null!;
    public DbSet<Modele_avion> Modele_avion {get; set; } = null!;
    public DbSet<Compagnie> Compagnie {get; set; } = null!;
    public DbSet<Classe> Classe {get; set; } = null!;
    public DbSet<Place> Place {get; set; } = null!;
    public DbSet<Statut_avion> Statut_avion {get; set; } = null!;
    public DbSet<Trajet> Trajet {get; set; } = null!;
    public DbSet<Pilote> Pilote {get; set; } = null!;
    public DbSet<Passager> Passager {get; set; } = null!;
    public DbSet<Reservation> Reservation {get; set; } = null!;
    public DbSet<Billet> Billet {get; set; } = null!;
    public DbSet<Affecter> Affecter {get; set; } = null!;
    public DbSet<Caracteriser> Caracteriser {get; set; } = null!;
    public DbSet<Posseder> Posseder {get; set; } = null!;
    public DbSet<Diviser> Diviser {get; set; } = null!;
    public DbSet<Regrouper> Regrouper {get; set; } = null!;

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            //Base.OnConfiguring(optionsBuilder);

            //EF Core cree la Base si elle n'existe pas encore
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gestion_vol04;Username=pilote;Password=csharp");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<entite>()
            //.ToTable("nom_table") //si le nom est different du nom de la classe
            //.Has
        modelBuilder.Entity<Compagnie>()
            .HasKey(co => co.id_compagnie );
        
        modelBuilder.Entity<Avion>()
            .HasKey(av => av.id_avion);
        modelBuilder.Entity<Avion>()
            .HasOne(av => av.Compagnie)
            .WithMany(co => co.Avions)
            .HasForeignKey(av => av.fk_id_compagnie)
            .HasConstraintName("fk_avion_compagnie");

        modelBuilder.Entity<Avion>()
            .HasOne(av => av.Modele_avion)
            .WithMany(mv => mv.Avions)
            .HasForeignKey(av => av.fk_code_modele)
            .HasConstraintName("fk_avion_modele");
        
        modelBuilder.Entity<Modele_avion>()
            .HasKey(mv => mv.code_modele);

        modelBuilder.Entity<Classe>()
            .HasKey(cl => cl.code_classe);

        modelBuilder.Entity<Place>()
            .HasKey(pl => pl.numero_place);
        
        modelBuilder.Entity<Statut_avion>()
            .HasKey(st => st.code_statut);
            
        modelBuilder.Entity<Vol>()
            .HasKey(v => v.id_vol);

        modelBuilder.Entity<Vol>()
            .HasOne(v => v.Trajet)
            .WithMany(tr => tr.Vols)
            .HasForeignKey(v => v.fk_id_trajet)
            .HasConstraintName("fk_vol_trajet");
        
        modelBuilder.Entity<Vol>()
            .HasOne(v => v.Avion)
            .WithMany(av => av.Vols)
            .HasForeignKey(v => v.fk_id_avion)
            .HasConstraintName("fk_vol_avion");
        
        modelBuilder.Entity<Trajet>()
            .HasKey(tr => tr.id_trajet);

        modelBuilder.Entity<Pilote>()
            .HasKey(pi => pi.id_pilote);

        modelBuilder.Entity<Passager>()
            .HasKey(pa => pa.passeport);

        modelBuilder.Entity<Reservation>()
            .HasKey(re => re.id_reservation);

        modelBuilder.Entity<Reservation>()
            .HasOne(re => re.Place)
            .WithMany(pl => pl.Reservations)
            .HasForeignKey(re => re.fk_numero_place)
            .HasConstraintName("fk_reservation_place");
        
        modelBuilder.Entity<Reservation>()
            .HasOne(re => re.Passager)
            .WithMany(pa => pa.Reservations)
            .HasForeignKey(re => re.fk_passeport)
            .HasConstraintName("fk_reservation_passager");


        modelBuilder.Entity<Billet>()
            .HasKey(b => b.numero_billet);

        modelBuilder.Entity<Billet>()
            .HasOne(b => b.Passager)
            .WithMany(pa => pa.Billets)
            .HasForeignKey(b => b.fk_passeport)
            .HasConstraintName("fk_billet_passager");

        modelBuilder.Entity<Billet>()
            .HasOne(b => b.Vol)
            .WithMany(v => v.Billets)
            .HasForeignKey(b => b.fk_id_vol)
            .HasConstraintName("fk_billet_vol");

        modelBuilder.Entity<Affecter>(ent =>
        {
            ent.HasKey(af => new{af.fk_id_pilote, af.fk_id_vol} );
            ent.HasOne(af => af.Pilote)
                .WithMany(p => p.Affecters)
                .HasForeignKey(af => af.fk_id_pilote)
                .HasConstraintName("fk_affecter_pilote");
            ent.HasOne(af => af.Vol)
                .WithMany(v => v.Affecters)
                .HasForeignKey(af => af.fk_id_vol)
                .HasConstraintName("fk_affecter_vol");


        });
        /*modelBuilder.Entity<Affecter>(ent =>
        {
            ent.HasOne(af => af.Pilote)
                .WithMany(p => p.Affecters)
                .HasForeignKey(af => af.fk_id_pilote)
                .HasConstraintName("fk_affecter_pilote");
        })*/

        modelBuilder.Entity<Caracteriser>(ent =>
        {
            ent.HasKey(car => new{car.fk_code_statut, car.fk_id_avion} );

            ent.HasOne(car => car.Statut_avion)
                .WithMany(st => st.Caracterisers)
                .HasForeignKey(car => car.fk_code_statut)
                .HasConstraintName("fk_caracteriser_statut_avion");
            
             ent.HasOne(car => car.Avion)
                .WithMany(av => av.Caracterisers)
                .HasForeignKey(car => car.fk_id_avion)
                .HasConstraintName("fk_caracteriser_avion");
            

        });
        modelBuilder.Entity<Posseder>(ent =>
        {
            ent.HasKey(po => new{po.fk_id_avion, po.fk_numero_place} );

            ent.HasOne(po => po.Avion)
            .WithMany(av => av.Posseders)
            .HasForeignKey(po => po.fk_id_avion)
            .HasConstraintName("fk_posseder_avion");

            ent.HasOne(po => po.Place)
            .WithMany(pl => pl.Posseders)
            .HasForeignKey(po => po.fk_numero_place)
            .HasConstraintName("fk_posseder_place");

        });
        modelBuilder.Entity<Diviser>(ent =>
        {
            ent.HasKey(div => new{div.fk_code_modele, div.fk_code_classe} );

             ent.HasOne(div => div.Modele_avion)
            .WithMany(mo => mo.Divisers)
            .HasForeignKey(div => div.fk_code_modele)
            .HasConstraintName("fk_diviser_modele_avion");

            ent.HasOne(div => div.Classe)
            .WithMany(cl => cl.Divisers)
            .HasForeignKey(div => div.fk_code_classe)
            .HasConstraintName("fk_diviser_classe");

        });

         modelBuilder.Entity<Regrouper>(ent =>
        {
            ent.HasKey(reg => new{reg.fk_code_classe, reg.fk_numero_place} );

             ent.HasOne(reg => reg.Classe)
            .WithMany(cl => cl.Regroupers)
            .HasForeignKey(reg => reg.fk_code_classe)
            .HasConstraintName("fk_regrouper_classe");

            ent.HasOne(reg => reg.Place)
            .WithMany(pl => pl.Regroupers)
            .HasForeignKey(reg => reg.fk_code_classe)
            .HasConstraintName("fk_regrouper_place");

        });
        
        
        
    }

}