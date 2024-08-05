using IdentityBlazorCoreAPI.Data.Models;
using IdentityBlazorCoreAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace IdentityBlazorCoreAPI.Pages.Pricings;

public class PricingBase : ComponentBase
{
    [Inject] private ISnackbar snackBar { get; set; } //Thông báo
    [Inject] private IEtherealEmailService etherealEmailService {get; set; }
    protected string BUY_BASIC = "Cơ bản";
    protected string BUY_MEDIUM = "Trung bình";
    protected string BUY_ADVANCED = "Nâng cao";
    protected override async Task OnInitializedAsync()
    {
        try
        {
           
        }
        catch (Exception ex)
        {

        }
    }

    #region Gọi Form mua Dialog
    [Inject] private IDialogService DialogService { get; set; }
    protected async Task BuyDialog(string price)
    {
        var parameters = new DialogParameters { ["price"] = price };
        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraExtraLarge, CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<PricingBuyDialog>("",parameters, options);
        var result = await dialog.Result;
        try
        {
            if (!result.Canceled)
            {
                var data = (EtherealEmail)result.Data;

                // Format lại dữ liệu để gửi mail
                data.Body = $"Họ tên người mua: {data.Body}<br/>Gói: {price}";
                data.Subject = $"Mua gói: {data.Subject}";
                
                var sendMail = await this.etherealEmailService.Send(data);
                if (sendMail) snackBar.Add("Đặt mua thành công.", Severity.Success);
                else snackBar.Add("Đăt mua không thành công.", Severity.Error);
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            snackBar.Add(ex.Message, Severity.Error);
        }

    }
    #endregion
}
