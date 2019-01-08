using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils {

    /// <summary>
    /// 设置layer,包含子节点
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void SetLayer(GameObject go, int layer) {
        go.layer = layer;

        Transform t = go.transform;

        for (int i = 0, imax = t.childCount; i < imax; ++i) {
            Transform child = t.GetChild(i);
            SetLayer(child.gameObject, layer);
        }
    }
}
