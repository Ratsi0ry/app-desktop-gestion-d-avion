using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Storage;

namespace back.Models
{

    //CLASSES DES TABLES PRINCIPALES
    public class Compagnie
    {
        public string id_compagnie {get; set; } = string.Empty;
        public required string nom_compagnie {get; set; } = string.Empty;
        public required string tel_compagnie {get; set; } = string.Empty;
        public string email_compagnie {get; set; } = string.Empty;

        public List<Avion> Avions {get; set; } = new();
        public List<Repartir> Repartirs {get; set; } = new();
    }
    
    public class Avion
    {
        public string id_avion {get; set; } = string.Empty;
        public required string nom_avion {get; set; } = string.Empty;

       // public string fk_code_modele {get; set; } = null!;
        public string fk_id_compagnie {get; set; } = null!;

        public List<Vol> Vols {get; set; } = new();
        public List<Caracteriser> Caracterisers {get; set; } = new();
        public List<Posseder> Posseders {get; set; } = new();
        public Compagnie Compagnie {get; set; } = null!;
       // public Modele_avion Modele_avion {get; set; } = null!;
    }
    /*public class Modele_avion
    {
        public string code_modele {get; set; } = string.Empty;
        public required string libelle_modele {get; set; } = string.Empty;

        public List<Avion> Avions {get; set; } = new();
        //public List<Classe> Classes {get; set; } = new();
        public List<Diviser> Divisers {get; set; } = new();

    }*/

    /*public class Classe
    {
        public string code_classe {get; set; } = string.Empty;
        public required string libelle_classe {get; set; } = string.Empty;

        public List<Modele_avion> Modele_Avions {get; set; } = new();
        public List<Diviser> Divisers {get; set; } = new();
        public List<Regrouper> Regroupers {get; set; } = new();
        public Place Place {get; set; } = null!;
    }*/

    public class Place
    {
        public string numero_place {get; set; } = string.Empty;
        public required string classe_siege {get; set; } = string.Empty;
        public required int occupee {get; set; } = 0;

        public List<Avion> Avions {get; set; } = new();
        public List<Reservation> Reservations {get; set; } = new();
        public List<Posseder> Posseders {get; set; } = new();
        //public List<Regrouper> Regroupers {get; set; } = new(); 
    }
    
    public class Statut_avion
    {
        public string code_statut {get; set; } = string.Empty;
        public required string libelle_statut {get; set; } = string.Empty;

        public List<Avion> Avions {get; set; } = new();
        public List<Caracteriser> Caracterisers {get; set; } = new();

    }

    public class Vol
    {
        public string id_vol {get; set; } = string.Empty;

        //le type de la date ???
        /*public required string date_depart {get; set; } = string.Empty;
        public string date_arrivee {get; set; } = string.Empty;*/

        public string fk_date_depart {get; set; } = string.Empty;

        public string fk_id_trajet {get; set; } = null!;
        public string fk_id_avion {get; set; } = null!;

        public List<Billet> Billets {get; set; } = new();

        public List<Affecter> Affecters {get; set; } = new();
        public List<Reservation> Reservations {get; set; } = new();
        public Avion Avion {get; set; } = null!;
        public Trajet Trajet {get; set; } = null!;
        public Date_vol Date_vol {get; set; } = null!;

    }

    public class Date_vol
    {
        public string date_depart {get; set; } = string.Empty;

        public List<Vol> Vols {get; set; } = new();
    }

    public class Trajet
    {
        public string id_trajet {get; set; } = string.Empty;
        public required string lieu_depart {get; set; } = string.Empty;
        public required string destination {get; set; } = string.Empty;

        public List<Vol> Vols {get; set; } = new(); 
        public List<Repartir> Repartirs {get; set; } = new();

    }

    public class Pilote
    {
        public string id_pilote {get; set; } = string.Empty;
        public required string nom_pilote {get; set; } = string.Empty;
        public string prenom_pilote {get; set; } = string.Empty;
        public required string tel_pilote {get; set; } = string.Empty;

        public List<Affecter> Affecters {get; set; } = new();
    }

    public class Passager
    {
        public string passeport {get; set; } = string.Empty;
        public required string nom_passager {get; set; } = string.Empty;
        public string prenom_passager {get; set; } = string.Empty;
        public required string tel_passager {get; set; } = string.Empty;

        public required string categorie_passager {get; set; } = string.Empty;

        public List<Reservation> Reservations {get; set; } = new();
        public List<Billet> Billets {get; set; } = new();
    }

    public class Reservation
    {
        public string id_reservation {get; set; } = string.Empty;
        public required string date_reservation {get; set; } = string.Empty;
        public int valide {get; set; } = 0;

        public string fk_numero_place {get; set; } = null!;
        public string fk_passeport {get; set; } = null!;
        public string fk_id_vol {get; set; } = null!;

        public Place Place {get; set; } = null!;
        public Passager Passager {get; set; } = null!;
        public Vol Vol {get; set; } = null!;
        //public Billet Billet {get; set; } = null!;
    }

    public class Billet
    {
        public int numero_billet {get; set; }
        
        public string fk_passeport {get; set; } = null!;
        public string fk_id_vol {get; set; } = null!;

        //public Reservation Reservation {get; set; } = null!;
        public Vol Vol {get; set; } = null!;

        public Passager Passager {get; set; } = null!;
    }


/// /////////////   ////////////////////               
/// //////          ///             
/// /////           ////////////////      
/// /////                       ////
/// /////                      //// 
/// ///////////  //////////////////  
//CLASSES DES TABLES D'ASSOCIATIONS
    public class Affecter
    {
        public string fk_id_vol {get; set; } = null!;
        public string fk_id_pilote {get; set; } = null!;

        public Vol Vol {get; set; } = null!;
        public Pilote Pilote {get; set; } = null!;
    }
    public class Caracteriser
    {
        public string fk_code_statut {get; set; } = null!;
        public string fk_id_avion {get; set; } = null!;

        public Statut_avion Statut_avion {get; set; } = null!;
        public Avion Avion {get; set; } = null!;
    }
    public class Posseder
    {
        public string fk_numero_place {get; set;} = null!;
        public string fk_id_avion {get; set; } = null!;

        public Avion Avion {get; set; } = null!;
        public Place Place {get; set; } = null!;

    }

    /*public class Diviser
    {
        public string fk_code_modele {get; set; } = null!;
        public string fk_code_classe {get; set; } = null!;

        //public Modele_avion Modele_avion {get; set; } = null!;
       // public Classe Classe {get; set; } = null!;
    }*/

    /*public class Regrouper
    {
        public string fk_code_classe {get; set; } = null!;
        public string fk_numero_place {get; set; } = null!;

       // public Classe Classe {get; set; } = null!;
        public Place Place {get; set; } = null!;
    }*/
    public class Repartir
    {
        public string fk_id_compagnie {get; set; } = string.Empty;
        public string fk_id_trajet {get; set; } = string.Empty;

        public Compagnie Compagnie {get; set; } = null!;
        public Trajet Trajet {get; set; } = null!;
    }
}