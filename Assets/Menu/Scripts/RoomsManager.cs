public class RoomsManager : Singleton<RoomsManager>
{
    public ListenableList<Room> Rooms { get; } = new();

    private void OnDeleteRoom(DeleteRoomMessage message)
    {
        int index = Rooms.FindIndex((r) => r.name == message.name);
        Rooms.RemoveAt(index);
    }

    private void OnCreateRoom(CreateRoomMessage message)
    {
        Room room = new() { name = message.name, networkAddress = message.ip, port = message.port };
        Rooms.Add(room);
    }

    protected void Start()
    {
        MasterServer.Current.AddListener<CreateRoomMessage>(OnCreateRoom);
        MasterServer.Current.AddListener<DeleteRoomMessage>(OnDeleteRoom);
    }
}