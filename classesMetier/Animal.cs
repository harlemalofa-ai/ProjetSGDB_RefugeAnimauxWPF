namespace RefugeAnimauxWPF.classesMetier
{
    public class Animal
    {
        public string Identifiant { get; set; } = "";
        public string Nom { get; set; } = "";
        public string Type { get; set; } = "";
        public string Sexe { get; set; } = "";
        public string? Particularites { get; set; }
        public DateTime? DateDeces { get; set; }
        public string? Description { get; set; }
        public DateTime? DateSterilisation { get; set; }
        public bool Sterilise { get; set; }
        public DateTime DateNaissance { get; set; }

        public Animal()
        {
        }

        public Animal(
            string identifiant,
            string nom,
            string type,
            string sexe,
            string? particularites,
            DateTime? dateDeces,
            string? description,
            DateTime? dateSterilisation,
            bool sterilise,
            DateTime dateNaissance)
        {
            Identifiant = identifiant;
            Nom = nom;
            Type = type;
            Sexe = sexe;
            Particularites = particularites;
            DateDeces = dateDeces;
            Description = description;
            DateSterilisation = dateSterilisation;
            Sterilise = sterilise;
            DateNaissance = dateNaissance;
        }
    }
}