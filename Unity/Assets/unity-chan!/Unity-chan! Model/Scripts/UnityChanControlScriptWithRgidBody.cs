// Mecanim的動畫數據，帶有Rigidbody的控制器
// 範例
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
    // 列舉所需的組件
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class UnityChanControlScriptWithRgidBody : MonoBehaviour
    {
        // 設定動畫播放速度
        public float animSpeed = 1.5f;
        // 相機運動的平滑設置
        public float lookSmoother = 3.0f;
        // 是否使用Mecanim曲線調整
        public bool useCurves = true;
        // 該開關未開啟時，曲線將不被使用
        public float useCurvesHeight = 0.5f;

        // 以下是角色控制器的參數
        public float forwardSpeed = 7.0f;
        public float backwardSpeed = 2.0f;
        public float rotateSpeed = 2.0f;
        public float jumpPower = 3.0f;
        private CapsuleCollider col;
        private Rigidbody rb;
        private Vector3 velocity;
        private float orgColHight;
        private Vector3 orgVectColCenter;
        private Animator anim;
        private AnimatorStateInfo currentBaseState;

        private GameObject cameraObject;

        // 將各動畫狀態的參數設置為哈希值
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");

        // 初始化
        void Start()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            cameraObject = GameObject.FindWithTag("MainCamera");
            orgColHight = col.height;
            orgVectColCenter = col.center;
        }

        // 主要處理，由於與Rigidbody結合，因此在FixedUpdate內進行處理
        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            anim.SetFloat("Speed", v);
            anim.SetFloat("Direction", h);
            anim.speed = animSpeed;
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            rb.useGravity = true;

            velocity = new Vector3(0, 0, v);
            velocity = transform.TransformDirection(velocity);

            if (v > 0.1)
            {
                velocity *= forwardSpeed;
            }
            else if (v < -0.1)
            {
                velocity *= backwardSpeed;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (currentBaseState.fullPathHash == locoState)
                {
                    if (!anim.IsInTransition(0))
                    {
                        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                        anim.SetBool("Jump", true);
                    }
                }
            }

            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate(0, h * rotateSpeed, 0);

            if (currentBaseState.fullPathHash == locoState)
            {
                if (useCurves)
                {
                    resetCollider();
                }
            }
            else if (currentBaseState.fullPathHash == jumpState)
            {
                cameraObject.SendMessage("setCameraPositionJumpView");
                if (!anim.IsInTransition(0))
                {
                    if (useCurves)
                    {
                        float jumpHeight = anim.GetFloat("JumpHeight");
                        float gravityControl = anim.GetFloat("GravityControl");
                        if (gravityControl > 0)
                            rb.useGravity = false;

                        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                        RaycastHit hitInfo = new RaycastHit();
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.distance > useCurvesHeight)
                            {
                                col.height = orgColHight - jumpHeight;
                                float adjCenterY = orgVectColCenter.y + jumpHeight;
                                col.center = new Vector3(0, adjCenterY, 0);
                            }
                            else
                            {
                                resetCollider();
                            }
                        }
                    }
                    anim.SetBool("Jump", false);
                }
            }
            else if (currentBaseState.fullPathHash == idleState)
            {
                if (useCurves)
                {
                    resetCollider();
                }
                if (Input.GetButtonDown("Jump"))
                {
                    anim.SetBool("Rest", true);
                }
            }
            else if (currentBaseState.fullPathHash == restState)
            {
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Rest", false);
                }
            }
        }

        // 重置角色碰撞器大小的函數
        void resetCollider()
        {
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }
    }
}
