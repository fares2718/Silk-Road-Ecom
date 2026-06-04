export interface ShippingAddressSnapshot {
  shippingFullName: string;
  shippingStreet: string;
  shippingCity: string;
  shippingPostalCode: string;
  shippingCountry: string;
}

export interface DeliverySnapshot {
  deliveryProviderName: string;
  deliveryMethodName: string;
  deliveryPrice: number;
}

export interface OrderItem {
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Order{
  orderId: string;
  shippingAddressSnapshot: ShippingAddressSnapshot;
  deliverySnapshot: DeliverySnapshot;
  subTotal: number;
  total: number;
  orderStatus: string;
  orderDate: string; // Typically an ISO string when serialized from JSON
  orderItems: OrderItem[];
}

export interface PlaceOrder{
  deliveryMethodID: number;
  basketID: string;
}

export interface DeliveryMethod {
    deliveryMethodId: number;
    providerName: string;
    methodName: string;
    description: string | null; // maps from string?
    deliveryTime: string;
    price: number;              // decimal maps to number
    available: boolean;
}

