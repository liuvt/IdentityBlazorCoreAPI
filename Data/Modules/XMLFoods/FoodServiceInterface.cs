namespace IdentityBlazorCoreAPI.Data.Modules.XMLFoods;
public interface FoodServiceInterface
{
    //Get all
    public Task<IEnumerable<Food>> Gets();

    //Get by id
    public Task<Food> Get(int id);

    //Create
    public Task<Food> Create(Food model);

    //Update
    public Task<Food> Update(Food model);

    //Delete
    public Task<string> Delete(int id);
}