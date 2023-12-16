using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        EnemyHealth enemyHealth = GetComponentInParent<EnemyHealth>();
        healthSlider.BindMaxValue(enemyHealth.max);
        healthSlider.BindValue(enemyHealth.current,
            i => healthSlider.gameObject.SetActive(!(enemyHealth.IsFullHealth || enemyHealth.IsDead)));
    }
}