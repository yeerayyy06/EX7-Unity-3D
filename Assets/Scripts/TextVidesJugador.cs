using UnityEngine;

public class TextVidesJugador : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _videsText;

    // Lazy getter: funciona fins i tot si l'objecte estava inactiu quan Start() no es va cridar
    private TMPro.TextMeshProUGUI Text
    {
        get
        {
            if (_videsText == null)
                _videsText = GetComponent<TMPro.TextMeshProUGUI>();
            return _videsText;
        }
    }

    public void ActualitzarText()
    {
        Text.text = "Vides: " + ValorsGlobals.videsActuals;
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
