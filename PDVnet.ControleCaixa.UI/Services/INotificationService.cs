namespace PDVnet.ControleCaixa.UI.Services
{
    public interface INotificationService
    {
        void Warning(string mensagem);

        void Error(string mensagem);

        void Information(string mensagem);
    }
}
