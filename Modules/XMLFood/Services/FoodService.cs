using System.Xml.Linq;
using IdentityBlazorCoreAPI.Modules.Models;

namespace IdentityBlazorCoreAPI.Modules.XMLFoods.Services;

public class FoodService : IFoodService
{
    //Connection String to XML
    private readonly XElement context;
    //Constructor
    public FoodService()
    {
        context = XElement.Load(@"wwwroot/data/xmlbase.xml");
    }

    public async Task<Food> Create(Food model)
    {
        //Create new food
        var newFood =
            new XElement("foods",
                new XElement("food",
                    new XAttribute("food_id",model.food_id),
                        new XElement("name", model.food_name),
                        new XElement("price", model.food_price),
                        new XElement("description", model.food_description),
                        new XElement("calories", model.food_calories),
                        new XElement("img", model.food_img)
                )
        );

        context.Add(newFood);
        context.Save(@"wwwroot/data/xmlbase.xml"); 
        
        return await this.Get(model.food_id);
    }

    public async Task<string> Delete(int id)
    {
        //select object to delete
        var food = (from f in context.Descendants("foods").Descendants("food")
                        where (int)f.Attribute("food_id") == id
                        select f).FirstOrDefault();
        //remove object
        food.Remove();

        //Save file xml;
        context.Save(@"wwwroot/data/xmlbase.xml");

        return "DONE!";
    }

    public async Task<Food> Get(int id)
    {
        var food = (from item in context.Descendants("foods").Descendants("food")
                        where (int)item.Attribute("food_id") == id
                            select (new Food{
                                food_id = (int)item.Attribute("food_id"), 
                                food_name = (string)item.Element("name"),
                                food_price = (string)item.Element("price"),
                                food_description = (string)item.Element("description"),
                                food_calories = (string)item.Element("calories"),
                                food_img = (string)item.Element("img")
                            })).FirstOrDefault();
        return food;
    }

    public async Task<IEnumerable<Food>> Gets()
    {
        //Select data from file xml with LinQ
        var foods = (from item in context.Descendants("foods").Descendants("food")
                                select (new Food{
                                    food_id = (int)item.Attribute("food_id"), //XML Attributes for Metadata
                                    food_name = (string)item.Element("name"),
                                    food_price = (string)item.Element("price"),
                                    food_description = (string)item.Element("description"),
                                    food_calories = (string)item.Element("calories"),
                                    food_img = (string)item.Element("img")
                                })).ToList();
        return foods;
    }

    public async Task<Food> Update(Food model)
    {
        //Select object to update/edit
        var food = (from f in context.Descendants("foods").Descendants("food")
                        where (int)f.Attribute("food_id") == model.food_id
                        select f).FirstOrDefault();
        
        //Update object
        food.Element("name").Value = model.food_name;
        food.Element("price").Value = model.food_price;
        food.Element("description").Value = model.food_description;
        food.Element("calories").Value = model.food_calories;
        food.Element("img").Value = model.food_img;

        //Save new object
        context.Save(@"wwwroot/data/xmlbase.xml");

        return await this.Get(model.food_id);
    }
}