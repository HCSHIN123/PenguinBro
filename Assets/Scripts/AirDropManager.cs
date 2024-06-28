using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropManager : MonoBehaviour
{
    [SerializeField] private GameObject airDropPlane;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private Transform planeMoveTransform;
    [SerializeField] private Transform planeDropTransform;

    [SerializeField] private Transform[] airDropTransforms;

    private bool hasDropped = false;
    private bool isMovingToDrop = false;

    void Start()
    {
        airDropPlane.SetActive(false);
        Invoke("ActivatePlane", 5f);
    }

    void Update()
    {
        if (airDropPlane.activeSelf)
        {
            if (!isMovingToDrop)
            {
                MovePlane(planeMoveTransform);
            }
            else
            {
                MovePlane(planeDropTransform);
            }
            CheckDropCondition();
        }
    }

    private void ActivatePlane()
    {
        airDropPlane.SetActive(true);
    }

    private void MovePlane(Transform targetTransform)
    {
        float moveSpeed = 250f;
        airDropPlane.transform.position = Vector3.MoveTowards(airDropPlane.transform.position, targetTransform.position, Time.deltaTime * moveSpeed);
    }

    private void CheckDropCondition()
    {
        float distanceToDropTransform = Vector3.Distance(airDropPlane.transform.position, planeDropTransform.position);

        if (!hasDropped && distanceToDropTransform <= 300f)
        {
            DropItem();
            hasDropped = true;
        }
    }

    private void DropItem()
    {
        int randomIndex = Random.Range(0, airDropTransforms.Length);
        Transform selectedDropTransform = airDropTransforms[randomIndex];
        Vector3 dropPosition = new Vector3(selectedDropTransform.position.x, selectedDropTransform.position.y + 50f, selectedDropTransform.position.z); // ��� ��ġ�� ���ʿ� ��ġ
        GameObject droppedItem = Instantiate(dropItem, dropPosition, Quaternion.identity);

        // ��� �� �ٽ� �̵��ϵ��� ����
        isMovingToDrop = false;
        hasDropped = false;
    }
}
