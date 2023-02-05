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

    private AmplificateurUI _amplificateurScript;
    void Start()
    {
        _amplificateurScript = FindObjectOfType<AmplificateurUI>();
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
            if(!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
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
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
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
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
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
            if (!Input.GetKey(KeyCode.Z)) MoveTile(targetTilePosition);
        } else
        {
            targetTilePosition = Vector3Int.zero;
        }

        if (!isMoving && Input.GetKeyDown(KeyCode.Space) && direction != Vector2Int.zero)
        {
            BuildTile(targetTilePosition);
        }

        if (!isMoving && Input.GetKey(KeyCode.Z) && direction != Vector2Int.zero)
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
        if (CheckHasRail(new Vector3Int(nextTile.x, -nextTile.y)))
        {
            gridPosition = new Vector3Int(nextTile.x, nextTile.y);
            playerMove.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
        }
    }

    public void BuildTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (!CheckHasObjectType(new Vector3Int(nextTile.x, -nextTile.y)))
        {
            if (currentTool == 0) //rails
            {
                if (railAmount <= 0) return;
                railAmount--;
                GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateRailList();
                GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.rails.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1f, .3f);
                });
            } else if (currentTool == 1) //repeater
            {
                if (repeaterAmount <= 0) return;
                repeaterAmount--;
                GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), GPCtrl.Instance.tileBaseTools[currentTool]);
                GPCtrl.Instance.UpdateRepeaterList();
                GPCtrl.Instance.repeaters.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1.2f, .3f).OnComplete(() =>
                {
                    GPCtrl.Instance.repeaters.Find(x => GPCtrl.Instance.railMap.WorldToCell(x.transform.position) == new Vector3Int(nextTile.x, -nextTile.y)).transform.DOScale(1f, .3f);
                });
            }
            GPCtrl.Instance.UpdateAllRailState();
            GPCtrl.Instance.UpdateObjectList();

        }
    }

    public void GetTile(Vector3Int _direction)
    {
        Vector3Int nextTile = gridPosition + _direction;
        if (CheckHasObjectType(new Vector3Int(nextTile.x, -nextTile.y))) 
        {
            switch(GetSpecificObject(new Vector3Int(nextTile.x, -nextTile.y)).GetComponent<InteractableObject>().objectType)
            {
                case InteractableObject.ObjectType.Rail:
                    Destroy(GetSpecificObject(new Vector3Int(nextTile.x, -nextTile.y)));
                    GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), null);
                    railAmount++;
                    GPCtrl.Instance.UpdateRailList();
                    break;
                case InteractableObject.ObjectType.Repeater:
                    Destroy(GetSpecificObject(new Vector3Int(nextTile.x, -nextTile.y)));
                    GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), null);
                    GPCtrl.Instance.UpdateRepeaterList();
                    repeaterAmount++;
                    break;
                case InteractableObject.ObjectType.Crystal:
                    Destroy(GetSpecificObject(new Vector3Int(nextTile.x, -nextTile.y)));
                    GPCtrl.Instance.railMap.SetTile(new Vector3Int(nextTile.x, -nextTile.y), null);
                    AddEnergy(30);
                    break;
                case InteractableObject.ObjectType.Spawner:
                    break;
            }
            GPCtrl.Instance.UpdateObjectList();
        }
    }

    public bool CheckHasObjectType(Vector3Int _tile, InteractableObject.ObjectType _objectType = InteractableObject.ObjectType.None)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.objectList.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.objectList[i].transform.position) == _tile)
            {
                if (_objectType == InteractableObject.ObjectType.None)
                {
                    hasTile = true;
                } else if (GPCtrl.Instance.rails[i].objectType == _objectType)
                {
                    hasTile = true;
                }
            }
        }
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

    public bool CheckHasRepeater(Vector3Int _tile)
    {
        bool hasTile = false;
        for (int i = 0; i < GPCtrl.Instance.repeaters.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.repeaters[i].transform.position) == _tile)
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
        railAmount++;
    }

    public GameObject GetSpecificObject(Vector3Int _tile)
    {
        for (int i = 0; i < GPCtrl.Instance.objectList.Count; i++)
        {
            if (GPCtrl.Instance.railMap.WorldToCell(GPCtrl.Instance.objectList[i].transform.position) == _tile)
            {
                return GPCtrl.Instance.objectList[i].gameObject;
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
            _amplificateurScript.CounterTextUpdate();
        }
        _amplificateurScript.FillBarUpdate();
    }

}
