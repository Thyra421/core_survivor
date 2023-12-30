using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        CharacterHealth characterHealth = GetComponentInParent<CharacterHealth>();
        
        healthSlider.maxValue = characterHealth.Max;
        healthSlider.BindValue(characterHealth.Current,
            i => healthSlider.gameObject.SetActive(!(characterHealth.IsFullHealth || characterHealth.IsDead)));
    }
}