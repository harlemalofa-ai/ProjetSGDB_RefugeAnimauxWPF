using Npgsql;
using RefugeAnimauxWPF.commandes;
using RefugeAnimauxWPF.coucheAccesBD;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace RefugeAnimauxWPF.coucheVueModele
{
    public class MainVueModele : BaseVueModele
    {
        private readonly AccesBD bd = new AccesBD();

        private DataView? tableauBord, animaux, contacts, adoptions, famillesAccueil, vaccins, vaccinations, couleurs, roles, compatibilites, animauxPresents, diagnostic;
        private DataView? ficheAnimalCouleurs;
        private DataView? ficheAnimalCompatibilites;
        private DataView? ficheAnimalVaccinations;
        private DataView? ficheAnimalHistorique;
        private DataView? ficheAnimalAdoptions;
        private DataView? ficheAnimalFamilles;
        private DataRowView? animalSelectionne, contactSelectionne, adoptionSelectionnee;
        private DataView? ficheContactRoles;
        private DataView? ficheContactAdoptions;
        private DataView? ficheContactFamilles;
        private DataView? ficheContactEntrees;
        private DataView? ficheContactSorties;
        private string rechercheAnimal = "", rechercheContact = "";

        public DataView? TableauBord { get => tableauBord; set { tableauBord = value; OnPropertyChanged(); } }
        public DataView? Animaux { get => animaux; set { animaux = value; AppliquerFiltreAnimaux(); OnPropertyChanged(); } }
        public DataView? Contacts { get => contacts; set { contacts = value; AppliquerFiltreContacts(); OnPropertyChanged(); } }
        public DataView? Adoptions { get => adoptions; set { adoptions = value; OnPropertyChanged(); } }
        public DataView? FamillesAccueil { get => famillesAccueil; set { famillesAccueil = value; OnPropertyChanged(); } }
        public DataView? Vaccins { get => vaccins; set { vaccins = value; OnPropertyChanged(); } }
        public DataView? Vaccinations { get => vaccinations; set { vaccinations = value; OnPropertyChanged(); } }
        public DataView? Couleurs { get => couleurs; set { couleurs = value; OnPropertyChanged(); } }
        public DataView? Roles { get => roles; set { roles = value; OnPropertyChanged(); } }
        public DataView? Compatibilites { get => compatibilites; set { compatibilites = value; OnPropertyChanged(); } }
        public DataView? AnimauxPresents { get => animauxPresents; set { animauxPresents = value; OnPropertyChanged(); } }
        public DataView? Diagnostic { get => diagnostic; set { diagnostic = value; OnPropertyChanged(); } }

        public DataRowView? AnimalSelectionne
        {
            get => animalSelectionne;
            set
            {
                animalSelectionne = value;
                OnPropertyChanged();
                ChargerFicheAnimalDepuisSelection();
            }
        }
        public DataRowView? ContactSelectionne { get => contactSelectionne; set { contactSelectionne = value; OnPropertyChanged(); RemplirContactDepuisSelection(); } }
        public DataRowView? AdoptionSelectionnee { get => adoptionSelectionnee; set { adoptionSelectionnee = value; OnPropertyChanged(); RemplirAdoptionDepuisSelection(); } }

        public string RechercheAnimal { get => rechercheAnimal; set { rechercheAnimal = value; AppliquerFiltreAnimaux(); OnPropertyChanged(); } }
        public string RechercheContact { get => rechercheContact; set { rechercheContact = value; AppliquerFiltreContacts(); OnPropertyChanged(); } }

        public string NouveauAnimalIdentifiant { get; set; } = "";
        public string NouveauAnimalNom { get; set; } = "";
        public string NouveauAnimalType { get; set; } = "chat";
        public string NouveauAnimalSexe { get; set; } = "M";
        public string NouveauAnimalParticularites { get; set; } = "";
        public string NouveauAnimalDescription { get; set; } = "";
        public bool NouveauAnimalSterilise { get; set; }
        public DateTime NouveauAnimalDateNaissance { get; set; } = DateTime.Today.AddYears(-1);
        public string NouveauAnimalRaisonEntree { get; set; } = "abandon";
        public string NouveauAnimalContactEntree { get; set; } = "";
        public DateTime NouveauAnimalDateEntree { get; set; } = DateTime.Today;

        public string ContactId { get; set; } = "";
        public string ContactNom { get; set; } = "";
        public string ContactPrenom { get; set; } = "";
        public string ContactRue { get; set; } = "";
        public string ContactCp { get; set; } = "";
        public string ContactLocalite { get; set; } = "";
        public string ContactRegistre { get; set; } = "";
        public string ContactGsm { get; set; } = "";
        public string ContactTelephone { get; set; } = "";
        public string ContactEmail { get; set; } = "";

        public string AdoptionAnimal { get; set; } = "";
        public string AdoptionContact { get; set; } = "";
        public DateTime AdoptionDate { get; set; } = DateTime.Today;
        public string AdoptionStatut { get; set; } = "demande";

        public string FamilleAnimal { get; set; } = "";
        public string FamilleContact { get; set; } = "";
        public DateTime FamilleDateDebut { get; set; } = DateTime.Today;
        public string FamilleContactRetour { get; set; } = "";
        public DateTime FamilleDateFin { get; set; } = DateTime.Today;

        public string VaccinId { get; set; } = "";
        public string VaccinNom { get; set; } = "";
        public string VaccinationAnimal { get; set; } = "";
        public string VaccinationVaccin { get; set; } = "";
        public DateTime VaccinationDate { get; set; } = DateTime.Today;

        public string CouleurId { get; set; } = "";
        public string CouleurNom { get; set; } = "";
        public string RoleId { get; set; } = "";
        public string RoleNom { get; set; } = "";
        public string AssocAnimalCouleur { get; set; } = "";
        public string AssocCouleurId { get; set; } = "";
        public string AssocContactRole { get; set; } = "";
        public string AssocRoleId { get; set; } = "";
        public string AssocAnimalCompat { get; set; } = "";
        public string AssocCompatId { get; set; } = "";
        public string AssocCompatValeur { get; set; } = "oui";
        public string AssocCompatDescription { get; set; } = "";

        public ICommand ChargerToutCommande { get; }
        public ICommand AjouterAnimalCommande { get; }
        public ICommand AjouterContactCommande { get; }
        public ICommand ModifierContactCommande { get; }
        public ICommand AjouterAdoptionCommande { get; }
        public ICommand ModifierStatutAdoptionCommande { get; }
        public ICommand AjouterFamilleCommande { get; }
        public ICommand CloturerFamilleCommande { get; }
        public ICommand AjouterVaccinCommande { get; }
        public ICommand AjouterVaccinationCommande { get; }
        public ICommand AjouterCouleurCommande { get; }
        public ICommand AjouterRoleCommande { get; }
        public ICommand AjouterCouleurAnimalCommande { get; }
        public ICommand AjouterRoleContactCommande { get; }
        public ICommand AjouterCompatibiliteAnimalCommande { get; }

        public MainVueModele()
        {
            ChargerToutCommande = new CommandeRelais(ChargerTout);
            AjouterAnimalCommande = new CommandeRelais(AjouterAnimal);
            AjouterContactCommande = new CommandeRelais(AjouterContact);
            ModifierContactCommande = new CommandeRelais(ModifierContact);
            AjouterAdoptionCommande = new CommandeRelais(AjouterAdoption);
            ModifierStatutAdoptionCommande = new CommandeRelais(ModifierStatutAdoption);
            AjouterFamilleCommande = new CommandeRelais(AjouterFamille);
            CloturerFamilleCommande = new CommandeRelais(CloturerFamille);
            AjouterVaccinCommande = new CommandeRelais(AjouterVaccin);
            AjouterVaccinationCommande = new CommandeRelais(AjouterVaccination);
            AjouterCouleurCommande = new CommandeRelais(AjouterCouleur);
            AjouterRoleCommande = new CommandeRelais(AjouterRole);
            AjouterCouleurAnimalCommande = new CommandeRelais(AjouterCouleurAnimal);
            AjouterRoleContactCommande = new CommandeRelais(AjouterRoleContact);
            AjouterCompatibiliteAnimalCommande = new CommandeRelais(AjouterCompatibiliteAnimal);

            ChargerTout();
        }

        private void Executer(Action action, string messageSucces = "")
        {
            try
            {
                action();
                if (!string.IsNullOrWhiteSpace(messageSucces))
                    MessageBox.Show(messageSucces, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.MessageText, "Erreur base de données", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int LireInt(string valeur, string nomChamp)
        {
            if (!int.TryParse(valeur, out int resultat))
                throw new Exception(nomChamp + " doit être un nombre.");
            return resultat;
        }

        private void VerifierNonVide(string valeur, string nomChamp)
        {
            if (string.IsNullOrWhiteSpace(valeur))
                throw new Exception(nomChamp + " est obligatoire.");
        }

        private string EchaperFiltre(string valeur) => valeur.Replace("'", "''");

        private void AppliquerFiltreAnimaux()
        {
            if (Animaux == null) return;
            Animaux.RowFilter = string.IsNullOrWhiteSpace(RechercheAnimal)
                ? ""
                : $"nom LIKE '%{EchaperFiltre(RechercheAnimal)}%' OR identifiant LIKE '%{EchaperFiltre(RechercheAnimal)}%' OR type LIKE '%{EchaperFiltre(RechercheAnimal)}%' OR etat_actuel LIKE '%{EchaperFiltre(RechercheAnimal)}%'";
        }

        private void AppliquerFiltreContacts()
        {
            if (Contacts == null) return;
            Contacts.RowFilter = string.IsNullOrWhiteSpace(RechercheContact)
                ? ""
                : $"nom LIKE '%{EchaperFiltre(RechercheContact)}%' OR prenom LIKE '%{EchaperFiltre(RechercheContact)}%' OR email LIKE '%{EchaperFiltre(RechercheContact)}%' OR gsm LIKE '%{EchaperFiltre(RechercheContact)}%'";
        }

        private void ChargerTout()
        {
            Executer(() =>
            {
                TableauBord = bd.GetTableauBord();
                Animaux = bd.GetAnimaux();
                Contacts = bd.GetContacts();
                Adoptions = bd.GetAdoptions();
                FamillesAccueil = bd.GetFamillesAccueil();
                Vaccins = bd.GetVaccins();
                Vaccinations = bd.GetVaccinations();
                Couleurs = bd.GetCouleurs();
                Roles = bd.GetRoles();
                Compatibilites = bd.GetCompatibilites();
                AnimauxPresents = bd.GetAnimauxPresents();
                Diagnostic = bd.GetDiagnostic();
            });
        }

        private void AjouterAnimal()
        {
            Executer(() =>
            {
                VerifierNonVide(NouveauAnimalIdentifiant, "Identifiant animal");
                VerifierNonVide(NouveauAnimalNom, "Nom animal");
                if (NouveauAnimalDateNaissance > DateTime.Today) throw new Exception("La date de naissance ne peut pas être dans le futur.");

                bd.AjouterAnimalAvecEntree(NouveauAnimalIdentifiant, NouveauAnimalNom, NouveauAnimalType, NouveauAnimalSexe,
                    NouveauAnimalParticularites, NouveauAnimalDescription, NouveauAnimalSterilise,
                    NouveauAnimalDateNaissance, NouveauAnimalRaisonEntree, LireInt(NouveauAnimalContactEntree, "Contact entrée"), NouveauAnimalDateEntree);

                ChargerTout();
            }, "Animal ajouté avec son entrée au refuge.");
        }

        private void AjouterContact()
        {
            Executer(() =>
            {
                VerifierNonVide(ContactNom, "Nom contact");
                VerifierNonVide(ContactPrenom, "Prénom contact");
                VerifierNonVide(ContactRue, "Rue contact");
                VerifierNonVide(ContactCp, "Code postal");
                VerifierNonVide(ContactLocalite, "Localité");
                VerifierNonVide(ContactRegistre, "Registre national");

                bd.AjouterContact(LireInt(ContactId, "Identifiant contact"), ContactNom, ContactPrenom, ContactRue, ContactCp, ContactLocalite, ContactRegistre, ContactGsm, ContactTelephone, ContactEmail);
                ChargerTout();
            }, "Contact ajouté.");
        }

        private void ModifierContact()
        {
            Executer(() =>
            {
                bd.ModifierContact(LireInt(ContactId, "Identifiant contact"), ContactNom, ContactPrenom, ContactRue, ContactCp, ContactLocalite, ContactRegistre, ContactGsm, ContactTelephone, ContactEmail);
                ChargerTout();
            }, "Contact modifié.");
        }

        private void RemplirContactDepuisSelection()
        {
            if (ContactSelectionne == null) return;

            ContactId = ContactSelectionne["contact_identifiant"].ToString() ?? "";
            ContactNom = ContactSelectionne["nom"].ToString() ?? "";
            ContactPrenom = ContactSelectionne["prenom"].ToString() ?? "";
            ContactRue = ContactSelectionne["rue"].ToString() ?? "";
            ContactCp = ContactSelectionne["cp"].ToString() ?? "";
            ContactLocalite = ContactSelectionne["localite"].ToString() ?? "";
            ContactRegistre = ContactSelectionne["registre_national"].ToString() ?? "";
            ContactGsm = ContactSelectionne["gsm"].ToString() ?? "";
            ContactTelephone = ContactSelectionne["telephone"].ToString() ?? "";
            ContactEmail = ContactSelectionne["email"].ToString() ?? "";

            OnPropertyChanged(nameof(ContactId));
            OnPropertyChanged(nameof(ContactNom));
            OnPropertyChanged(nameof(ContactPrenom));
            OnPropertyChanged(nameof(ContactRue));
            OnPropertyChanged(nameof(ContactCp));
            OnPropertyChanged(nameof(ContactLocalite));
            OnPropertyChanged(nameof(ContactRegistre));
            OnPropertyChanged(nameof(ContactGsm));
            OnPropertyChanged(nameof(ContactTelephone));
            OnPropertyChanged(nameof(ContactEmail));

            ChargerFicheContactDepuisSelection();
        }

        private void AjouterAdoption()
        {
            Executer(() =>
            {
                VerifierNonVide(AdoptionAnimal, "Animal adoption");
                bd.AjouterAdoption(AdoptionAnimal, LireInt(AdoptionContact, "Contact adoption"), AdoptionDate, AdoptionStatut);
                ChargerTout();
            }, "Adoption ajoutée.");
        }

        private void ModifierStatutAdoption()
        {
            Executer(() =>
            {
                bd.ModifierStatutAdoption(AdoptionAnimal, LireInt(AdoptionContact, "Contact adoption"), AdoptionDate, AdoptionStatut);
                ChargerTout();
            }, "Statut d'adoption modifié.");
        }

        private void RemplirAdoptionDepuisSelection()
        {
            if (AdoptionSelectionnee == null) return;
            AdoptionAnimal = AdoptionSelectionnee["animal"].ToString() ?? "";
            AdoptionContact = AdoptionSelectionnee["contact"].ToString() ?? "";
            object valeurDate = AdoptionSelectionnee["date_demande"];

            if (valeurDate is DateOnly dateOnly)
            {
                AdoptionDate = dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                AdoptionDate = Convert.ToDateTime(valeurDate);
            }
            AdoptionStatut = AdoptionSelectionnee["statut"].ToString() ?? "demande";
            OnPropertyChanged(nameof(AdoptionAnimal));
            OnPropertyChanged(nameof(AdoptionContact));
            OnPropertyChanged(nameof(AdoptionDate));
            OnPropertyChanged(nameof(AdoptionStatut));
        }

        private void AjouterFamille()
        {
            Executer(() =>
            {
                bd.AjouterFamilleAccueil(FamilleAnimal, LireInt(FamilleContact, "Contact famille"), FamilleDateDebut);
                ChargerTout();
            }, "Famille d'accueil ajoutée.");
        }

        private void CloturerFamille()
        {
            Executer(() =>
            {
                bd.CloturerFamilleAccueil(FamilleAnimal, LireInt(FamilleContactRetour, "Contact retour"), FamilleDateFin);
                ChargerTout();
            }, "Famille d'accueil clôturée.");
        }

        private void AjouterVaccin()
        {
            Executer(() =>
            {
                bd.AjouterVaccin(LireInt(VaccinId, "Identifiant vaccin"), VaccinNom);
                ChargerTout();
            }, "Vaccin ajouté.");
        }

        private void AjouterVaccination()
        {
            Executer(() =>
            {
                bd.AjouterVaccination(VaccinationAnimal, LireInt(VaccinationVaccin, "Vaccin"), VaccinationDate);
                ChargerTout();
            }, "Vaccination ajoutée.");
        }

        private void AjouterCouleur()
        {
            Executer(() =>
            {
                bd.AjouterCouleur(LireInt(CouleurId, "Identifiant couleur"), CouleurNom);
                ChargerTout();
            }, "Couleur ajoutée.");
        }

        private void AjouterRole()
        {
            Executer(() =>
            {
                bd.AjouterRole(LireInt(RoleId, "Identifiant rôle"), RoleNom);
                ChargerTout();
            }, "Rôle ajouté.");
        }

        private void AjouterCouleurAnimal()
        {
            Executer(() => bd.AjouterCouleurAnimal(AssocAnimalCouleur, LireInt(AssocCouleurId, "Couleur")), "Couleur associée à l'animal.");
        }

        private void AjouterRoleContact()
        {
            Executer(() => bd.AjouterRoleContact(LireInt(AssocContactRole, "Contact"), LireInt(AssocRoleId, "Rôle")), "Rôle associé au contact.");
        }

        private void AjouterCompatibiliteAnimal()
        {
            Executer(() => bd.AjouterCompatibiliteAnimal(AssocAnimalCompat, LireInt(AssocCompatId, "Compatibilité"), AssocCompatValeur, AssocCompatDescription), "Compatibilité associée à l'animal.");
        }

        public DataView? FicheAnimalCouleurs
        {
            get => ficheAnimalCouleurs;
            set
            {
                ficheAnimalCouleurs = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheAnimalCompatibilites
        {
            get => ficheAnimalCompatibilites;
            set
            {
                ficheAnimalCompatibilites = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheAnimalVaccinations
        {
            get => ficheAnimalVaccinations;
            set
            {
                ficheAnimalVaccinations = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheAnimalHistorique
        {
            get => ficheAnimalHistorique;
            set
            {
                ficheAnimalHistorique = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheAnimalAdoptions
        {
            get => ficheAnimalAdoptions;
            set
            {
                ficheAnimalAdoptions = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheAnimalFamilles
        {
            get => ficheAnimalFamilles;
            set
            {
                ficheAnimalFamilles = value;
                OnPropertyChanged();
            }
        }

        private void ChargerFicheAnimalDepuisSelection()
        {
            if (AnimalSelectionne == null)
            {
                FicheAnimalCouleurs = null;
                FicheAnimalCompatibilites = null;
                FicheAnimalVaccinations = null;
                FicheAnimalHistorique = null;
                FicheAnimalAdoptions = null;
                FicheAnimalFamilles = null;
                return;
            }

            string animal = AnimalSelectionne["identifiant"].ToString() ?? "";

            if (string.IsNullOrWhiteSpace(animal))
            {
                return;
            }

            Executer(() =>
            {
                FicheAnimalCouleurs = bd.GetFicheAnimalCouleurs(animal);
                FicheAnimalCompatibilites = bd.GetFicheAnimalCompatibilites(animal);
                FicheAnimalVaccinations = bd.GetFicheAnimalVaccinations(animal);
                FicheAnimalHistorique = bd.GetFicheAnimalHistorique(animal);
                FicheAnimalAdoptions = bd.GetFicheAnimalAdoptions(animal);
                FicheAnimalFamilles = bd.GetFicheAnimalFamilles(animal);
            });
        }

        public DataView? FicheContactRoles
        {
            get => ficheContactRoles;
            set
            {
                ficheContactRoles = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheContactAdoptions
        {
            get => ficheContactAdoptions;
            set
            {
                ficheContactAdoptions = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheContactFamilles
        {
            get => ficheContactFamilles;
            set
            {
                ficheContactFamilles = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheContactEntrees
        {
            get => ficheContactEntrees;
            set
            {
                ficheContactEntrees = value;
                OnPropertyChanged();
            }
        }

        public DataView? FicheContactSorties
        {
            get => ficheContactSorties;
            set
            {
                ficheContactSorties = value;
                OnPropertyChanged();
            }
        }

        private void ChargerFicheContactDepuisSelection()
        {
            if (ContactSelectionne == null)
            {
                FicheContactRoles = null;
                FicheContactAdoptions = null;
                FicheContactFamilles = null;
                FicheContactEntrees = null;
                FicheContactSorties = null;
                return;
            }

            if (!int.TryParse(ContactId, out int contact))
            {
                return;
            }

            Executer(() =>
            {
                FicheContactRoles = bd.GetFicheContactRoles(contact);
                FicheContactAdoptions = bd.GetFicheContactAdoptions(contact);
                FicheContactFamilles = bd.GetFicheContactFamilles(contact);
                FicheContactEntrees = bd.GetFicheContactEntrees(contact);
                FicheContactSorties = bd.GetFicheContactSorties(contact);
            });
        }
    }
}
