using Newtonsoft.Json;

public class PlayerSkill : Keyd<int>
{
    [JsonProperty]
    public readonly int id;
    [JsonProperty]
    public readonly int price;

    public override int Key => id;


    public PlayerSkill(int id, int price)
    {
        this.id = id;
        this.price = price;
    }
    public override string ToString()
    {
        return $"id: {id}, price: {price}";
    }
}
