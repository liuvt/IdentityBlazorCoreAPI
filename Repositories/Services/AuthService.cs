using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using IdentityBlazorCoreAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using IdentityBlazorCoreAPI.Data.Entities;
using System.Net.Http.Headers;
using IdentityBlazorCoreAPI.Data.Models;
using System.Text.Json;
using System.Net;

namespace IdentityBlazorCoreAPI.Repositories.Services;

public class AuthService : AuthenticationStateProvider, IAuthService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly HttpClient httpClient;
    //JavaScript
    private readonly IJSRuntime jS;
    //Key localStorage
    private string key = "_IdentityBlazorCoreAPI";
    //Anonymous authentication state
    private AuthenticationState Anonymous =>
        new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));

    //Constructor
    public AuthService(IHttpClientFactory _httpClientFactory, HttpClient _httpClient, IJSRuntime _jS)
    {
        this.httpClientFactory = _httpClientFactory;
        this.httpClient = _httpClient;
        this.jS = _jS;
    }

    public async Task<AppUser> GetMe()
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Auth/Me");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(AppUser);
                }

                return await response.Content.ReadFromJsonAsync<AppUser>();
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }

    public async Task<bool> DeleteMe()
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/Auth/Me");

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
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> EditMe(AppEditDTO models)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/Auth/Me/Edit", models);
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
                //var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Reponse400EditMe>(mess); //=> convert JSON to Object
                //var errs = Newtonsoft.Json.JsonConvert.SerializeObject(result.errors);  //=> convert Object to JSON
                throw new Exception(mess);
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }

    public async Task<bool> ChangePassword(AppChangePasswordDTO changePassword)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/Auth/Me/ChangePassword/", changePassword);
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
        catch (System.Exception ex)
        {

            throw;
        }
    }

    public async Task<bool> Register(AppRegisterDTO models)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<AppRegisterDTO>("api/Auth/register", models);

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

    /*
        Login
        - Get API Login Controller by httpClientFactory
        - Set Token to LocalStorage
        - Call BuildAuthenticationState(token) to check state login
    */
    public async Task Login(AppLoginDTO models)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<AppLoginDTO>($"api/Auth/Login", models);

            if (response.IsSuccessStatusCode)
            {
                //Lấy token từ API đăng nhập
                var token = await response.Content.ReadAsStringAsync();

                //Lưu token vào localStorage
                await jS.SetFromLocalStorage(key, token);

                //Kiểm tra trạng thái xác thực
                var state = BuildAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (System.Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    /*
        Log Out
        - Remove token in localstorage
        - BuildAuthenticationState check state
    */
    public async Task LogOut()
    {
        try
        {
            await jS.RemoveFromLocalStorage(key);

            //Kiểm tra trạng thái sau khi đăng nhập
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }
        catch (System.Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    /* Kiểm tra trạng thái đăng nhập trả về True or false */
    public async Task<bool> CheckAuthenState()
    {
        try
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Identity.IsAuthenticated;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /* Xem trạng thái đăng nhập của User */
    public async Task<AuthenticationState> GetAuthenState() => await GetAuthenticationStateAsync();


    #region Authentication State
    /*
        Authentication
        - Get token in localStorage by key
        - Check token by ValidationToken(): bool
        - return BuildAuthenticationState(token)
    */
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //Lấy token từ LocalStorage
        var token = await jS.GetFromLocalStorage(key);

        //Kiểm tra xem token 
        if (!ValidateToken(token))
        {
            return Anonymous;
        }

        //Build AuthenticationState
        return BuildAuthenticationState(token);
    }

    /*
        Build authentication state
        - Check authorization by token
        - Create ParseClaimsFromJwt get claims
        - Get Notify authentication state
        - return authenticationstate
    */
    private AuthenticationState BuildAuthenticationState(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    //Parse Claims From Jwt
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    //Parse Base64 Without Padding
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    //Validate
    private bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        //Check can read token
        var handler = new JwtSecurityTokenHandler();
        bool readToken = handler.CanReadToken(token.Replace("\"", ""));
        return readToken;
    }
    #endregion

}
