using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IPermissionGrantedListener
    {
        void writeExternalStorageResponse(int result);
        void accessCoarseLocationResponse(int result);
    }
}