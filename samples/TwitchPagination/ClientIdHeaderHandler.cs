using Microsoft.Extensions.Options;

namespace TwitchPagination;

public class ClientIdHeaderHandler : DelegatingHandler
{
    private readonly IOptions<ClientData> _clientDataOptions;

    public ClientIdHeaderHandler(IOptions<ClientData> clientDataOptions)
    {
        _clientDataOptions = clientDataOptions;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("Client-Id", _clientDataOptions.Value.Id);

        return await base.SendAsync(request, cancellationToken);
    }
}
