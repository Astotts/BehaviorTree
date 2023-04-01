using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnController
{
    public static bool playerTurn = true;

    public static void AiUpdate(){
        playerTurn = false;
    }
}
