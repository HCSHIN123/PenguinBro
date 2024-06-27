using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkManager : MonoBehaviour
{
    [SerializeField] private GameObject penguin1;
    [SerializeField] private GameObject penguin2;
    [SerializeField] private Transform[] penguin1Path; // penguin1의 이동 경로 배열
    [SerializeField] private Transform[] penguin2Path; // penguin2의 이동 경로 배열

    private Vector3 penguinOriginalPosition1;
    private Quaternion penguinOriginalRotation1;
    private Vector3 penguinOriginalPosition2;
    private Quaternion penguinOriginalRotation2;

    private void Start()
    {
        if (penguin1 != null)
        {
            // 펭귄1의 초기 위치와 회전을 저장합니다.
            penguinOriginalPosition1 = penguin1.transform.position;
            penguinOriginalRotation1 = penguin1.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Penguin1 is not assigned in the inspector.");
        }

        if (penguin2 != null)
        {
            // 펭귄2의 초기 위치와 회전을 저장합니다.
            penguinOriginalPosition2 = penguin2.transform.position;
            penguinOriginalRotation2 = penguin2.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Penguin2 is not assigned in the inspector.");
        }

        // 펭귄1과 펭귄2 이동 시작
        StartCoroutine(MovePenguin(penguin1, penguin1Path, penguinOriginalPosition1, penguinOriginalRotation1, new float[] { 6, -90, 180 }));
        StartCoroutine(MovePenguin(penguin2, penguin2Path, penguinOriginalPosition2, penguinOriginalRotation2, new float[] { -6, 90, -180 }));
    }

    private IEnumerator MovePenguin(GameObject penguin, Transform[] path, Vector3 originalPosition, Quaternion originalRotation, float[] rotationAngles)
    {
        while (true)
        {
            if (penguin != null && path != null && path.Length > 0)
            {
                // 경로를 순회
                for (int i = 0; i < path.Length; i++)
                {
                    Transform target = path[i];

                    // 목표 지점으로 이동
                    yield return StartCoroutine(MoveToPosition(penguin.transform, target.position));

                    // 목표 지점에 따른 회전
                    if (i < rotationAngles.Length)
                    {
                        yield return StartCoroutine(RotateToAngle(penguin.transform, Quaternion.Euler(0, rotationAngles[i], 0)));
                    }

                    // 잠시 대기
                    yield return new WaitForSeconds(1.0f);
                }

                // 초기 위치로 이동
                yield return StartCoroutine(MoveToPosition(penguin.transform, originalPosition));
                // 초기 회전 상태로 회전
                yield return StartCoroutine(RotateToAngle(penguin.transform, originalRotation));

                // 잠시 대기
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                Debug.LogWarning("Penguin or move positions are not assigned correctly.");
                yield break;
            }
        }
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 position)
    {
        float time = 0;
        float duration = 2.0f; // 이동 시간
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = position;
    }

    private IEnumerator RotateToAngle(Transform transform, Quaternion targetRotation)
    {
        float time = 0;
        float duration = 2.0f;
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
