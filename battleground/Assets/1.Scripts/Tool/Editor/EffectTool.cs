using UnityEngine;
using UnityEditor;
using System.Text;
using UnityObject = UnityEngine.Object;

/// <summary>
/// 
/// </summary>
public class EffectTool : EditorWindow
{
    //UI 그리는데 필요한 변수들
    public int uiWidthLarge = 300;
    public int uiWidthMiddle = 200;
    private int selection = 0;
    private Vector2 SP1 = Vector2.zero;
    private Vector2 SP2 = Vector2.zero;
}
