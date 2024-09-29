using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JH
{
    namespace Match3Sample
    {
        public static class InGameUtil
        {
            public static int ParseInt(ref LitJson.JsonData root, string key, int defaultValue = 0) => root.Keys.Contains(key)? (int) root[key] : defaultValue;
        }
    }
}
