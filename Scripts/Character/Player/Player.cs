using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //通过代码来给挂载的物体添加组件
public class Player : Character
{

    [SerializeField] StateBar_HUD stateBar_HUD;

    /// <summary>
    /// 表示是否开启再生生命
    /// </summary>
    [SerializeField] bool regenerateHealth = true;

    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]

    [SerializeField] PlayerInput input;


    [Header("---- MOVE ----")]

    [SerializeField] float moveSpeed = 10f;

    /// <summary>
    /// 物体移动过程中的加速时间
    /// </summary>
    [SerializeField] float accelerationTime = 3f;
    /// <summary>
    /// 物体移动过程中的减速时间
    /// </summary>
    [SerializeField] float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;


    [Header("---- FIRE ----")]

    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectileOverdrive;

    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleButtom;

    [SerializeField, Range(0, 2)] int weaponPower = 0;

    [SerializeField] float fireInterval;

    [SerializeField] AudioData projectileLaunchSFX;


    [Header("---- DODGE ----")]

    [SerializeField] AudioData dodgeSFX;

    [SerializeField,Range(0, 100)] int dodgeEneryCost = 25;

    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;

    /// <summary>
    /// 闪避时Player最小的缩放值
    /// </summary>
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);


    [Header("---- OVERDRIVE ----")]

    [SerializeField] int overdriveDodgeFactor = 2;

    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;

    bool isdodging = false;
    bool isOverdriving = false;

    readonly float timeScaleDuration = 0.5f;

    float paddingX = 0.2f;
    float paddingY = 0.2f;

    float dodgeDuration;
    float currentRoll;

    float t;

    Vector2 previousVelocity;

    Quaternion previousRotation;

    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitRegenerateHealthTime;
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waitDecelerationTime;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    new Rigidbody2D rigidbody;

    new Collider2D collider;

    private void Awake()
    {
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitRegenerateHealthTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;

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

        input.onOverdrive += Overdrive;

        PlayerOverdive.on += OverdriveOn;
        PlayerOverdive.off += OverdriveOff;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStop -= StopMove;

        input.onFire -= Fire;
        input.onStopFire -= StopFire;

        input.onDodge -= Dodge;

        input.onOverdrive -= Overdrive;

        PlayerOverdive.on -= OverdriveOn;
        PlayerOverdive.off -= OverdriveOff;
    }

    private void Start()
    {
        

        rigidbody.gravityScale = 0f;

        input.EnableGamePlayInput();

        //初始化HUD血条
        stateBar_HUD.Initialize(health, maxHealth);     

        //TakeDamage(50f);       //用来测试自动回血功能
    }

    private void Update()
    {
        //由于Update 性能消耗过大 改用携程来减少性能消耗
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

        //在调用完基类函数后更新玩家独有状态条的状态
        /// 一、更新HUD 血条状态
        stateBar_HUD.UpdateState(health, maxHealth);

        //受伤时进入子弹时间
        TimeController.Instance.BulletTime(timeScaleDuration);

        if (gameObject.activeSelf && regenerateHealth)
        {
            if(healthRegenerateCoroutine != null)   //防止玩家在恢复过程中再次受到伤害而有多个携程同时运行，在启用这个携程之前先将它禁用
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
        GameManager.GameState = GameState.GameOver;
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
        StopCoroutine(nameof(DecelerationCoroutine));
    }



    /// <summary>
    /// 用来实现Player 加速和减速 效果的携程
    /// </summary>
    /// <param name="moveVelocity">最大速度</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.localRotation;

        /* 两种实现加速减速效果的写法 */
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            //实现Player 移动时加速运动
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);

            //实现Player 移动时绕x轴旋转
            transform.localRotation = Quaternion.Lerp(previousRotation, moveRotation, t);

            yield return new WaitForFixedUpdate();  //由于使用了Time.fixedDeltaTime 则挂起等待的时间也应该是固定下一帧的更新
        }

        //while (t < time)
        //{
        //    t += Time.fixedDeltaTime;
        //    //实现Player 移动时加速运动
        //    rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);

        //    //实现Player 移动时绕x轴旋转
        //    transform.localRotation = Quaternion.Lerp(transform.localRotation, moveRotation, t / time);

        //    yield return null;
        //}
    }

    /// <summary>
    /// 用来限制Player 移动位不超过屏幕的携程
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

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(nameof(MovePositionLimitedCoroutine));
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
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleButtom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleButtom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);    //开火时播放开火音效

            /*yield return new WaitForSeconds(fireInterval);*///在循环内new 效率太过于底下 舍弃该方案

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    #endregion

    #region DODGE
    private void Dodge()
    {
        //如果 正在闪避 或则 能量不够的情况下 则不执行下方的代码
        if (isdodging || !PlayerEnergy.Instance.isEnough(dodgeEneryCost)) return;  

        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isdodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);  //播放闪避音效
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEneryCost);

        //让玩家无敌 并且 改变缩放大小
        ///打开碰撞器的触发器开关（iTrrigger）来让玩家与子弹无法碰撞从而实现无敌的效果
        //·闪避开始
        collider.isTrigger = true;

        var scale = transform.localScale;

        //绕X轴旋转
        currentRoll = 0f;   //每次旋转开始之前将当前滚转角重置
        TimeController.Instance.BulletTime(timeScaleDuration, timeScaleDuration);

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

        //* Method 2 分别对缩小和放大使用两次线性插值函数
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

        //* Method 3 用贝塞尔曲线得到更加丝滑的曲线
        while (currentRoll < maxRoll)
        {
            currentRoll += Time.deltaTime * rollSpeed;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return null;
        }

        //·闪避结束
        collider.isTrigger = false;
        isdodging = false;
        
    }
    #endregion

    #region OVERDRIVE
    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.isEnough(PlayerEnergy.MAX)) return;

        PlayerOverdive.on.Invoke();
    }

    private void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEneryCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(timeScaleDuration, timeScaleDuration);
    }

    private void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEneryCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }

    #endregion

}
