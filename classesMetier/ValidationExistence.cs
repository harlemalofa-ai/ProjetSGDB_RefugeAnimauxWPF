using RefugeAnimauxWPF.coucheAccesBD;

namespace RefugeAnimauxWPF.classesMetier
{
    public static class ValidationExistence
    {
        public static bool AnimalExiste(AccesBD bd, string identifiant)
        {
            return bd.AnimalExiste(identifiant);
        }

        public static bool ContactExiste(AccesBD bd, int identifiant)
        {
            return bd.ContactExiste(identifiant);
        }

        public static bool VaccinExiste(AccesBD bd, int identifiant)
        {
            return bd.VaccinExiste(identifiant);
        }

        public static bool CouleurExiste(AccesBD bd, int identifiant)
        {
            return bd.CouleurExiste(identifiant);
        }

        public static bool RoleExiste(AccesBD bd, int identifiant)
        {
            return bd.RoleExiste(identifiant);
        }

        public static bool CompatibiliteExiste(AccesBD bd, int identifiant)
        {
            return bd.CompatibiliteExiste(identifiant);
        }
    }
}