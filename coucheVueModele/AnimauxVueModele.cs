using RefugeAnimauxWPF.classesMetier;
using RefugeAnimauxWPF.commandes;
using RefugeAnimauxWPF.coucheAccesBD;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RefugeAnimauxWPF.coucheVueModele
{
    public class AnimauxVueModele : INotifyPropertyChanged
    {
        private readonly AccesBD bd;

        private ObservableCollection<Animal> animaux;
        private ObservableCollection<Contact> contacts;

        private Animal? animalSelectionne;
        private Contact? contactSelectionne;

        public ObservableCollection<Animal> Animaux
        {
            get { return animaux; }
            set
            {
                animaux = value;
                OnPropertyChanged(nameof(Animaux));
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }

        public Animal? AnimalSelectionne
        {
            get { return animalSelectionne; }
            set
            {
                animalSelectionne = value;
                OnPropertyChanged(nameof(AnimalSelectionne));
            }
        }

        public Contact? ContactSelectionne
        {
            get { return contactSelectionne; }
            set
            {
                contactSelectionne = value;
                OnPropertyChanged(nameof(ContactSelectionne));
            }
        }

        public string NouveauContactId { get; set; } = "";
        public string NouveauContactNom { get; set; } = "";
        public string NouveauContactPrenom { get; set; } = "";
        public string NouveauContactRue { get; set; } = "";
        public string NouveauContactCp { get; set; } = "";
        public string NouveauContactLocalite { get; set; } = "";
        public string NouveauContactRegistreNational { get; set; } = "";
        public string NouveauContactGsm { get; set; } = "";
        public string NouveauContactTelephone { get; set; } = "";
        public string NouveauContactEmail { get; set; } = "";

        public ICommand ChargerAnimauxCommande { get; }
        public ICommand ChargerContactsCommande { get; }
        public ICommand AjouterContactCommande { get; }

        public AnimauxVueModele()
        {
            bd = new AccesBD();

            animaux = new ObservableCollection<Animal>();
            contacts = new ObservableCollection<Contact>();

            ChargerAnimauxCommande = new CommandeRelais(ChargerAnimaux);
            ChargerContactsCommande = new CommandeRelais(ChargerContacts);
            AjouterContactCommande = new CommandeRelais(AjouterContact);
        }

        private void ChargerAnimaux()
        {
            try
            {
                Animaux = bd.GetAnimaux();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur chargement animaux", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChargerContacts()
        {
            try
            {
                Contacts = bd.GetContacts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur chargement contacts", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AjouterContact()
        {
            try
            {
                if (!int.TryParse(NouveauContactId, out int id))
                {
                    MessageBox.Show("L'identifiant doit être un nombre.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(NouveauContactNom) ||
                    string.IsNullOrWhiteSpace(NouveauContactPrenom) ||
                    string.IsNullOrWhiteSpace(NouveauContactRue) ||
                    string.IsNullOrWhiteSpace(NouveauContactCp) ||
                    string.IsNullOrWhiteSpace(NouveauContactLocalite) ||
                    string.IsNullOrWhiteSpace(NouveauContactRegistreNational))
                {
                    MessageBox.Show("Veuillez remplir les champs obligatoires.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Contact contact = new Contact(
                    id,
                    NouveauContactNom,
                    NouveauContactPrenom,
                    NouveauContactRue,
                    NouveauContactCp,
                    NouveauContactLocalite,
                    NouveauContactRegistreNational,
                    NouveauContactGsm,
                    NouveauContactTelephone,
                    NouveauContactEmail
                );

                bd.AjouterContact(contact);

                Contacts = bd.GetContacts();

                MessageBox.Show("Contact ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur ajout contact", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string nomPropriete)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}