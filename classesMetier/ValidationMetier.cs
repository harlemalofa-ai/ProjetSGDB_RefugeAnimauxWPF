namespace RefugeAnimauxWPF.classesMetier
{
    public static class ValidationMetier
    {
        public static bool PeutAjouterAdoption(string etat)
        {
            return etat == "au_refuge";
        }

        public static bool PeutAjouterFamilleAccueil(string etat)
        {
            return etat == "au_refuge";
        }

        public static bool PeutAjouterSortie(string etat)
        {
            return etat == "au_refuge";
        }

        public static bool PeutAjouterEntree(string etat)
        {
            return etat == "famille_accueil";
        }

        public static bool PeutAjouterVaccination(string etat)
        {
            return etat != "decede"
                && etat != "retour_proprietaire";
        }

        public static string MessageActionImpossible(string etat, string action)
        {
            string etatLisible = etat switch
            {
                "au_refuge" => "au refuge",
                "famille_accueil" => "en famille d'accueil",
                "adopte" => "adopté",
                "decede" => "décédé",
                "retour_proprietaire" => "retourné chez son propriétaire",
                "sans_entree" => "sans entrée enregistrée",
                _ => "dans un état inconnu"
            };

            return $"Action impossible : l'animal est actuellement {etatLisible}. Action demandée : {action}.";
        }
    }
}