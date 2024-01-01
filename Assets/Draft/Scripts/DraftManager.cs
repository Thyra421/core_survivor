using Mirror;

public class DraftManager : Singleton<DraftManager>
{
    public void LeaveDraft()
    {
        ((Network)NetworkManager.singleton).Stop();
        LobbyManager.Current.LeaveLobby();
        SceneLoader.Current.LoadMenuAsync();
    }
}