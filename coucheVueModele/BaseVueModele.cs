using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RefugeAnimauxWPF.coucheVueModele
{
    public class BaseVueModele : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? nomPropriete = null)
        {
            if (nomPropriete != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }
    }
}
