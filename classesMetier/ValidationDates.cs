namespace RefugeAnimauxWPF.classesMetier
{
    public static class ValidationDates
    {
        public static bool DateFuture(DateTime date)
        {
            return date.Date > DateTime.Today;
        }

        public static bool DateAvant(DateTime date1, DateTime date2)
        {
            return date1.Date < date2.Date;
        }
    }
}