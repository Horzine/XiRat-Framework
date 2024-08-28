using DG.Tweening;

namespace Xi.Extension.DoTween
{
    public static class DoTweenExtension
    {
        public static void SafeKill(this Tween tween)
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
        }
    }
}
