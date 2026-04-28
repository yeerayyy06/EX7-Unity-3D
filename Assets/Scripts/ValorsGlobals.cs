using UnityEngine;

public static class ValorsGlobals
{
    public static string puntsAconseguits = "Punts: 0";
    public static int videsAgafades;
    public static int videsActuals = 3;

    public static void Reiniciar()
    {
        puntsAconseguits = "Punts: 0";
        videsAgafades = 0;
        videsActuals = 3;
    }
}
