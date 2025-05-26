/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UIStackClose
{
    private static Stack<GameObject> stack = new Stack<GameObject>();

    public static void Push(GameObject popup)
    {
        if (popup != null && !stack.Contains(popup))
        {
            stack.Push(popup);
        }
    }

    public static void PopTop()
    {
        if (stack.Count > 0)
        {
            GameObject top = stack.Pop();
            if (top != null)
            {
                top.SetActive(false);
            }
        }
    }

    public static bool HasPopup => stack.Count > 0;

    public static void Remove(GameObject popup)
    {
        if (stack.Contains(popup))
        {
            // tạo lại stack mới không chứa popup đó
            stack = new Stack<GameObject>(stack.Where(go => go != popup).Reverse());
        }
    }

    public static void Clear()
    {
        stack.Clear();
    }
}
*/