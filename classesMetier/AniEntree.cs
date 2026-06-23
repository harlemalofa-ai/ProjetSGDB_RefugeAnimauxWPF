namespace RefugeAnimauxWPF.classesMetier
{
    public class AniEntree
    {
        public string AniIdentifiant { get; set; } = "";

        public DateTime DateEntree { get; set; }

        public string Raison { get; set; } = "";

        public int EntreeContact { get; set; }

        public AniEntree() { }

        public AniEntree(
            string aniIdentifiant,
            DateTime dateEntree,
            string raison,
            int entreeContact)
        {
            AniIdentifiant = aniIdentifiant;
            DateEntree = dateEntree;
            Raison = raison;
            EntreeContact = entreeContact;
        }
    }
}