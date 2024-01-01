using Mirror;

public class DraftManager : Singleton<DraftManager>
{
    public void LeaveDraft()
    {
        LobbyManager.Current.LeaveLobby();
        ((Network)NetworkManager.singleton).Stop();
    }
}