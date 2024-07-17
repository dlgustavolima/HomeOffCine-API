using HomeOffCine.Business.Notifications;

namespace HomeOffCine.Business.Interfaces;

public interface INotificator
{
    bool TemNotificacao();
    List<Notification> ObterNotificacoes();
    void Handle(Notification notificacao);
}
