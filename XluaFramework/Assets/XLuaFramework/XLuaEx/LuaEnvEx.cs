using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua
{
    public class LuaEnvEx : LuaEnv
    {
        public LuaEnvEx()
        {
            AddSearcherEx(StaticLuaCallbacks.LoadFromCustomDataPath, 5);
        }

        private void AddSearcherEx(lua_CSFunction searcher, int index)
        {
            object obj = (LuaEnv)this;
            Type type = obj.GetType();
            type = type.BaseType;
            MethodInfo mf = type.GetMethod("AddSearcher", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            mf.Invoke(obj, new object[] { searcher, index });
        }
    }
}