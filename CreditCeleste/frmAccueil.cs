﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreditCeleste
{
    public partial class frmAccueil : Form
    {
        public frmAccueil()
        {
            InitializeComponent();
        }

        private void frmAccueil_Load(object sender, EventArgs e)
        {
            // Globales unGlobal = new Globales();

            lblMonApplication.Text = Globales.nomUtilisateur;
            lblRegion.Text = Globales.region;

            Globales.uneConcession = new Concession("Garage Soares", "66 rue des Voyages");
            ajoutVendeur();
            ajoutVoitureOccas();
            ajoutVoitureNeuve();
            ajoutAncienClient();
        }

        private void btnIntro_Click(object sender, EventArgs e)
        {
            //if(Globales.fenIntro == null)
            //{
            //}

            // creation et ouverture de Introduction
            Globales.fenIntro = new frmIntro();
            Globales.fenIntro.FormClosed += new FormClosedEventHandler(FenIntro_FormClosed);
            Globales.fenIntro.Show();

            this.Hide();
        }

        void FenIntro_FormClosed(object sender, FormClosedEventArgs e)  // que faire a la fermeture de Introduction
        {
            this.Show();
        }

        private void btnEtude_Click(object sender, EventArgs e)
        {

        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            // creation et ouverture de Introduction
            Globales.fenTestBDD = new frmTestBDD();
            Globales.fenTestBDD.FormClosed += new FormClosedEventHandler(FenIntro_FormClosed);
            Globales.fenTestBDD.Show();

            this.Hide();
        }

        private void ajoutVendeur()
        {
            using (SqlConnection connection = new SqlConnection(Globales.connectionString))
            {
                connection.Open();
                string query = "SELECT civV, nomV, prenomV FROM VENDEUR"; // Récupère uniquement les infos nécessaires

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupération des données et concaténation dans le format souhaité
                        string civV = reader.GetString(0);
                        string nomV = reader.GetString(1);
                        string prenomV = reader.GetString(2);

                        // Création du vendeur avec les données récupérées
                        Vendeur unVendeur = new Vendeur(civV, nomV, prenomV);

                        // Ajout du vendeur dans la concession
                        Globales.uneConcession.ajoutVendeur(unVendeur);
                    }
                }
            }
        }


        private void ajoutVoitureOccas()
        {
            using (SqlConnection connection = new SqlConnection(Globales.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM VOITURE v INNER JOIN VOITURE_OCCASION vo ON v.numSerie = vo.numSerie WHERE v.stock = 'En Stock'"; // Récupère uniquement les infos nécessaires

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupération des données et concaténation dans le format souhaité
                        string NumS = reader.GetString(0);
                        string marque = reader.GetString(1);
                        string modele = reader.GetString(2);

                        foreach (VoitureOccasion xlistNumSeriesOcas in Globales.uneConcession.GetNumSeriesOcas())
                        {
                            Globales.uneConcession.ajoutNumSeriesOcas(xlistNumSeriesOcas);
                        }

                        // Création du vendeur avec les données récupérées
                        VoitureOccasion uneVoitureOccasion = new VoitureOccasion(NumS, marque, modele);

                        // Ajout du vendeur dans la concession
                        Globales.uneConcession.ajoutVoiture(uneVoitureOccasion);


                    }
                }
            }


        }



        private void ajoutVoitureNeuve()
        {
            using (SqlConnection connection = new SqlConnection(Globales.connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM VOITURE v INNER JOIN VOITURE_NEUVE vo ON v.numSerie = vo.numSerie WHERE v.stock = 'En Stock'"; // Récupère uniquement les infos nécessaires

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupération des données et concaténation dans le format souhaité
                        string NumS = reader.GetString(0);
                        string marque = reader.GetString(1);
                        string modele = reader.GetString(2);


                        foreach (VoitureNeuve xlistNumSeriesNeuve in Globales.uneConcession.GetNumSeriesNeuve())
                        {
                            Globales.uneConcession.ajoutNumSeriesNeuve(xlistNumSeriesNeuve);
                        }

                        // Création du vendeur avec les données récupérées
                        VoitureNeuve uneVoitureNeuve = new VoitureNeuve(NumS, marque, modele);

                        // Ajout du vendeur dans la concession
                        Globales.uneConcession.ajoutVoiture(uneVoitureNeuve);


                    }
                }
            }
        }


        private void ajoutAncienClient()
        {
            using (SqlConnection connection = new SqlConnection(Globales.connectionString))
            {
                connection.Open();
                string query = "SELECT numC, civ, nom, prenom FROM CLIENT"; // Récupère uniquement les infos nécessaires

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Récupération des données
                        int numC = reader.GetInt32(0);
                        string civC = reader.GetString(1);
                        string nomC = reader.GetString(2);
                        string prenomC = reader.GetString(3);

                        // Création du client avec les données récupérées
                        Client unClient = new Client(civC,nomC,prenomC);
                        Client unClientID = new Client(numC);


                        // Ajout du client dans la concession
                        Globales.uneConcession.ajoutClients(unClient);
                        Globales.uneConcession.ajoutClientsID(unClientID);



                    }
                }
            }
        }
    }

}