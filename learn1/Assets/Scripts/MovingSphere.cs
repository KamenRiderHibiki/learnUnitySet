using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    public enum ControlWays
    {
        Keyboard, Mouse
    }
    [SerializeField]
    ControlWays controlWay = ControlWays.Keyboard;
    [SerializeField, Range(0f, 5f)]
    float maxSpeed = 1f;
    [SerializeField, Range(0f, 1f)]
    float maxAccleration = 0.1f;
    [SerializeField, Range(0f, 0.15f)]
    float friction = 0.001f;
    [SerializeField]
    Rect allowArea = new Rect(-4.5f, -4.5f, 9f, 9f);
    public Vector3 direction;
    float directionTimer;
    Vector3 lastPosition;
    public Vector3 velocity;
    Vector3 accelarion;
    void Awake()
    {
        velocity = accelarion = new Vector3(0, 0);
        direction = Vector3.zero;
        lastPosition = transform.localPosition;
    }
    void Update() {
        Vector3 newPosition = transform.localPosition;
        if (controlWay == ControlWays.Keyboard)
        {
            Vector2 playerInput;
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            if (!playerInput.Equals(Vector2.zero))
            {
                playerInput = Vector2.ClampMagnitude(playerInput, 1f);
                Vector3 desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed;
                float speedChange = maxAccleration * Time.deltaTime;
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speedChange);
                velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, speedChange);
            }
            else
            {
                velocity = slowByFriction(velocity);
            }
            velocity += accelarion * Time.deltaTime;
            newPosition = transform.localPosition + velocity;
        }
        else if(controlWay == ControlWays.Mouse)
        {
            if (Input.GetMouseButton(0))
            {
                newPosition = transform.localPosition + velocity;
                velocity = slowByFriction(velocity);
            }
            else
            {
                //获取需要移动物体的世界转屏幕坐标
                Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                //获取鼠标位置
                Vector3 mousePos = Input.mousePosition;
                //因为鼠标只有X，Y轴，所以要赋予给鼠标Z轴
                mousePos.z = screenPos.z;
                //把鼠标的屏幕坐标转换成世界坐标
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                //控制物体移动
                if(direction != Vector3.zero)
                {
                    velocity = Vector3.Normalize(direction * Time.deltaTime);
                    velocity.x *= 0.1f;
                    velocity.z *= 0.1f;
                }
                else
                {
                    velocity = Vector3.zero;
                }
                newPosition = worldPos;
                //transform.localPosition = worldPos;
                //刚体的方式
                //transform.GetComponent<Rigidbody>().MovePosition(worldPos);
            }
        }
        if (!allowArea.Contains(new Vector2(newPosition.x, newPosition.z)))
        {
            if (newPosition.x < allowArea.xMin || newPosition.x > allowArea.xMax)
            {
                velocity.x *= -1;
            }
            if (newPosition.z < allowArea.yMin || newPosition.z > allowArea.yMax)
            {
                velocity.z *= -1;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, allowArea.xMin, allowArea.xMax);
            newPosition.z = Mathf.Clamp(newPosition.z, allowArea.yMin, allowArea.yMax);
        }
        transform.localPosition = newPosition;
    }
    void FixedUpdate()
    {
        directionTimer += Time.deltaTime;
        if (directionTimer > 0.1f)
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = mouse.y;
            mouse.y = 0f;
            direction = mouse - lastPosition;
            lastPosition = mouse;
            directionTimer = 0f;
        }
    }

    Vector3 slowByFriction(Vector3 velocity)
    {
        float frictionX = Mathf.Abs(velocity.normalized.x * Time.deltaTime * friction);
        float frictionZ = Mathf.Abs(velocity.normalized.z * Time.deltaTime * friction);
        velocity.x = Mathf.MoveTowards(velocity.x, 0f, frictionX);
        velocity.z = Mathf.MoveTowards(velocity.z, 0f, frictionZ);
        return velocity;
    }
}
