using Unity.Collections;
using UnityEditor;

/// <summary>
/// 内存泄露检测模式
/// </summary>
/// namespace Game.HHL.Editor
public class LeakDetectionMode
{
    [MenuItem("HHL/Jobs/内存泄漏检测/显示当前模式")]
    private static void ShowLeakDetection()
    {
        var message = string.Format("当前模式： {0}", NativeLeakDetection.Mode.ToString());
        EditorUtility.DisplayDialog("内存泄漏检测模式", message, "OK");
    }


    [MenuItem("HHL/Jobs/内存泄漏检测/禁用")]
    private static void LeakDetectionDisable()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
    }

    // 验证方法会在正式方法前执行，通不过就会置灰
    [MenuItem("HHL/Jobs/内存泄漏检测/禁用", true)]
    private static bool ValidateLeakDetectionDisable()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.Disabled;
    }


    [MenuItem("HHL/Jobs/内存泄漏检测/启用")]
    private static void LeakDetectionEnabled()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.Enabled;
    }

    [MenuItem("HHL/Jobs/内存泄漏检测/启用", true)]
    private static bool ValidateLeakDetectionEnabled()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.Enabled;
    }


    [MenuItem("HHL/Jobs/内存泄漏检测/启用堆栈跟踪")]
    private static void LeakDetectionEnabledWithStackTrace()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    [MenuItem("HHL/Jobs/内存泄漏检测/启用堆栈跟踪", true)]
    private static bool ValidateLeakDetectionEnabledWithStackTrace()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.EnabledWithStackTrace;
    }
}