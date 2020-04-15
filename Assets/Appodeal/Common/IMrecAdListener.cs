using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IMrecAdListener
    {
        void onMrecLoaded(bool isPrecache);
        void onMrecFailedToLoad();
        void onMrecShown();
        void onMrecClicked();
        void onMrecExpired();
    }
}