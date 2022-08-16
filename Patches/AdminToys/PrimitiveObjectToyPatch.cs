using AdminToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using UnityEngine;

namespace Kutils.SCP.Patchs.Objects.AdminToys
{
    [HarmonyLib.HarmonyPatch(typeof(PrimitiveObjectToy))]
    [HarmonyLib.HarmonyPatch("OnSpawned")]
    public static class PrimitiveObjectToyPatch
    {
        public static bool Prefix(PrimitiveObjectToy __instance, ReferenceHub admin, ArraySegment<string> arguments)
        {
            if (admin != null)
                return true;
            string[] array = arguments.Array;
            Enum.TryParse<PrimitiveType>(array[0], out PrimitiveType primitiveType);
            ColorUtility.TryParseHtmlString(array[1], out Color color);
            __instance.NetworkPrimitiveType = primitiveType;
            __instance.NetworkMaterialColor = color;
            __instance.transform.localPosition = new Vector3(float.Parse(array[2]), float.Parse(array[3]), float.Parse(array[4]));
            __instance.transform.eulerAngles = new Vector3(float.Parse(array[5]), float.Parse(array[6]), float.Parse(array[7]));
            __instance.transform.localScale = new Vector3(float.Parse(array[8]), float.Parse(array[9]), float.Parse(array[10]));
            __instance.NetworkScale = __instance.transform.localScale;
            return false;
        }
    }
}
