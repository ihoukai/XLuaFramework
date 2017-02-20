using UnityEngine;
using System.Collections;

namespace XLuaFramework {

    /// <summary>
    /// </summary>
    public class Main : MonoBehaviour {

        void Start() {
            AppFacade.Instance.StartUp();   //启动游戏
        }
    }
}