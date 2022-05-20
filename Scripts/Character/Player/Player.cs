using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //ͨ�������������ص������������
public class Player : Character
{

    [SerializeField] StateBar_HUD stateBar_HUD;

    /// <summary>
    /// ��ʾ�Ƿ�����������
    /// </summary>
    [SerializeField] bool regenerateHealth = true;

    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]

    [SerializeField] PlayerInput input;


    [Header("---- MOVE ----")]

    [SerializeField] float moveSpeed = 10f;

    /// <summary>
    /// �����ƶ������еļ���ʱ��
    /// </summary>
    [SerializeField] float accelerationTime = 3f;
    /// <summary>
    /// �����ƶ������еļ���ʱ��
    /// </summary>
    [SerializeField] float decelerationTime = 3f;

    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;
    [SerializeField] float moveRotationAngle = 50f;


    [Header("---- FIRE ----")]

    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;

    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleButtom;

    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [SerializeField] float fireInterval;


    [Header("---- DODGE ----")]

    [SerializeField,Range(0, 100)] int dodgeEneryCost = 25;

    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;

    /// <summary>
    /// ����ʱPlayer��С������ֵ
    /// </summary>
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool isdodging = false;

    float dodgeDuration;
    float currentRoll;

    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitRegenerateHealthTime;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    new Rigidbody2D rigidbody;

    new Collider2D collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;
    }

    private protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStop += StopMove;

        input.onFire += Fire;
        input.onStopFire += StopFire;

        input.onDodge += Dodge;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStop -= StopMove;

        input.onFire -= Fire;
        input.onStopFire -= StopFire;

        input.onDodge -= Dodge;
    }

    private void Start()
    {
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitRegenerateHealthTime = new WaitForSeconds(healthRegenerateTime);

        rigidbody.gravityScale = 0f;

        input.EnableGamePlayInput();

        //��ʼ��HUDѪ��
        stateBar_HUD.Initialize(health, maxHealth);     

        //TakeDamage(50f);       //���������Զ���Ѫ����
    }

    private void Update()
    {
        //����Update �������Ĺ��� ����Я����������������
        //transform.position = Viewport.Instance.PlayerMovablePosition(transform.position);
    }

    #region HEALTH
    //public override void RestoreHealth(float value)
    //{
    //    base.RestoreHealth(value);
    //    Debug.Log("Regenerate health! Current health :" + health + "\n Time :" + Time.time);
    //}

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        //�ڵ�������ຯ���������Ҷ���״̬����״̬
        /// һ������HUD Ѫ��״̬
        stateBar_HUD.UpdateState(health, maxHealth);

        if (gameObject.activeSelf && regenerateHealth)
        {
            if(healthRegenerateCoroutine != null)   //��ֹ����ڻָ��������ٴ��ܵ��˺����ж��Я��ͬʱ���У����������Я��֮ǰ�Ƚ�������
            {
                StopCoroutine(healthRegenerateCoroutine);
            }
            healthRegenerateCoroutine = StartCoroutine(
                HealthRegenerateCoroutine(waitRegenerateHealthTime, healthRegeneratePercent));
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        stateBar_HUD.UpdateState(health, maxHealth);
    }

    public override void Die()
    {
        stateBar_HUD.UpdateState(0, maxHealth);
        base.Die();
    }
    #endregion

    #region MOVE
    private void Move(Vector2 moveInput)
    {
        /*Vector2 moveAmount = moveInput * moveSpeed;*/
        /*rigidbody.velocity = moveInput * moveSpeed;*/
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        //moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed,
            Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StartCoroutine(nameof(MovePositionLimitedCoroutine));
    }

    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        /*rigidbody.velocity = Vector2.zero;*/
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(nameof(MovePositionLimitedCoroutine));
    }



    /// <summary>
    /// ����ʵ��Player ���ٺͼ��� Ч����Я��
    /// </summary>
    /// <param name="moveVelocity">����ٶ�</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        float t = 0;

        /* ����ʵ�ּ��ټ���Ч����д�� */
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            //ʵ��Player �ƶ�ʱ�����˶�
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t);

            //ʵ��Player �ƶ�ʱ��x����ת
            transform.localRotation = Quaternion.Lerp(transform.localRotation, moveRotation, t);

            yield return null;
        }

        //while (t < time)
        //{
        //    t += Time.fixedDeltaTime;
        //    //ʵ��Player �ƶ�ʱ�����˶�
        //    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);

        //    //ʵ��Player �ƶ�ʱ��x����ת
        //    transform.localRotation = Quaternion.Lerp(transform.localRotation, moveRotation, t / time);

        //    yield return null;
        //}
    }

    /// <summary>
    /// ��������Player �ƶ�λ��������Ļ��Я��
    /// </summary>
    /// <returns></returns>
    IEnumerator MovePositionLimitedCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMovablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    #endregion

    #region FIRE
    private void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //switch (weaponPower)
            //{
            //    case 0:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        break;
            //    case 1:
            //        Instantiate(projectile1, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile1, muzzleButtom.position, Quaternion.identity);
            //        break;
            //    case 2:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        Instantiate(projectile2, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile3, muzzleButtom.position, Quaternion.identity);
            //        break;
            //    default:
            //        break;
            //}

            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);
                    PoolManager.Release(projectile1, muzzleButtom.position);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    PoolManager.Release(projectile2, muzzleTop.position);
                    PoolManager.Release(projectile3, muzzleButtom.position);
                    break;
                default:
                    break;
            }
            /*yield return new WaitForSeconds(fireInterval);*///��ѭ����new Ч��̫���ڵ��� �����÷���
            yield return waitForFireInterval;
        }
    }
    #endregion

    #region DODGE
    private void Dodge()
    {
        //��� �������� ���� ��������������� ��ִ���·��Ĵ���
        if (isdodging || !PlayerEnergy.Instance.isEnough(dodgeEneryCost)) return;  

        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isdodging = true;
        //��������
        PlayerEnergy.Instance.Use(dodgeEneryCost);

        //������޵� ���� �ı����Ŵ�С
        ///����ײ���Ĵ��������أ�iTrrigger������������ӵ��޷���ײ�Ӷ�ʵ���޵е�Ч��
        //�����ܿ�ʼ
        collider.isTrigger = true;

        var scale = transform.localScale;

        //��X����ת
        currentRoll = 0f;   //ÿ����ת��ʼ֮ǰ����ǰ��ת������

        //* Method 1
        //
        //while(currentRoll < maxRoll)
        //{
        //    currentRoll += rollSpeed * Time.deltaTime;
        //    transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

        //    if(currentRoll < maxRoll / 2)
        //    {
        //        scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //        scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //        scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //    }
        //    else
        //    {
        //        scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //        scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //        scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //    }

        //    transform.localScale = scale;
        //    yield return null;
        //}

        //* Method 2 �ֱ����С�ͷŴ�ʹ���������Բ�ֵ����
        //var t1 = 0f;
        //var t2 = 0f;
        
        //while(currentRoll < maxRoll)
        //{
        //    currentRoll += Time.deltaTime * rollSpeed;
        //    transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

        //    if(currentRoll < maxRoll / 2)
        //    {
        //        t1 += Time.deltaTime / dodgeDuration;
        //        transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
        //    }
        //    else
        //    {
        //        t2 += Time.deltaTime / dodgeDuration;
        //        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
        //    }

        //    yield return null;
        //}

        //* Method 3 �ñ��������ߵõ�����˿��������
        while(currentRoll < maxRoll)
        {
            currentRoll += Time.deltaTime * rollSpeed;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return null;
        }

        //�����ܽ���
        collider.isTrigger = false;
        isdodging = false;
        
    }
    #endregion

}