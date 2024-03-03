namespace LeitorExcelV3.Models;

public class PlooFieldsModel
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public int? TypeId { get; set; }
    public int? EntityId { get; set; }
    public string? ReceiveExternalKey { get; set; }
    public string? SendExternalKey { get; set; }

    public PlooFieldsModel()
    {
        
    }

    public PlooFieldsModel(string name, int typeId, int entityId, string receiveExternalKey)
    {
        Name = name;
        TypeId = typeId;
        EntityId = entityId;
        ReceiveExternalKey = receiveExternalKey;
    }
}
