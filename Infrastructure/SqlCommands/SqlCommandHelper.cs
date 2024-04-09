using Domain.Common;
using Infrastructure.Helpers;
using MediatR;
using MySqlConnector;

namespace Infrastructure.SqlCommands;

public class SqlCommandHelper(IMediator mediator)
{
    protected async Task<int> GetLastId(MySqlConnection conn)
    {
        var comm = new MySqlCommand(await SqlScriptRetriever.GetScript("GetLastId"), conn);
        var id = await comm.ExecuteScalarAsync();

        return Convert.ToInt32(id);
    }

    protected async Task PublishMediatorEvents(BaseEntity entity)
    {
        if (!entity.DomainEvents.Any())
        {
            return;
        }

        foreach (var domainEvent in entity.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}