using UnityEngine;
using System.Collections;
using XLua;

namespace XLuaFramework {
    public class LuaManager : Manager {
        LuaEnv luaenv = null;
        // Use this for initialization
        void Awake() {
        }

        void Start()
        {
            luaenv = new LuaEnvEx();
        }
        void Update()
        {
            if (luaenv != null)
            {
                luaenv.Tick();
            }
        }

        void OnDestroy()
        {
            luaenv.Dispose();
        }

        public void InitStart() {
            luaenv.DoString("require 'lua/main'");
        }


        public object[] DoFile(string filename) {
            return null;
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            return null;
        }

        public void LuaGC() {
        }

        public void Close() {
        }
    }
}