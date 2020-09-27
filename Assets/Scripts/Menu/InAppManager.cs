using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Cubra
{
    public class InAppManager : MonoBehaviour, IStoreListener
    {
        // Событие успешной покупки
        public UnityEvent OnPurchase;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        // Идентификаторы товаров
        private readonly string _stretcherX3 = "stretcher_3";
        private readonly string _stretcherX10 = "stretcher_10";

        private void Start()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            
            builder.AddProduct(_stretcherX3, ProductType.Consumable);
            builder.AddProduct(_stretcherX10, ProductType.Consumable);
            
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Инициализация Unity IAP
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;
        }

        /// <summary>
        /// Выполнена ли инициализация Unity IAP
        /// </summary>
        private bool IsInitialized()
        {
            return _controller != null && _extensions != null;
        }

        /// <summary>
        /// Покупка указанного товара
        /// </summary>
        /// <param name="identifier">идентификатор товара</param>
        public void BuyProduct(string identifier)
        {
            BuyProductID(identifier);
        }

        /// <summary>
        /// Покупка товара с указанным идентификатором
        /// </summary>
        private void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                Product product = _controller.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    _controller.InitiatePurchase(product);
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
            if (string.Equals(e.purchasedProduct.definition.id, _stretcherX3, StringComparison.Ordinal))
            {
                PurchaseCompleted(3);
            }
            else if (string.Equals(e.purchasedProduct.definition.id, _stretcherX10, StringComparison.Ordinal))
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
}