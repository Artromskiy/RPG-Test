using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)] // This attribute forces user to specify serialized data with JsonProprtyAttribute
public interface IConfig { }
