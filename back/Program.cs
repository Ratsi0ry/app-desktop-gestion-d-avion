using System;
using back.Data;
using back.Models;

Console.WriteLine("Demarrage de l'application");

//initialisation du contexte de base de donnees
using (var contexte = new Contextedb());
