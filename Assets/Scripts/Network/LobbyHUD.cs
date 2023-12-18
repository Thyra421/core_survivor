using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHUD : MonoBehaviour
{
    [SerializeField] private RectTransform gamesContentRoot;
    [SerializeField] private GameObject gameSelectionEntryPrefab;
    [SerializeField] private Button createGameButton;
    [SerializeField] private TMP_InputField createGameNameInputField;

    private void OnGameInstancesChanged(ListenableList<GameInstance> instances)
    {
        Debug.Log(string.Join(",", instances.Select(e => e.name)));
        Debug.Log(gamesContentRoot.childCount);
        for (int i = gamesContentRoot.childCount - 1; i >= 0; i--)
            Destroy(gamesContentRoot.GetChild(i).gameObject);
        Debug.Log(gamesContentRoot.childCount);

        foreach (GameInstance i in instances)
        {
            GameObject newEntry = Instantiate(gameSelectionEntryPrefab, gamesContentRoot);
            GameSelectionEntryHUD hud = newEntry.GetComponent<GameSelectionEntryHUD>();

            hud.Initialize(i);
        }
    }

    private void Start()
    {
        GameInstanceManager.Current.GameInstances.OnChanged += (e)=>
        {
            OnGameInstancesChanged(e);
            Debug.Log("OnGameInstancesChanged");
        };
        OnGameInstancesChanged(GameInstanceManager.Current.GameInstances);
        createGameButton.onClick.AddListener(() =>
            MasterServerHelper.StartDedicatedServerInstance(createGameNameInputField.text));
    }
}