using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenHUD : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    public void SetProgress(float progress)
    {
        progressBar.value = progress;
    }
}
