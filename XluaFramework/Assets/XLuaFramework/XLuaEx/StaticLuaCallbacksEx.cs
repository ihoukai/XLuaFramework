#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
using XLua.LuaDLL;
using System.IO;
using System;
using UnityEngine;
using System.Text;
#endif

namespace XLua
{
    public partial class StaticLuaCallbacks
    {
#if !XLUA_GENERAL
        [MonoPInvokeCallback(typeof(LuaCSFunction))]
        internal static int LoadFromCustomDataPath(RealStatePtr L)
        {
            try
            {
                string filename = LuaAPI.lua_tostring(L, 1).Replace('.', '/') + ".lua";
                var filepath = XLuaFramework.Util.GetRelativePath() + filename;
#if UNITY_ANDROID && !UNITY_EDITOR
                UnityEngine.WWW www = new UnityEngine.WWW(filepath);
                while (true)
                {
                    if (www.isDone || !string.IsNullOrEmpty(www.error))
                    {
                        System.Threading.Thread.Sleep(50); //比较hacker的做法
                        if (!string.IsNullOrEmpty(www.error))
                        {
                            LuaAPI.lua_pushstring(L, string.Format(
                               "\n\tno such file '{0}' in streamingAssetsPath!", filename));
                        }
                        else
                        {
                            UnityEngine.Debug.Log("load lua file from StreamingAssets is obsolete, filename:" + filename);
                            if (LuaAPI.luaL_loadbuffer(L, www.text, "@" + filename) != 0)
                            {
                                return LuaAPI.luaL_error(L, String.Format("error loading module {0} from streamingAssetsPath, {1}",
                                    LuaAPI.lua_tostring(L, 1), LuaAPI.lua_tostring(L, -1)));
                            }
                        }
                        break;
                    }
                }
#else
                if (File.Exists(filepath))
                {
                    Stream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    stream.Close();

                    UnityEngine.Debug.LogWarning("load lua file from StreamingAssets is obsolete, filename:" + filename);
                    if (LuaAPI.luaL_loadbuffer(L, text, "@" + filename) != 0)
                    {
                        return LuaAPI.luaL_error(L, String.Format("error loading module {0} from streamingAssetsPath, {1}",
                            LuaAPI.lua_tostring(L, 1), LuaAPI.lua_tostring(L, -1)));
                    }
                }
                else
                {
                    LuaAPI.lua_pushstring(L, string.Format(
                        "\n\tno such file '{0}' in streamingAssetsPath!", filename));
                }
#endif
                return 1;
            }
            catch (System.Exception e)
            {
                return LuaAPI.luaL_error(L, "c# exception in LoadFromStreamingAssetsPath:" + e);
            }
        }
#endif
    }
}