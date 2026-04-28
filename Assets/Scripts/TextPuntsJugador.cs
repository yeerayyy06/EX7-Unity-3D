using UnityEngine;

public class TextPuntsJugador : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _puntsText;
    private int _puntsInt;

    // Lazy getter: funciona fins i tot si l'objecte estava inactiu quan Start() no es va cridar
    private TMPro.TextMeshProUGUI Text
    {
        get
        {
            if (_puntsText == null)
                _puntsText = GetComponent<TMPro.TextMeshProUGUI>();
            return _puntsText;
        }
    }

    void Start()
    {
        _puntsInt = 0;
    }

    public void setPuntsJugador(int nousPunts)
    {
        _puntsInt += nousPunts;
        Text.text = "Punts: " + _puntsInt;
        ValorsGlobals.puntsAconseguits = Text.text;
    }

    public int getPuntsJugador() => _puntsInt;

    public void InicialitzarPunts()
    {
        _puntsInt = 0;
        Text.text = "Punts: 0";
        ValorsGlobals.puntsAconseguits = "Punts: 0";
    }
}
