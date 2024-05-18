using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace FasterEngine
{
    [BepInPlugin("com.Melon.FasterEngine", "FasterEngine", "1.0.0")]
    public class FasterEngine : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogInfo("FasterEngine has loaded");

            Harmony harmony = new Harmony("com.Melon.FasterEngine");

            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Engine))]
    [HarmonyPatch("FixedUpdate")]
    public class EnginePatch
    {
        static bool Prefix(object __instance)
        {
            var bodyField = AccessTools.Field(__instance.GetType(), "body");
            var timePassedField = AccessTools.Field(__instance.GetType(), "timePassed");
            var forceAnimField = AccessTools.Field(__instance.GetType(), "forceAnim");
            var forceField = AccessTools.Field(__instance.GetType(), "force");

            Rigidbody2D body = (Rigidbody2D)bodyField.GetValue(__instance);
            float timePassed = (float)timePassedField.GetValue(__instance);
            AnimationCurve forceAnim = (AnimationCurve)forceAnimField.GetValue(__instance);
            float force = (float)forceField.GetValue(__instance);

            body.AddRelativeForce(Vector2.right * forceAnim.Evaluate(timePassed) * force * 100);

            timePassedField.SetValue(__instance, timePassed + Time.fixedDeltaTime);

            return false;
        }
    }
}
