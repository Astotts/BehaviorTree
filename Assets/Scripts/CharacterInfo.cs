using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    internal Animator animator;

    [Header("Movement")]
    public float speed;
    public float xCenter;
    public float yCenter;

    void Start()
    {   
        //Movement controller is a static class which is why it can be referenced directly
        activeTile = MovementController.GetTileFromPos(this.transform.position);
        MovementController.PositionCharacterOnLine(this, activeTile);
        animator = GetComponent<Animator>();
    }

    //Testing function 
    public void TakeDamage(){
        Debug.Log(this + " Taking Damage");
    }
}

