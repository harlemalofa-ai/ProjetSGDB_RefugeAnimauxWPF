namespace RefugeAnimauxWPF.classesMetier
{
    public class Vaccination
    {
        public string VacAnimal { get; set; } = "";

        public int IdVaccin { get; set; }

        public DateTime VaccinationDate { get; set; }

        public Vaccination()
        {
        }

        public Vaccination(
            string vacAnimal,
            int idVaccin,
            DateTime vaccinationDate)
        {
            VacAnimal = vacAnimal;
            IdVaccin = idVaccin;
            VaccinationDate = vaccinationDate;
        }
    }
}