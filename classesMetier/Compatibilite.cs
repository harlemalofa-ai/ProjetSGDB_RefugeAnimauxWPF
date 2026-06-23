namespace RefugeAnimauxWPF.classesMetier
{
    public class Compatibilite
    {
        public int Identifiant { get; set; }
        public string Type { get; set; }

        public Compatibilite(int identifiant, string type)
        {
            Identifiant = identifiant;
            Type = type;
        }
    }
}