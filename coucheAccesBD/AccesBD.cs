using Npgsql;
using System;
using System.Data;

namespace RefugeAnimauxWPF.coucheAccesBD
{
    public class AccesBD
    {
        private readonly string chaineConnexion =
            "Host=localhost;" +
            "Database=refuge_animaux;" +
            "Username=postgres;" +
            "Password=Jesus2001";

        private DataView ExecuterTable(string sql)
        {
            DataTable table = new DataTable();

            using (NpgsqlConnection connexion = new NpgsqlConnection(chaineConnexion))
            {
                connexion.Open();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                using (NpgsqlDataAdapter adaptateur = new NpgsqlDataAdapter(commande))
                {
                    adaptateur.Fill(table);
                }
            }

            return table.DefaultView;
        }

        private DataView ExecuterTableAvecParametre(string sql, string nomParametre, string valeurParametre)
        {
            DataTable table = new DataTable();

            using (NpgsqlConnection connexion = new NpgsqlConnection(chaineConnexion))
            {
                connexion.Open();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue(nomParametre, valeurParametre);

                    using (NpgsqlDataAdapter adaptateur = new NpgsqlDataAdapter(commande))
                    {
                        adaptateur.Fill(table);
                    }
                }
            }

            return table.DefaultView;
        }

        private DataView ExecuterTableAvecParametreInt(string sql, string nomParametre, int valeurParametre)
        {
            DataTable table = new DataTable();

            using (NpgsqlConnection connexion = new NpgsqlConnection(chaineConnexion))
            {
                connexion.Open();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    commande.Parameters.AddWithValue(nomParametre, valeurParametre);

                    using (NpgsqlDataAdapter adaptateur = new NpgsqlDataAdapter(commande))
                    {
                        adaptateur.Fill(table);
                    }
                }
            }

