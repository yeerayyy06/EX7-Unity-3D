using UnityEngine;

public class TextPuntsJugador : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _puntsText;
    private int _puntsInt;

    void Start()
    {
        _puntsText = GetComponent<TMPro.TextMeshProUGUI>();
        _puntsInt = 0;
        ValorsGlobals.videsActuals = 3;
        ValorsGlobals.videsAgafades = 0;
    }

    public void setPuntsJugador(int nousPunts)
    {
        _puntsInt += nousPunts;
        _puntsText.text = "Punts: " + _puntsInt;
        ValorsGlobals.puntsAconseguits = _puntsText.text;
    }

    public int getPuntsJugador() => _puntsInt;

    public void InicialitzarPunts()
    {
        _puntsInt = 0;
        _puntsText.text = "Punts: 0";
    }
}
