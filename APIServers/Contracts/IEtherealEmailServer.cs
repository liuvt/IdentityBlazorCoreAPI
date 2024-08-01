using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.APIServers.Contracts;

public interface IEtherealEmailServer
{
    void Send(EtherealEmail model);
} 
