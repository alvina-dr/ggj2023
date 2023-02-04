using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    public Vector3Int gridPosition;
    public Vector3Int targetTilePosition;
    public GameObject playerMove;
    public GameObject playerMesh;
    public float playerSpeed;
    Vector2 direction;
    public bool isMoving = false;

    public int railAmmo;


    void Start()
    {
        gridPosition = GPCtrl.Instance.railMap.WorldToCell(new Vector3(playerMove.transform.position.x, playerMove.transform.position.z, 0));
        playerMesh.transform.position = new Vector3(gridPosition.x, playerMesh.transform.position.y, gridPosition.y);
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMesh.transform.position = Vector3.MoveTowards(playerMesh.transform.position, new Vector3(playerMove.transform.position.x, playerMesh.transform.position.y, playerMove.transform.position.z), Time.deltaTime * playerSpeed);
        if (playerMesh.transform.position != new Vector3(playerMove.transform.position.x, playerMesh.transform.position.y, playerMove.transform.position.z)) isMoving = true;
        else isMoving = false;


        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(1, 0);
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(-1, 0);
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(0, 1);
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(0, -1);
            MoveTile(targetTilePosition);
        } else
        {
            targetTilePosition = Vector3Int.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space) && direction != Vector2Int.zero)
        {
            BuildTile(targetTilePosition);
        }
    }

    public void MoveTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (CheckHasRail(new Vector3Int(nextTile.x, -nextTile.y)))
        {
            gridPosition = new Vector3Int(nextTile.x, nextTile.y);
            playerMove.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
        }
    }

    public void BuildTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (!CheckHasRail(new Vector3Int(nextTile.x, -nextTile.y)))
        {
            GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), GPCtrl.Instance.railTile);
            GPCtrl.Instance.UpdateRailList();
            GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1.2f, .3f).OnComplete(() =>
            {
                GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1f, .3f);
            });
        }
    }

    public bool CheckObstacle(Vector3Int _tile)
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

    public bool CheckHasRail(Vector3Int _tile)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.rails.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.rails[i].transform.position) == _tile)
            {
                hasTile = true;
            }
        }
        return hasTile;
    }
}
