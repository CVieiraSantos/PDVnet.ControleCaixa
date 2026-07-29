using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PDVnet.ControleCaixa.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Valor_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        string novoTexto = textBox.Text.Insert(
            textBox.SelectionStart,
            e.Text);

        Regex regex = new(@"^\d*(,\d{0,2})?$");

        e.Handled = !regex.IsMatch(novoTexto);
    }
}
