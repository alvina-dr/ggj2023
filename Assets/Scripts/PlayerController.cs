using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    public Vector3Int gridPosition;
    public Vector3Int targetTilePosition;
    public GameObject playerMove;
    public GameObject playerMesh;
    public float playerSpeed;
    public float playerFastSpeed;
    public float playerSlowSpeed;
    Vector2 direction;
    public bool isMoving = false;

    public int railAmmoMax;
    public int railAmmo;
    public float railReloadTime;
    bool isLoadingRail;

    public int currentTool;

    private bool isTurning = false;


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
        if (CheckHasActivatedRail(new Vector3Int(gridPosition.x, -gridPosition.y))) {
            playerSpeed = playerFastSpeed;
        } else
        {
            playerSpeed = playerSlowSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(1, 0);
            if (playerMesh.transform.rotation != Quaternion.Euler(0, -90, 0) && !isTurning)
            {
                isTurning = true;
                playerMesh.transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), .2f).OnComplete(() =>
                {
                    isTurning = false;
                });
            }
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(-1, 0);
            if (playerMesh.transform.rotation != Quaternion.Euler(0, 90, 0) && !isTurning)
            {
                isTurning = true;
                playerMesh.transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), .2f).OnComplete(() =>
                {
                    isTurning = false;
                });
            }
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(0, 1);
            if (playerMesh.transform.rotation != Quaternion.Euler(0, 180, 0) && !isTurning)
            {
                isTurning = true;
                playerMesh.transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), .2f).OnComplete(() =>
                {
                    isTurning = false;
                });
            }
            MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isMoving)
        {
            targetTilePosition = new Vector3Int(0, -1);
            if (playerMesh.transform.rotation != Quaternion.Euler(0, 0, 0) && !isTurning)
            {
                isTurning = true;
                playerMesh.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), .2f).OnComplete(() =>
                {
                    isTurning = false;
                });
            }
            MoveTile(targetTilePosition);
        } else
        {
            targetTilePosition = Vector3Int.zero;
        }

        if (!isMoving && Input.GetKeyDown(KeyCode.Space) && direction != Vector2Int.zero)
        {
            BuildTile(targetTilePosition);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentTool++;
            if (currentTool >= GPCtrl.Instance.tileBaseTools.Count) currentTool = 0;
        }

        if (railAmmo < railAmmoMax && !isLoadingRail)
        {
            isLoadingRail = true;
            StartCoroutine(RailReload());
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
        if (!CheckHasObstacle(new Vector3Int(nextTile.x, -nextTile.y)))
        {
            if (currentTool == 0) //rails
            {
                if (railAmmo <= 0) return;
                railAmmo--;
                GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateRailList();
                GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1f, .3f);
                });
            } else if (currentTool == 1) //repeater
            {
                GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateRepeaterList();
                GPCtrl.Instance.repeaters.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.repeaters.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1f, .3f);
                });
            }
            GPCtrl.Instance.UpdateAllRailState();

        }
    }

    public bool CheckHasObstacle(Vector3Int _tile)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.rails.Count; i++)
        {
            if (GPCtrl.Instance.rails[i].transform.position == _tile)
            {
                hasTile = true;
            }
        }
        for (int i = 0; i < GPCtrl.Instance.repeaters.Count; i++)
        {
            if (GPCtrl.Instance.repeaters[i].transform.position == _tile)
            {
                hasTile = true;
            }
        }
        if (hasTile == true) Debug.Log("CAN'T BUILD TILE");
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

    public bool CheckHasActivatedRail(Vector3Int _tile)
    {
        bool isActivated = false;
        for (int i = 0; i < GPCtrl.Instance.rails.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.rails[i].transform.position) == _tile)
            {
                if (GPCtrl.Instance.rails[i].isActivated)
                {
                    isActivated = true;
                }
            }
        }
        return isActivated;
    }

    public IEnumerator RailReload()
    {
        yield return new WaitForSeconds(railReloadTime);
        isLoadingRail = false;
        railAmmo++;
    }

}
