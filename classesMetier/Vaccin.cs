namespace RefugeAnimauxWPF.classesMetier
{
    public class Vaccin
    {
        public int Identifiant { get; set; }

        public string Nom { get; set; } = "";

        public Vaccin()
        {
        }

        public Vaccin(
            int identifiant,
            string nom)
        {
            Identifiant = identifiant;
            Nom = nom;
        }
    }
}