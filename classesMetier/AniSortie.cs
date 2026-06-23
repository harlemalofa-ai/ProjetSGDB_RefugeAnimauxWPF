namespace RefugeAnimauxWPF.classesMetier
{
    public class AniSortie
    {
        public string AniIdentifiant { get; set; } = "";

        public DateTime DateSortie { get; set; }

        public string Raison { get; set; } = "";

        public int SortieContact { get; set; }

        public AniSortie() { }

        public AniSortie(
            string aniIdentifiant,
            DateTime dateSortie,
            string raison,
            int sortieContact)
        {
            AniIdentifiant = aniIdentifiant;
            DateSortie = dateSortie;
            Raison = raison;
            SortieContact = sortieContact;
        }
    }
}