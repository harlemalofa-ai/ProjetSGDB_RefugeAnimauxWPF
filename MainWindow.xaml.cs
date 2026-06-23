using RefugeAnimauxWPF.coucheVueModele;
using System.Windows;

namespace RefugeAnimauxWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVueModele();
        }
    }
}
