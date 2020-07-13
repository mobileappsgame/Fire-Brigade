using UnityEngine;
using AppodealAds.Unity.Api;

namespace Cubra
{
    public class Banner : MonoBehaviour
    {
        public void ShowBanner()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
                    Appodeal.show(Appodeal.INTERSTITIAL);
            }
        }
    }
}