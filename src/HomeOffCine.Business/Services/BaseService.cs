using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Notifications;
using FluentValidation.Results;

namespace HomeOffCine.Business.Services;

public abstract class BaseService
{
    private readonly INotificator _notificator;

    protected BaseService(INotificator notificator)
    {
        _notificator = notificator;
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var errorMessage in validationResult.Errors.Select(e => e.ErrorMessage))
        {
            _notificator.Handle(new Notification(errorMessage));
        }
    }

    protected void Notificar(string mensagem)
    {
        _notificator.Handle(new Notification(mensagem));
    }
}
