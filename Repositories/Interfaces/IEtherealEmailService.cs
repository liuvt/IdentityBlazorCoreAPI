using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.Repositories.Interfaces;

public interface IEtherealEmailService
{
    Task<bool> Send(EtherealEmail model);
} 
