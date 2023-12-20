//
// Unity-chan 的第三人稱相機
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;

namespace UnityChan
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public float smooth = 3f;       // 用於平滑攝影機運動的變數
        Transform standardPos;          // 攝影機的標準位置，由遊戲中的一個變換指定
        Transform frontPos;         // 前置攝影機位置
        Transform jumpPos;          // 跳躍攝影機位置

        // 用於不平滑過渡時（快速切換）的布爾標誌
        bool bQuickSwitch = false;  // 快速切換攝影機位置

        void Start()
        {
            // 各個參考的初始化
            standardPos = GameObject.Find("CamPos").transform;

            if (GameObject.Find("FrontPos"))
                frontPos = GameObject.Find("FrontPos").transform;

            if (GameObject.Find("JumpPos"))
                jumpPos = GameObject.Find("JumpPos").transform;

            // 開始時將攝影機設置為標準位置和方向
            transform.position = standardPos.position;
            transform.forward = standardPos.forward;
        }

        void FixedUpdate()  // 這個攝影機切換在FixedUpdate()內才能正常運作
        {
            if (Input.GetButton("Fire1"))
            {   // 左 Ctrl	
                // 切換到前置攝影機
                setCameraPositionFrontView();
            }
            else if (Input.GetButton("Fire2"))
            {   // Alt	
                // 切換到跳躍攝影機
                setCameraPositionJumpView();
            }
            else
            {
                // 返回攝影機到標準位置和方向
                setCameraPositionNormalView();
            }
        }

        void setCameraPositionNormalView()
        {
            if (bQuickSwitch == false)
            {
                // 將攝影機位置和方向平滑地切換到標準位置
                transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
                transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
            }
            else
            {
                // 快速切換攝影機到標準位置和方向
                transform.position = standardPos.position;
                transform.forward = standardPos.forward;
                bQuickSwitch = false;
            }
        }

        void setCameraPositionFrontView()
        {
            // 切換到前置攝影機
            bQuickSwitch = true;
            transform.position = frontPos.position;
            transform.forward = frontPos.forward;
        }

        void setCameraPositionJumpView()
        {
            // 切換到跳躍攝影機
            bQuickSwitch = false;
            transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);
        }
    }
}
