public class DraftManager : Singleton<DraftManager>
{
    public void LeaveDraft()
    {
        LobbyManager.Current.LeaveLobby();
        SceneLoader.Current.LoadMenuAsync();
    }
}