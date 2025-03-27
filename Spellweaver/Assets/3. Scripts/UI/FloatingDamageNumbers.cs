using TMPro;
using UnityEngine;

public class FloatingDamageNumbers : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float floatSpeed = 1.5f;
    public float fadeTime = 0.75f;

    private Color textColor;
    private float elapsedTime;

    public void Initialize(float damage, Color color)
    {
        damageText.text = damage.ToString("F0"); 
        textColor = color;
        damageText.color = textColor;
        Destroy(gameObject, fadeTime); 
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Fade out effect
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
        damageText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
    }
}