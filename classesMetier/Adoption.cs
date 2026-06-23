namespace RefugeAnimauxWPF.classesMetier
{
    public class Adoption
    {
        public string AniIdentifiant { get; set; } = "";

        public int AdopContact { get; set; }

        public DateTime DateDemande { get; set; }

        public string Statut { get; set; } = "";

        public Adoption()
        {
        }

        public Adoption(
            string aniIdentifiant,
            int adopContact,
            DateTime dateDemande,
            string statut)
        {
            AniIdentifiant = aniIdentifiant;
            AdopContact = adopContact;
            DateDemande = dateDemande;
            Statut = statut;
        }
    }
}