using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace finished2
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField] GameObject cursor;
        [SerializeField] SpriteRenderer cursorSprite;
        private RaycastHit2D enemy;

        [Header("========Character Info========")]
        public GameObject characterPrefab;
        [SerializeField] private CharacterInfo character;

        [Header("========Movement========")]
        
        public bool isMoving = false;

        public PathFinder pathFinder;
        public List<OverlayTile> path;

        [Header("========Combat========")]
        //IMPORTANT: Whenever you add or remove characters from a scene please add them to this list
        [SerializeField] List<CharacterInfo> characters;
        private CharacterInfo selectedEnemy;

        private void Start()
        {
            pathFinder = new PathFinder();
            path = new List<OverlayTile>();
        }

        void Update(){
            if (path.Count > 0)
            {
                MovementController.MoveAlongPath(character, path);
            } else
            {
                isMoving = false;
                if (character != null && character.animator != null){
                    character.animator.SetBool("isMoving", false);
                } 
            }
        }

        void LateUpdate()
        {
            if(TurnController.playerTurn == true){
                RaycastHit2D? hit = GetFocusedOnTile();

                if (hit.HasValue && hit.Value.collider.gameObject.GetComponent<OverlayTile>() != null)
                {
                    OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                    cursor.transform.position = tile.transform.position;
                    
                    //Finds enemies on selected tiles
                    for(int i = 0; i < characters.Count; i++){
                        if(characters[i].activeTile == tile && characters[i].gameObject.tag != "Player"){
                            //Debug.Log(enemy.collider);
                            selectedEnemy = characters[i];
                            cursorSprite.color = Color.red;
                            break;
                        }
                        else{
                            selectedEnemy = null;
                        }
                    }
                    if(selectedEnemy == null){
                    cursorSprite.color = Color.yellow; 
                    }
                    else{
                        if (Input.GetMouseButtonDown(0) && isMoving == false){
                            //Do a range check and LOS check (Recommend creating a seperate CombatManager script)
                            //LOS: https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague
                            //Assumes just one member for each team, will need to adjust when multiple characters are present
                            //TurnController.AiUpdate();
                            selectedEnemy.TakeDamage();
                            return;
                        }
                    }

                    
                
                
                    cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
                    if (Input.GetMouseButtonDown(0) && isMoving == false)
                    {
                        tile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                        if (character == null)
                        {
                            character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                            MovementController.PositionCharacterOnLine(character, tile);
                            character.activeTile = tile;
                        }
                        else
                        {
                            path = pathFinder.FindPath(character.activeTile, tile);

                            tile.gameObject.GetComponent<OverlayTile>().HideTile();

                            isMoving = true;
                            if (character != null) character.animator.SetBool("isMoving", true);
                        }
                        //TurnController.AiUpdate();
                    }
                }
            }
        }
            

        //Allows the mouse to recognize tiles
        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                //Debug.Log("Length " + hits.Length);
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
}