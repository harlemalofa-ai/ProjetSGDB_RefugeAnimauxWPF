namespace RefugeAnimauxWPF.classesMetier
{
    public class Role
    {
        public int RolIdentifiant { get; set; }
        public string RolNom { get; set; } = "";

        public Role() { }

        public Role(int rolIdentifiant, string rolNom)
        {
            RolIdentifiant = rolIdentifiant;
            RolNom = rolNom;
        }
    }
}