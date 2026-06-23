namespace RefugeAnimauxWPF.classesMetier
{
    public class FamilleAccueil
    {
        public string FaAniIdentifiant { get; set; } = "";

        public int FaContact { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public FamilleAccueil()
        {
        }

        public FamilleAccueil(
            string faAniIdentifiant,
            int faContact,
            DateTime dateDebut,
            DateTime? dateFin)
        {
            FaAniIdentifiant = faAniIdentifiant;
            FaContact = faContact;
            DateDebut = dateDebut;
            DateFin = dateFin;
        }
    }
}