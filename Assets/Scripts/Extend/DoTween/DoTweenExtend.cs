using DG.Tweening;

namespace Xi.Extend.DoTween
{
    public static class DoTweenExtend
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
