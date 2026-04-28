using UnityEngine;

public class TextVidesJugador : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _videsText;

    void Start()
    {
        _videsText = GetComponent<TMPro.TextMeshProUGUI>();
        ActualitzarText();
    }

    public void ActualitzarText()
    {
        _videsText.text = "Vides: " + ValorsGlobals.videsActuals;
    }

    public void AfegirVida()
    {
        ValorsGlobals.videsActuals++;
        ValorsGlobals.videsAgafades++;
        ActualitzarText();
    }

    public void TreureVida()
    {
        ValorsGlobals.videsActuals--;
        ActualitzarText();
    }

    public void InicialitzarVides()
    {
        ValorsGlobals.videsActuals = 3;
        ActualitzarText();
    }
}
