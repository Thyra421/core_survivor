    using TMPro;
    using UnityEngine;

    public class PlayersHUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text playersAliveText;

        private void GameStarted()
        {
            playersAliveText.Bind(PlayerManager.Current.PlayersAlive, value=>$"{value} joueurs en vie");
        }
    }
