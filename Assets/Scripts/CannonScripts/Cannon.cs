using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{


    public delegate void CallbackMethod();
    private CallbackMethod callback = null;
    public Bullet[] bullets;
    private Bullet bullet = null;
    public Transform start, peak, end;

    private FirePort firePort;
    private LineRenderer lr;
    List<Vector3> bulletPathList = new List<Vector3>();

    private const float gravity = -9.81f;
    public float initialVelocity = 10f; // �ʱ� �ӵ�
    public float launchAngle = 45f; // �߻� ���� (y�� ����)
    public float directionAngle = 0f; // �߻� ���� (y�� ��鿡���� ����)
    public float desiredDistance = 20f; // ���ϴ� ���� �Ÿ�
    public float bulletVelocity = 120f;

    //����ȭ�� ����
    private Vector3 prevPortDir = Vector3.zero;
    private Vector3 prevPos = Vector3.zero; 

    public bool isMyTurn = false;
    private void Start()
    {
        firePort = start.GetComponent<FirePort>();
        lr = GetComponent<LineRenderer>();

        foreach(Bullet bullet in bullets)
        {
            bullet.transform.SetParent(transform, false);
            bullet.gameObject.SetActive(false);
        }
        AttachBullet(0);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            AttachBullet(0);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            AttachBullet(1);


        launchAngle = firePort.transform.eulerAngles.x;
        // 360�� �̻� ���� ������ �ʵ��� ����
        if (launchAngle >= 360f)
            launchAngle %= 360f;
            
        // 180�� �̸� ������ ��ȯ
        if (launchAngle > 180f)
            launchAngle = 360f - launchAngle;

        directionAngle = transform.eulerAngles.y;
        UpdateBulletPath();
       
        // if (Input.GetMouseButtonUp(0))
        // {
        //     if(bullet?.gameObject.activeSelf == true)
        //         bullet.Shooting_Physical(bulletPathList.ToArray());
        // }
            //bullet.Shooting(start, peak, end);
    }
    public void SetCallbackMethod(CallbackMethod _methodValue)
    {
        callback = _methodValue;
    }

    public void StartShooting()
    {
        isMyTurn = true;
        
        StartCoroutine(ShootBullet());
        
    }

    private IEnumerator ShootBullet()
    {
        if(isMyTurn && bullet?.gameObject.activeSelf == true)
        {
            bullet.Shooting_Physical(bulletPathList.ToArray());            
        }
        yield return new WaitForSeconds(3.0f);
        callback?.Invoke();
        yield break;
    }
   

    private void AttachBullet(int _idx)
    {
        bullet?.gameObject.SetActive(false);
        bullet = bullets[_idx];
        bullet.gameObject.SetActive(true);
    }

    private void UpdateBulletPath()
    {
        // Bezier Curve ���
        //peak.position = start.position + curShootingeDir * bullet.Range;

        //float dis = Vector3.Distance(new Vector3(peak.position.x, 0f, peak.position.z), new Vector3(start.position.x, 0f, start.position.z));
        //Vector3 dir = new Vector3(peak.position.x, 0f, peak.position.z) - new Vector3(start.position.x, 0f, start.position.z);
        //dir.Normalize();
        //end.position = start.position + dir * dis * 2f;
        //bulletPathList.Clear();

        //if (start != null && peak != null && end != null)
        //{
        //    for (int i = 0; i < lineDetail; i++)
        //    {
        //        float t = (i / lineDetail);
        //        Vector3 p4 = Vector3.Lerp(start.position, peak.position, t);
        //        Vector3 p5 = Vector3.Lerp(peak.position, end.position, t);
        //        bulletPathList.Add(Vector3.Lerp(p4, p5, t));
        //    }
        //}
        //lr.SetPositions(bulletPathList.ToArray());

        
        bulletPathList.Clear();
        AdjustInitialVelocityForDistance();
        float angleUpDown = launchAngle * Mathf.Deg2Rad; // ���ϰ���
        float angleLeftRight = directionAngle * Mathf.Deg2Rad; // �¿찢��

        Vector3 initialPosition = start.position;

        // �¿���� ���� ��� (y�� �߽�) x = sin(theta), z = cos(theta)
        Vector3 direction = new Vector3(Mathf.Sin(angleLeftRight), 0, Mathf.Cos(angleLeftRight));

        // �ʱ� �ӵ� ���� ���
        Vector3 initialVelocityVector = new Vector3(
            direction.x * initialVelocity * Mathf.Cos(angleUpDown),
            initialVelocity * Mathf.Sin(angleUpDown),
            direction.z * initialVelocity * Mathf.Cos(angleUpDown)
        );

        // ���� ���� �ʱ� �ӵ� ���� ���
        initialVelocityVector += Vector3.up * initialVelocity * Mathf.Sin(angleUpDown) * Mathf.Sin(angleUpDown) * Mathf.Sin(angleUpDown);

        for (int i = 0; ; i++)
        {
            float t = i / desiredDistance;
            float time = bulletVelocity * t * 2 * Mathf.Sin(angleUpDown) / -gravity;
            Vector3 position = CalculatePositionAtTime(initialPosition, initialVelocityVector, time);
            bulletPathList.Add(position);
            if (i >= 20 && position.y <= firePort.transform.position.y)
                break;
        }

        lr.positionCount = bulletPathList.Count;
        lr.SetPositions(bulletPathList.ToArray());
    }
    private void AdjustInitialVelocityForDistance()
    {
        float radianLaunchAngle = launchAngle * Mathf.Deg2Rad;
        float horizontalDistance = desiredDistance * Mathf.Cos(radianLaunchAngle);
        float verticalDistance = desiredDistance * Mathf.Sin(radianLaunchAngle);

        float timeToReachDistance = Mathf.Sqrt(2 * verticalDistance / -gravity);
        initialVelocity = horizontalDistance / timeToReachDistance;
    }

    private Vector3 CalculatePositionAtTime(Vector3 initialPosition, Vector3 initialVelocity, float time)
    {
        float x = initialPosition.x + initialVelocity.x * time;
        float y = initialPosition.y + initialVelocity.y * time + 0.5f * gravity * time * time;
        float z = initialPosition.z + initialVelocity.z * time;
        return new Vector3(x, y, z);
    }
}
   

