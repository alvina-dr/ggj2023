using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UI.Scripts;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    public Vector3Int gridPosition;
    public Vector3Int targetTilePosition;
    public GameObject playerMove;
    public GameObject playerTarget;
    public GameObject playerMesh;
    public float playerSpeed;
    public float playerFastSpeed;
    public float playerSlowSpeed;
    Vector2 direction;
    public bool isMoving = false;

    public int railMax;
    public int railAmount;
    public float railReloadTime;
    bool isLoadingRail;

    public int energyMax;
    public int energyAmount;

    public int repeaterAmount;

    public int currentTool;

    private bool isTurning = false;

    void Start()
    {
        gridPosition = GPCtrl.Instance.interactionMap.WorldToCell(new Vector3(transform.position.x, 0, transform.position.z));
        playerMesh.transform.position = new Vector3(gridPosition.x, playerMesh.transform.position.y, gridPosition.y);
        GPCtrl.Instance.amplificateurUI.CounterTextUpdate(repeaterAmount);
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMesh.transform.position = Vector3.MoveTowards(playerMesh.transform.position, new Vector3(playerMove.transform.position.x, playerMesh.transform.position.y, playerMove.transform.position.z), Time.deltaTime * playerSpeed);
        if (playerMesh.transform.position != new Vector3(playerMove.transform.position.x, playerMesh.transform.position.y, playerMove.transform.position.z)) isMoving = true;
        else isMoving = false;
        playerTarget.transform.position = new Vector3(gridPosition.x + targetTilePosition.x, playerTarget.transform.position.y, gridPosition.y + targetTilePosition.y);
        if (CheckHasActivatedRail(gridPosition)) {
            playerSpeed = playerFastSpeed;
        } else
        {
            playerSpeed = playerSlowSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
        {
            if (targetTilePosition != new Vector3Int(1, 0)) Rotate(Quaternion.Euler(0, -90, 0));
            targetTilePosition = new Vector3Int(1, 0);
            if(!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isMoving)
        {
            if (targetTilePosition != new Vector3Int(-1, 0)) Rotate(Quaternion.Euler(0, 90, 0));
            targetTilePosition = new Vector3Int(-1, 0);
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isMoving)
        {
            if (targetTilePosition != new Vector3Int(0, 1)) Rotate(Quaternion.Euler(0, 180, 0));
            targetTilePosition = new Vector3Int(0, 1);
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isMoving)
        {
            if (targetTilePosition != new Vector3Int(0, -1)) Rotate(Quaternion.Euler(0, 0, 0));
            targetTilePosition = new Vector3Int(0, -1);
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
        }

        if (!isMoving && Input.GetKeyDown(KeyCode.Space))
        {
            InterractWithTile(targetTilePosition);
        } else if (!isMoving && Input.GetKeyDown(KeyCode.Z))
        {
            GetTile(targetTilePosition);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentTool++;
            if (currentTool >= GPCtrl.Instance.tileBaseTools.Count) currentTool = 0;
        }

        if (railAmount < railMax && !isLoadingRail)
        {
            isLoadingRail = true;
            StartCoroutine(RailReload());
        }
    }

    public void MoveTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (CheckTileHasRail(nextTile))
        {
            gridPosition = nextTile;
            playerMove.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
        }
    }

    public void Rotate(Quaternion _rotation)
    {
        if (playerMesh.transform.rotation != _rotation /*&& !isTurning*/)
        {
            isTurning = true;
            playerMesh.transform.DORotateQuaternion(_rotation, .2f).OnComplete(() =>
            {
                isTurning = false;
            });
        }
    }

    public void InterractWithTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (CheckHasObjectType(nextTile)) //if there's something get it
        {
            GetTile(_direction);
        } else // if there's nothing build
        {
            BuildTile(_direction);
        }
    }

    public void BuildTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (!CheckHasObjectType(nextTile) && !CheckTileHasRail(nextTile) && !CheckTileHasObstacle(nextTile)) //try to minus y
        {
            if (currentTool == 0) //rails
            {
                if (railAmount <= 0) return;
                railAmount--;
                GPCtrl.Instance.railMap.SetTile(nextTile, GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateRailList();
                GPCtrl.Instance.UpdateAllRailState();
                GPCtrl.Instance.railList.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == nextTile).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.railList.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == nextTile).transform.DOScale(1f, .3f);
                });
            }
            else if (currentTool == 1) //repeater
            {
                if (repeaterAmount <= 0) return;
                repeaterAmount--;
                GPCtrl.Instance.amplificateurUI.CounterTextUpdate(repeaterAmount);
                GPCtrl.Instance.interactionMap.SetTile(nextTile, GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateObjectList();
                GPCtrl.Instance.UpdateAllRailState();
                GPCtrl.Instance.objectList.Find(x => GPCtrl.Instance.interactionMap.WorldToCell(x.transform.position) == nextTile).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.objectList.Find(x => GPCtrl.Instance.interactionMap.WorldToCell(x.transform.position) == nextTile).transform.DOScale(1f, .3f);
                });
            }
        }
    }

    public void GetTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (CheckHasObjectType(new Vector3Int(nextTile.x, nextTile.y))) 
        {
            switch(GetSpecificObject(nextTile).GetComponent<InteractableObject>().objectType)
            {
                case InteractableObject.ObjectType.Rail:
                    break;
                case InteractableObject.ObjectType.Repeater:
                    Destroy(GetSpecificObject(nextTile).gameObject);
                    GPCtrl.Instance.interactionMap.SetTile(nextTile, null);
                    repeaterAmount++;
                    GPCtrl.Instance.amplificateurUI.CounterTextUpdate(repeaterAmount);
                    GPCtrl.Instance.UpdateObjectList();
                    GPCtrl.Instance.UpdateAllRailState();
                    break;
                case InteractableObject.ObjectType.Crystal:
                    Destroy(GetSpecificObject(nextTile));
                    GPCtrl.Instance.interactionMap.SetTile(nextTile, null);
                    AddEnergy(30);
                    GPCtrl.Instance.UpdateObjectList();
                    break;
                case InteractableObject.ObjectType.Spawner:
                    break;
                case InteractableObject.ObjectType.Artifact:
                    GPCtrl.Instance.WinGame();
                    break;
            }
        } else if (CheckTileHasRail(nextTile))
        {
            Destroy(GetSpecificRail(nextTile).gameObject);
            GPCtrl.Instance.railMap.SetTile(nextTile, null);
            railAmount++;
            GPCtrl.Instance.UpdateRailList();
            GPCtrl.Instance.UpdateAllRailState();
        }
    }

    public bool CheckTileHasRail(Vector3Int _tile)
    {
        if (GPCtrl.Instance.railMap.GetTile(_tile) != null)
        {
            return true;
        }
        else return false;
    }

    public bool CheckTileHasObstacle(Vector3Int _tile)
    {
        if (GPCtrl.Instance.obstacleMap.GetTile(_tile) != null)
        {
            return true;
        }
        else return false;
    }

    public bool CheckHasObjectType(Vector3Int _tile, InteractableObject.ObjectType _objectType = InteractableObject.ObjectType.None)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.objectList.Count; i++)
        {
            if (GPCtrl.Instance.interactionMap.WorldToCell(GPCtrl.Instance.objectList[i].transform.position) == _tile)
            {
                switch(_objectType)
                {
                    case InteractableObject.ObjectType.None:
                        hasTile = true;
                        break;
                    case InteractableObject.ObjectType.Rail:
                    case InteractableObject.ObjectType.Repeater:
                    case InteractableObject.ObjectType.Artifact:
                    case InteractableObject.ObjectType.Crystal:
                        if (GPCtrl.Instance.objectList[i].objectType == _objectType)
                            hasTile = true;
                        break;
                }
            }
        }
        return hasTile;
    }

    public bool CheckHasActivatedRail(Vector3Int _tile)
    {
        bool isActivated = false;
        if(CheckTileHasRail(_tile))
        {
            isActivated = GetSpecificRail(_tile).CheckIfRailActivated();
        }
        return isActivated;
    }

    public IEnumerator RailReload()
    {
        yield return new WaitForSeconds(railReloadTime);
        isLoadingRail = false;
        railAmount++;
    }

    public GameObject GetSpecificObject(Vector3Int _tile)
    {
        for (int i = 0; i < GPCtrl.Instance.objectList.Count; i++)
        {
            if (GPCtrl.Instance.interactionMap.WorldToCell(GPCtrl.Instance.objectList[i].transform.position) == _tile)
            {
                return GPCtrl.Instance.objectList[i].gameObject;
            }

        }
        return null;
    }

    public Rail GetSpecificRail(Vector3Int _tile)
    {
        for (int i = 0; i < GPCtrl.Instance.railList.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.railList[i].transform.position) == _tile)
            {
                return GPCtrl.Instance.railList[i];
            }

        }
        return null;
    }

    public void AddEnergy(int _num)
    {
        energyAmount += _num;
        if (energyAmount >= energyMax)
        {
            energyAmount -= energyMax;
            repeaterAmount++;
            GPCtrl.Instance.amplificateurUI.CounterTextUpdate(repeaterAmount);
        }
        GPCtrl.Instance.amplificateurUI.FillBarUpdate();
    }

}
