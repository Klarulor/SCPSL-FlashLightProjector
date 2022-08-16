using AdminToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Kutils.SCP.Patchs.Objects.AdminToys
{
    [HarmonyLib.HarmonyPatch(typeof(LightSourceToy))]
    [HarmonyLib.HarmonyPatch("OnSpawned")]
    public static class LightObjectToyPatch
    {
        public static bool Prefix(LightSourceToy __instance, ReferenceHub admin, ArraySegment<string> arguments)
        {
            if (admin != null)
                return true;
            string[] array = arguments.Array;
            ColorUtility.TryParseHtmlString(array[1], out Color color);
            __instance.LightColor = color;
            __instance.LightRange = float.Parse(array[0]);
            __instance.transform.position = new Vector3(float.Parse(array[2]), float.Parse(array[3]), float.Parse(array[4]));
            __instance.transform.eulerAngles = new Vector3(float.Parse(array[5]), float.Parse(array[6]), float.Parse(array[7]));
            __instance.transform.localScale = new Vector3(float.Parse(array[8]), float.Parse(array[9]), float.Parse(array[10]));
            return false;
        }
    }
}
