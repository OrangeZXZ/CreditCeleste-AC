﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CreditCeleste
{
    public partial class frmIntro : Form
    {
        public frmIntro()
        {
            InitializeComponent();
        }

        // Ce lance a l'ouverture du le page
        private void frmIntro_Load(object sender, EventArgs e)
        {
            // Recuperation des informations
            if (Globales.unClient != null)
            {
                // récupération des éléments du client
                cboCiv.Text = Globales.unClient.getCivClient();
                txtNom.Text = Globales.unClient.getNomClient();
                txtPrenom.Text = Globales.unClient.getPrenomClient();
            }

            // FAIRE AUSSI POUR ANCIEN VEHICULE //

            // Affiche nom vendeur dans le label
            lblVendeur.Text = Globales.nomVendeur;

            // Rajout des vendeurs au combobox
            foreach (Vendeur unVendeur in Globales.uneConcession.getLesVendeurs())
            {
                cboVendeur.Items.Add(unVendeur.getInfoVendeur());
            }

            // Rajout des vendeurs au combobox
            foreach (Client unClient in Globales.uneConcession.getLesClients())
            {
                cboChoixC.Items.Add(unClient.getInfoClients());
            }



            // Desactive le bouton Valider
            btnValider.Enabled = false;
        }

        // Fonction pour le bouton Enregistrer
        private void btnEnregistre_Click(object sender, EventArgs e)
        {
            // Sauvegarde les saisie dans des variables
            string civilite = cboCiv.Text;
            string nom = txtNom.Text;
            string prenom = txtPrenom.Text;

            string vendeur = cboVendeur.Text;


            // Verification de la saisie
            if (verifierSaisie(civilite, nom, prenom, vendeur))
            {
                // Sauvegarde dans Globales
                Globales.unClient = new Client(civilite, nom, prenom);
                Globales.nomVendeur = vendeur;


                // Affiche nom vendeur
                lblVendeur.Text = vendeur;

                // Affiche un message
                string affichage =
                    "Client: " + civilite + " " + nom + " " + prenom + " " + Environment.NewLine +
                    "Vendeur: " + vendeur + Environment.NewLine;

                MessageBox.Show(affichage, "Enregistrer", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Active le bouton Valider
                btnValider.Enabled = true;
            }
        }

        // Fonction pour vérifier si les saisies sont valides
        private bool verifierSaisie(string civilite, string nom, string prenom, string vendeur)
        {
            // Variable
            bool valeur = true;

            // Verifie les champs obligatoires
            if (string.IsNullOrWhiteSpace(civilite) || string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(vendeur))
            {
                // Affiche un message d'erreur
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                valeur = false; // Retourne faux si une valeur n'est pas saisie
            }

            // Retourne la Variable
            return valeur;
        }

        private void btnVoiture_Click(object sender, EventArgs e)
        {
            // Creation d'une page VoitureNeuve
            Globales.fenVoiture = new frmVoitureNeuve();
            Globales.fenVoiture.FormClosed += new FormClosedEventHandler(FenVoiture_FormClosed);

            // Masquer Intro
            this.Hide();

            // Ouverture de la page VoitureNeuve
            Globales.fenVoiture.Show();
        }

        void FenVoiture_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Affiche intro a la fermeture de VoitureNeuve
            this.Show();
        }

        private void cboVendeur_SelectedIndexChanged(object sender, EventArgs e) // A FAIRE AU PROPRE //
        {
            // Que faire a la selection d'un item dans le combobox
            lblVendeur.Text = Globales.uneConcession.getLesVendeurs()[cboVendeur.SelectedIndex].getInfoVendeur(); // CA PEUT ETRE SIMPLIFIER?
        }



        //Fonction pour le bouton Valider
        private void btnValider_Click(object sender, EventArgs e) // A FAIRE //
        {
            CnxBDD();
        }

        //Fonction pour la connexion à la base de donnée
        private void CnxBDD()
        {
            try
            {
                // Sauvegarde le texte saisi dans des variables
                string civiliteC = cboCiv.Text;
                string nomC = txtNom.Text;
                string prenomC = txtPrenom.Text;
                int numV = cboVendeur.SelectedIndex + 1;

                // Requête SQL avec OUTPUT pour récupérer l'ID auto-incrémenté
                string query = "INSERT INTO CLIENT (civ, nom, prenom, numV) OUTPUT INSERTED.NumC VALUES (@civC, @nomC, @prenomC, @numV)";

                using (SqlConnection oConnexion = new SqlConnection(Globales.connectionString))
                {
                    oConnexion.Open();

                    using (SqlCommand cmd = new SqlCommand(query, oConnexion))
                    {
                        cmd.Parameters.Add(new SqlParameter("@civC", SqlDbType.NVarChar) { Value = civiliteC });
                        cmd.Parameters.Add(new SqlParameter("@nomC", SqlDbType.NVarChar) { Value = nomC });
                        cmd.Parameters.Add(new SqlParameter("@prenomC", SqlDbType.NVarChar) { Value = prenomC });
                        cmd.Parameters.Add(new SqlParameter("@numV", SqlDbType.Int) { Value = numV });

                        // Exécuter la commande et récupérer l'ID inséré
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            Globales.IdClient = Convert.ToInt32(result);
                            MessageBox.Show($"Les données ont été enregistrées avec succès !\nNumC : {Globales.IdClient}");
                        }
                        else
                        {
                            MessageBox.Show("Aucune donnée n'a été enregistrée.");
                        }
                    }
                }

                Globales.nomClient = nomC + " " + prenomC;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}");
            }
        }


        //Fonction pour le bouton VoitureOccasion
        private void btnVoitureOccasion_Click(object sender, EventArgs e)
        {
            // Creation d'une page VoitureNeuve
            Globales.fenVoitureOccasion = new frmVoitureOccasion();
            Globales.fenVoitureOccasion.FormClosed += new FormClosedEventHandler(FenVoitureOccasion_FormClosed);

            // Masquer Intro
            this.Hide();

            // Ouverture de la page VoitureNeuve
            Globales.fenVoitureOccasion.Show();
        }

        // Que faire a la fermeture de VoitureOccasion
        void FenVoitureOccasion_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Afficher Intro si VoitureOccasion est fermée
            this.Show();
        }

        // Bouton Retour
        private void btnInfo_Click(object sender, EventArgs e)
        {
            this.Close(); // afficher Intro, retour en arrière, on affiche un écran à la fois 
        }

        private void cboChoixC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Vérifie si un client est sélectionné
                if (cboChoixC.SelectedIndex >= 0)
                {
                    // Récupération directe des données affichées dans le ComboBox
                    string[] infosClient = cboChoixC.SelectedItem.ToString().Split(' '); // Supposons que c'est "Nom Prénom"

                    if (infosClient.Length >= 2)
                    {
                        // Mise à jour des TextBox avec les infos sélectionnées
                        txtNom.Text = infosClient[1]; // Nom
                        txtPrenom.Text = infosClient[2]; // Prénom


                        // Récupération de l'ID correspondant via la liste des IDs
                        int idClient = Globales.uneConcession.GetClientsID()[cboChoixC.SelectedIndex].getIDClient();
                        Globales.IdClient = idClient;

                        // Connexion à la base de données pour récupérer le numV du vendeur (index dans la combo)
                        using (SqlConnection connection = new SqlConnection(Globales.connectionString))
                        {
                            connection.Open();
                            string query = "SELECT numV FROM CLIENT WHERE numC = @numC"; // Récupère le numV du vendeur

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@numC", idClient); // numC = ID client

                                // Exécute la commande et récupère un SqlDataReader
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read()) // Lire la première ligne de résultat
                                    {
                                        // Récupérer le numV du vendeur à partir du premier champ
                                        int numV = reader.GetInt32(0);

                                        // Ajustement de l'index (si numV commence à 1 et que la ComboBox commence à 0)
                                        int adjustedIndex = numV - 1;

                                        // Sélectionner l'index du vendeur dans la ComboBox
                                        if (adjustedIndex >= 0 && adjustedIndex < cboVendeur.Items.Count)
                                        {
                                            cboVendeur.SelectedIndex = adjustedIndex;
                                        }
                                    }
                                }
                            }
                        }

                        // Mise à jour de la combo box de civilité si nécessaire
                        string civClient = Globales.uneConcession.getLesClients()[cboChoixC.SelectedIndex].getCivClient();
                        if (cboCiv.Items.Contains(civClient))
                        {
                            cboCiv.SelectedItem = civClient;
                        }

                        // Sauvegarde les saisie dans des variables
                        string civilite = cboCiv.Text;
                        string nom = txtNom.Text;
                        string prenom = txtPrenom.Text;
                        string vendeur = cboVendeur.Text;
                        string nomprenom = txtNom.Text + " " + txtPrenom.Text;

                        // Sauvegarde dans Globales
                        Globales.unClient = new Client(civilite, nom, prenom);
                        Globales.nomVendeur = vendeur;
                        Globales.nomClient = nomprenom;

                        btnValider.Enabled = false;
                        btnEnregistre.Enabled = false;


                        MessageBox.Show("Veuillez passer à la suite.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }








    }


}
