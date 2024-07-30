using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Data.Entities;
using System.Net;

namespace IdentityBlazorCoreAPI.Repositories.Services;

public class UserService : IUserService
{
    private readonly HttpClient httpClient;

    //Constructor
    public UserService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<List<AppUser>> Gets()
    {
        try
        {
            var response = await httpClient.GetAsync($"api/User/Gets");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(List<AppUser>);
                }

                return await response.Content.ReadFromJsonAsync<List<AppUser>>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> Create(UserCreateDTO models)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<UserCreateDTO>("api/User/Create", models);

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

    public async Task<bool> Delete(string userId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/User/Delete/{userId}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
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
            throw;
        }
    }

    // Sử dụng PostAsJsonAsync để thay thế DeleteAsync truyền một FromBody
    public async Task<bool> DeleteSelect(IEnumerable<string> userIds)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<IEnumerable<string>>($"api/User/Delete/Select", userIds);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
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
            throw;
        }
    }
}
