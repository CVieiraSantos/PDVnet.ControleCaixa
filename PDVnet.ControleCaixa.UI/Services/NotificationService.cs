using System.Windows;

namespace PDVnet.ControleCaixa.UI.Services
{
    public class NotificationService : INotificationService
    {
        public void Warning(string mensagem)
        {
            MessageBox.Show(
               mensagem,
               "Atenção",
               MessageBoxButton.OK,
               MessageBoxImage.Warning);
        }

        public void Error(string mensagem)
        {
            MessageBox.Show(
                mensagem,
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public void Information(string mensagem)
        {
            MessageBox.Show(
                mensagem,
                "Informação",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
