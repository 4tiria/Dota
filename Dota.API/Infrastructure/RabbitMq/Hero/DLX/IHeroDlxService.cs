namespace Dota.API.Hero.RabbitMq.DLX;

public interface IHeroDlxService
{
    void Consume();
}