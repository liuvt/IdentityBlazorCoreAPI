using System.Net;
using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Repositories.Interfaces;

namespace IdentityBlazorCoreAPI.Repositories.Services;

public class EtherealEmailService : IEtherealEmailService
{
    private readonly HttpClient httpClient;
    //Constructor
    public EtherealEmailService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<bool> Send(EtherealEmail model)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<EtherealEmail>("api/EtherealEmail/", model);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return false;
                }

                return true;
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}