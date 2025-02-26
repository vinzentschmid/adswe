using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL;

public class User : Entity
{
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    [BsonIgnore]
    public string Password { get; set; }
    public string HashedPassword { get; set; }
    public Boolean Active { get; set; }
}

public class Aquarium : Entity
{
    public String Name { get; set; }
    [BsonRepresentation(BsonType.String)]
    [EnumDataType(typeof(WaterType))]
    public WaterType WaterType { get; set; }
    public Double Depth { get; set; }
    public Double Height { get; set; }
    public Double Length { get; set; }
    public Double Liters { get; set; }
}

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Animal), typeof(Coral))]
[KnownType(typeof(Animal))]
[KnownType(typeof(Coral))]
public class AquariumItem : Entity
{   
    public String Aquarium { get; set; }
    public String Name { get; set; }
    public String Species { get; set; }
    public DateTime Inserted { get; set; } = DateTime.Now;
    public int Amount { get; set; }
    public String Description { get; set; }
}


public class Coral : AquariumItem
{
    [BsonRepresentation(BsonType.String)]
    [EnumDataType(typeof(CoralType))]
    public CoralType CoralType { get; set; }
}


public enum CoralType
{
    SoftCoral,
    HardCoral,
}

public class Animal : AquariumItem
{
    public DateTime DeathDate { get; set; } = DateTime.MinValue;
    public Boolean IsAlive { get; set; } = true;

}

public class Picture : Entity
{
    public DateTime Uploaded { get; set; }
    public String Aquarium { get; set; }
    public String Description { get; set; }
    public String ContentType { get; set; }
    public String PictureID { get; set; }
}

public class UserAquarium : Entity
{

    public String UserID { get; set; }
    public String AquariumID { get; set; }
    [BsonRepresentation(BsonType.String)]
    [EnumDataType(typeof(UserRole))]
    public UserRole Role { get; set; }

}

public enum UserRole
{
    Admin,
    User
}

public enum WaterType
{
    Saltwater,
    Freshwater
}