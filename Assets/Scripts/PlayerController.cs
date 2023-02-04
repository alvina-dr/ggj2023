using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector2Int gridPosition;
    public GameObject playerMove;
    public GameObject playerMesh;
    public float playerSpeed;
    Vector2 direction;
    public bool isMoving = false;


    void Start()
    {
        gridPosition = new Vector2Int(Mathf.RoundToInt(playerMove.transform.position.x), Mathf.RoundToInt(playerMove.transform.position.z));
        playerMesh.transform.position = new Vector3(gridPosition.x, playerMesh.transform.position.y, gridPosition.y);
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMesh.transform.position = Vector3.MoveTowards(playerMesh.transform.position, new Vector3(playerMove.transform.position.x +.5f, playerMesh.transform.position.y, playerMove.transform.position.z + .5f), Time.deltaTime * playerSpeed);
        if (playerMesh.transform.position != new Vector3(playerMove.transform.position.x + .5f, playerMesh.transform.position.y, playerMove.transform.position.z + .5f)) isMoving = true;
        else isMoving = false;


        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
            MoveTile(new Vector2Int(1, 0));
        else if (Input.GetKey(KeyCode.LeftArrow) && !isMoving)
            MoveTile(new Vector2Int(-1, 0));
        else if (Input.GetKey(KeyCode.UpArrow) && !isMoving)
            MoveTile(new Vector2Int(0, 1));
        else if (Input.GetKey(KeyCode.DownArrow) && !isMoving)
            MoveTile(new Vector2Int(0, -1));

        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    Debug.Log("space appuyé");
        //    gridPosition = new Vector2Int(gridPosition.x, gridPosition.y + 1);
        //    playerMove.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);

        //}
    }

    public void MoveTile(Vector2Int _direction)
    {
        Vector2Int nextTile = gridPosition + new Vector2Int(_direction.x, _direction.y);
        if (CheckHasRail(nextTile))
        {
            gridPosition = nextTile;
            playerMove.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
            //direction = _direction;
        }
    }

    public bool CheckObstacle(Vector2Int _tile)
    {
        bool hasTile = false;
        //for (int i = 0; i < manager.obstacleTilemaps.Count; i++)
        //{
        //    if (manager.obstacleTilemaps[i].HasTile(_tile))
        //        hasTile = true;
        //}
        //for (int i = 0; i < manager.objects.Count; i++)
        //{
        //    if (Vector3Int.FloorToInt(manager.objects[i].transform.position - manager.offset) == _tile && manager.objects[i].isObstacle)
        //    {
        //        hasTile = true;
        //    }
        //}

        return hasTile;
    }

    public bool CheckHasRail(Vector2Int _tile)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.rails.Count; i++)
        {
            if (Mathf.RoundToInt(GPCtrl.Instance.rails[i].transform.position.x-.5f) == _tile.x && Mathf.RoundToInt(GPCtrl.Instance.rails[i].transform.position.z-.5f) == _tile.y)
            {
                hasTile = true;
            }
        }
        return hasTile;
    }
}
