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

    // Идентификаторы товаров
    private readonly string stretcherX3 = "stretcher_3";
    private readonly string stretcherX10 = "stretcher_10";

    private void Start()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(stretcherX3, ProductType.Consumable);
        builder.AddProduct(stretcherX10, ProductType.Consumable);

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
    private void BuyProductID(string productId)
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
    /// Покупка указанного товара
    /// </summary>
    /// <param name="quantity">количество носилок</param>
    public void BuyProduct(int quantity)
    {
        switch (quantity)
        {
            case 3:
                BuyProductID(stretcherX3);
                break;
            case 10:
                BuyProductID(stretcherX10);
                break;
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
        if (string.Equals(e.purchasedProduct.definition.id, stretcherX3, StringComparison.Ordinal))
        {
            PurchaseCompleted(3);
        }
        else if (string.Equals(e.purchasedProduct.definition.id, stretcherX10, StringComparison.Ordinal))
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