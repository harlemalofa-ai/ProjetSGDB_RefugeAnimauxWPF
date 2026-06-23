namespace RefugeAnimauxWPF.classesMetier
{
    public class Couleur
    {
        public int ColIdentifiant { get; set; }
        public string NomCouleur { get; set; } = "";

        public Couleur() { }

        public Couleur(int colIdentifiant, string nomCouleur)
        {
            ColIdentifiant = colIdentifiant;
            NomCouleur = nomCouleur;
        }
    }
}