            return table.DefaultView;
        }
        private void ExecuterProcedure(string sql, Action<NpgsqlCommand> ajouterParametres)
        {
            using (NpgsqlConnection connexion = new NpgsqlConnection(chaineConnexion))
            {
                connexion.Open();

                using (NpgsqlCommand commande = new NpgsqlCommand(sql, connexion))
                {
                    ajouterParametres(commande);
                    commande.ExecuteNonQuery();
                }
            }
        }

        public DataView GetTableauBord() => ExecuterTable("SELECT * FROM get_tableau_bord()");
        public DataView GetAnimaux() => ExecuterTable("SELECT * FROM get_animaux()");
        public DataView GetContacts() => ExecuterTable("SELECT * FROM get_contacts()");
        public DataView GetAdoptions() => ExecuterTable("SELECT * FROM get_adoptions()");
        public DataView GetFamillesAccueil() => ExecuterTable("SELECT * FROM get_familles_accueil()");
        public DataView GetVaccins() => ExecuterTable("SELECT * FROM get_vaccins()");
        public DataView GetVaccinations() => ExecuterTable("SELECT * FROM get_vaccinations()");
        public DataView GetCouleurs() => ExecuterTable("SELECT * FROM get_couleurs()");
        public DataView GetRoles() => ExecuterTable("SELECT * FROM get_roles()");
        public DataView GetCompatibilites() => ExecuterTable("SELECT * FROM get_compatibilites()");
        public DataView GetAnimauxPresents() => ExecuterTable("SELECT * FROM get_animaux_presents()");
        public DataView GetDiagnostic() => ExecuterTable("SELECT * FROM verifier_coherence_refuge()");

        public void AjouterContact(int id, string nom, string prenom, string rue, string cp, string localite, string registre, string gsm, string telephone, string email)
        {
            ExecuterProcedure(
                "CALL ajouter_contact(@id, @nom, @prenom, @rue, @cp, @localite, @registre, @gsm, @telephone, @email)",
                c =>
                {
                    c.Parameters.AddWithValue("@id", id);
                    c.Parameters.AddWithValue("@nom", nom);
                    c.Parameters.AddWithValue("@prenom", prenom);
                    c.Parameters.AddWithValue("@rue", rue);
                    c.Parameters.AddWithValue("@cp", cp);
                    c.Parameters.AddWithValue("@localite", localite);
                    c.Parameters.AddWithValue("@registre", registre);
                    c.Parameters.AddWithValue("@gsm", gsm ?? "");
                    c.Parameters.AddWithValue("@telephone", telephone ?? "");
                    c.Parameters.AddWithValue("@email", email ?? "");
                });
        }

        public void ModifierContact(int id, string nom, string prenom, string rue, string cp, string localite, string registre, string gsm, string telephone, string email)
        {
            ExecuterProcedure(
                "CALL modifier_contact(@id, @nom, @prenom, @rue, @cp, @localite, @registre, @gsm, @telephone, @email)",
                c =>
                {
                    c.Parameters.AddWithValue("@id", id);
                    c.Parameters.AddWithValue("@nom", nom);
                    c.Parameters.AddWithValue("@prenom", prenom);
                    c.Parameters.AddWithValue("@rue", rue);
                    c.Parameters.AddWithValue("@cp", cp);
                    c.Parameters.AddWithValue("@localite", localite);
                    c.Parameters.AddWithValue("@registre", registre);
                    c.Parameters.AddWithValue("@gsm", gsm ?? "");
                    c.Parameters.AddWithValue("@telephone", telephone ?? "");
                    c.Parameters.AddWithValue("@email", email ?? "");
                });
        }

        public void AjouterAnimalAvecEntree(string identifiant, string nom, string type, string sexe, string particularites, string description, bool sterilise, DateTime dateNaissance, string raisonEntree, int contactEntree, DateTime dateEntree)
        {
            ExecuterProcedure(
                "CALL ajouter_animal_avec_entree(@identifiant, @nom, @type, @sexe, @particularites, @description, @sterilise, @date_naissance, @raison_entree, @contact_entree, @date_entree)",
                c =>
                {
                    c.Parameters.AddWithValue("@identifiant", identifiant);
                    c.Parameters.AddWithValue("@nom", nom);
                    c.Parameters.AddWithValue("@type", type);
                    c.Parameters.AddWithValue("@sexe", sexe);
                    c.Parameters.AddWithValue("@particularites", particularites ?? "");
                    c.Parameters.AddWithValue("@description", description ?? "");
                    c.Parameters.AddWithValue("@sterilise", sterilise);
                    c.Parameters.AddWithValue("@date_naissance", dateNaissance);
                    c.Parameters.AddWithValue("@raison_entree", raisonEntree);
                    c.Parameters.AddWithValue("@contact_entree", contactEntree);
                    c.Parameters.AddWithValue("@date_entree", dateEntree);
                });
        }

        public void AjouterAdoption(string animal, int contact, DateTime dateDemande, string statut)
        {
            ExecuterProcedure("CALL ajouter_adoption(@animal, @contact, @date_demande, @statut)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@contact", contact);
                c.Parameters.AddWithValue("@date_demande", dateDemande);
                c.Parameters.AddWithValue("@statut", statut);
            });
        }

        public void ModifierStatutAdoption(string animal, int contact, DateTime dateDemande, string statut)
        {
            ExecuterProcedure("CALL modifier_statut_adoption(@animal, @contact, @date_demande, @statut)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@contact", contact);
                c.Parameters.AddWithValue("@date_demande", dateDemande);
                c.Parameters.AddWithValue("@statut", statut);
            });
        }

        public void AjouterFamilleAccueil(string animal, int contact, DateTime dateDebut)
        {
            ExecuterProcedure("CALL ajouter_famille_accueil(@animal, @contact, @date_debut)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@contact", contact);
                c.Parameters.AddWithValue("@date_debut", dateDebut);
            });
        }

        public void CloturerFamilleAccueil(string animal, int contactRetour, DateTime dateFin)
        {
            ExecuterProcedure("CALL cloturer_famille_accueil(@animal, @contact_retour, @date_fin)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@contact_retour", contactRetour);
                c.Parameters.AddWithValue("@date_fin", dateFin);
            });
        }

        public void AjouterVaccin(int id, string nom)
        {
            ExecuterProcedure("CALL ajouter_vaccin(@id, @nom)", c =>
            {
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@nom", nom);
            });
        }

        public void AjouterVaccination(string animal, int vaccin, DateTime dateVaccination)
        {
            ExecuterProcedure("CALL ajouter_vaccination(@animal, @vaccin, @date_vaccination)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@vaccin", vaccin);
                c.Parameters.AddWithValue("@date_vaccination", dateVaccination);
            });
        }

        public void AjouterCouleur(int id, string nom)
        {
            ExecuterProcedure("CALL ajouter_couleur(@id, @nom)", c =>
            {
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@nom", nom);
            });
        }

        public void AjouterRole(int id, string nom)
        {
            ExecuterProcedure("CALL ajouter_role(@id, @nom)", c =>
            {
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@nom", nom);
            });
        }

        public void AjouterCouleurAnimal(string animal, int couleur)
        {
            ExecuterProcedure("CALL ajouter_couleur_animal(@animal, @couleur)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@couleur", couleur);
            });
        }

        public void AjouterRoleContact(int contact, int role)
        {
            ExecuterProcedure("CALL ajouter_role_contact(@contact, @role)", c =>
            {
                c.Parameters.AddWithValue("@contact", contact);
                c.Parameters.AddWithValue("@role", role);
            });
        }

        public void AjouterCompatibiliteAnimal(string animal, int compatibilite, string valeur, string description)
        {
            ExecuterProcedure("CALL ajouter_compatibilite_animal(@animal, @compatibilite, @valeur, @description)", c =>
            {
                c.Parameters.AddWithValue("@animal", animal);
                c.Parameters.AddWithValue("@compatibilite", compatibilite);
                c.Parameters.AddWithValue("@valeur", valeur);
                c.Parameters.AddWithValue("@description", description ?? "");
            });
        }

        public DataView GetFicheAnimalCouleurs(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_couleurs(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheAnimalCompatibilites(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_compatibilites(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheAnimalVaccinations(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_vaccinations(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheAnimalHistorique(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_historique(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheAnimalAdoptions(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_adoptions(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheAnimalFamilles(string animal)
        {
            return ExecuterTableAvecParametre(
                "SELECT * FROM get_fiche_animal_familles(@animal)",
                "@animal",
                animal
            );
        }

        public DataView GetFicheContactRoles(int contact)
        {
            return ExecuterTableAvecParametreInt(
                "SELECT * FROM get_fiche_contact_roles(@contact)",
                "@contact",
                contact
            );
        }

        public DataView GetFicheContactAdoptions(int contact)
        {
            return ExecuterTableAvecParametreInt(
                "SELECT * FROM get_fiche_contact_adoptions(@contact)",
                "@contact",
                contact
            );
        }

        public DataView GetFicheContactFamilles(int contact)
        {
            return ExecuterTableAvecParametreInt(
                "SELECT * FROM get_fiche_contact_familles(@contact)",
                "@contact",
                contact
            );
        }

        public DataView GetFicheContactEntrees(int contact)
        {
            return ExecuterTableAvecParametreInt(
                "SELECT * FROM get_fiche_contact_entrees(@contact)",
                "@contact",
                contact
            );
        }

        public DataView GetFicheContactSorties(int contact)
        {
            return ExecuterTableAvecParametreInt(
                "SELECT * FROM get_fiche_contact_sorties(@contact)",
                "@contact",
                contact
            );
        }
    }
}
