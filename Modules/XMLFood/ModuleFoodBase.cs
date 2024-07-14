using IdentityBlazorCoreAPI.Modules.XMLFoods;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace IdentityBlazorCoreAPI.Modules.XMLFoods;
public class ModuleFoodBase : ComponentBase
{
    [Inject]
    protected IFoodService foodService { get; set; }
    protected IEnumerable<Food> listFoods { get; set; }
    protected Food _food { get; set; }



    protected override async Task OnInitializedAsync()
    {
        //Get all Food data
        listFoods = await foodService.Gets();

        //Get Food by Id frist
        _food = await foodService.Get(1);
    }

    protected async void onClickDeleteFood(int foodid)
    {
        var mess = await foodService.Delete(foodid);
        Console.WriteLine("Delete food with ID{" + foodid + "}: " + mess);
    }

    protected async void onClickFood(int foodid)
    {
        _food = await foodService.Get(foodid);
    }

    protected Food model = new Food();
    protected async void OnValidSubmit(EditContext editContext)
    {
        //Do something
        await foodService.Create(model);
        StateHasChanged();
    }

    protected async void OnValidSubmit_Edit(EditContext editContext)
    {
        //Do something
        await foodService.Update(_food);
        StateHasChanged();
    }
}