using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class InAppManager : MonoBehaviour, IStoreListener
{
    // Событие успешной покупки
    public UnityEvent OnPurchase;

    private IStoreController controller;
    private IExtensionProvider extensions;

    private void Awake()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct("3_stretcher", ProductType.Consumable, new IDs
        {
            {"3_stretcher_google", GooglePlay.Name},
            {"3_stretcher_mac", MacAppStore.Name}
        });

        builder.AddProduct("10_stretcher", ProductType.Consumable, new IDs
        {
            {"10_stretcher_google", GooglePlay.Name},
            {"10_stretcher_mac", MacAppStore.Name}
        });

        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// Вызывается, когда приложение может подключиться к Unity IAP
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    /// <summary>
    /// Выполнена ли инициализация Unity IAP
    /// </summary>
    private bool IsInitialized()
    {
        return controller != null && extensions != null;
    }

    /// <summary>
    /// Покупка товара с указанным идентификатором
    /// </summary>
    /// <param name="productId">id товара</param>
    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = controller.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                controller.InitiatePurchase(product);
            }
            else
            {
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
            }
        }
    }

    /// <summary>
    /// Ошибка инициализации Unity IAP
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error) {}

    /// <summary>
    /// Успешное завершение покупки
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (string.Equals(e.purchasedProduct.definition.id, "3_stretcher", StringComparison.Ordinal))
        {
            PurchaseCompleted(3);
        }
        else if (string.Equals(e.purchasedProduct.definition.id, "10_stretcher", StringComparison.Ordinal))
        {
            PurchaseCompleted(10);
        }

        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Действия при завершенной покупке
    /// </summary>
    private void PurchaseCompleted(int quantity)
    {
        // Увеличиваем количество улучшенных носилок
        PlayerPrefs.SetInt("super-stretcher", PlayerPrefs.GetInt("super-stretcher") + quantity);
        OnPurchase?.Invoke();
    }

    /// <summary>
    /// Покупка товара не удалась
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p) {}
}