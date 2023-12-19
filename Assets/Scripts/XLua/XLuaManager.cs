using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// 说明：xLua管理类
/// 注意：
/// 1、整个Lua虚拟机执行的脚本分成3个模块：热修复、公共模块、逻辑模块
/// 2、公共模块：提供Lua语言级别的工具类支持，和游戏逻辑无关，最先被启动
/// 3、热修复模块：脚本全部放Lua/XLua目录下，随着游戏的启动而启动
/// 4、逻辑模块：资源热更完毕后启动
/// 5、资源热更以后，理论上所有被加载的Lua脚本都要重新执行加载，如果热更某个模块被删除，则可能导致Lua加载异常，这里的方案是释放掉旧的虚拟器另起一个
/// @by wsh 2017-12-28
/// </summary>
[Hotfix]
[LuaCallCSharp]
public class XLuaManager : MonoBehaviour
{
    public const string luaScriptsFolder = "LuaScripts";
    const string gameMainScriptName = "GameMain";
    LuaEnv luaEnv = null;
    LuaUpdater luaUpdater = null;
    public bool HasGameStart { get; protected set; }

    public LuaEnv GetLuaEnv()
    {
        return luaEnv;
    }

    void Awake()
    {
        InitLuaEnv();
        StartGame();
    }

    void InitLuaEnv()
    {
        luaEnv = new LuaEnv();
        HasGameStart = false;
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);
        }
        else
        {
            Debug.LogError("InitLuaEnv null!!!");
        }
    }

    public void SafeDoString(string scriptContent)
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }

    public void StartGame()
    {
        if (luaEnv != null)
        {
            LoadScript(gameMainScriptName);
            SafeDoString("GameMain.Start()");
            HasGameStart = true;
        }
    }

    public void ReloadScript(string scriptName)
    {
        SafeDoString(string.Format("package.loaded['{0}'] = nil", scriptName));
        LoadScript(scriptName);
    }

    void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require('{0}')", scriptName));
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
        filepath = filepath.Replace(".", "/") + ".lua";
        scriptPath = Path.Combine(Application.dataPath, luaScriptsFolder);
        scriptPath = Path.Combine(scriptPath, filepath);
        //Logger.Log("Load lua script : " + scriptPath);
        return GameUtility.SafeReadAllBytes(scriptPath);
    }

    private void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
            if (Time.frameCount % 100 == 0)
            {
                luaEnv.FullGc();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (luaEnv != null && HasGameStart)
        {
            SafeDoString("GameMain.OnApplicationQuit()");
        }
    }

    public void Dispose()
    {
        if (luaUpdater != null)
        {
            luaUpdater.OnDispose();
        }

        if (luaEnv != null)
        {
            try
            {
                luaEnv.Dispose();
                luaEnv = null;
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
}