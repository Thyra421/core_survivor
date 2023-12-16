using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        EnemyHealth enemyHealth = GetComponentInParent<EnemyHealth>();
        healthSlider.maxValue = enemyHealth.Max;
        healthSlider.BindValue(enemyHealth.Current,
            i => healthSlider.gameObject.SetActive(!(enemyHealth.IsFullHealth || enemyHealth.IsDead)));
    }
}