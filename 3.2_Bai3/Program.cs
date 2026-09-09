using System;
using System.Collections.Generic;

// ==========================================
// 1. METHOD OVERLOADING
// ==========================================

class DiscountCalculator
{
    // Giảm mặc định 5%
    public decimal ApplyDiscount(decimal totalAmount)
    {
        return totalAmount * 0.95m;
    }

    // Giảm theo phần trăm tùy biến
    public decimal ApplyDiscount(decimal totalAmount, double percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentException(
                "Phần trăm giảm phải từ 0 đến 100.");
        }

        return totalAmount * (decimal)(1 - percentage / 100);
    }

    // Giảm bằng voucher tiền mặt
    // nếu đơn hàng đạt giá trị tối thiểu
    public decimal ApplyDiscount(
        decimal totalAmount,
        decimal fixedVoucher,
        decimal minimumOrder)
    {
        if (totalAmount >= minimumOrder)
        {
            return totalAmount - fixedVoucher;
        }

        return totalAmount;
    }
}


// ==========================================
// 2. METHOD OVERRIDING
// ==========================================

// Lớp cha
class DeliveryService
{
    public string OrderId { get; set; }
    public double DistanceKm { get; set; }

    public DeliveryService(string orderId, double distanceKm)
    {
        OrderId = orderId;
        DistanceKm = distanceKm;
    }

    // Phương thức virtual cho phép lớp con ghi đè
    public virtual decimal CalculateShippingFee()
    {
        return (decimal)DistanceKm * 5000;
    }
}


// ==========================================
// 3. EXPRESS DELIVERY
// ==========================================

class ExpressDelivery : DeliveryService
{
    public ExpressDelivery(string orderId, double distanceKm)
        : base(orderId, distanceKm)
    {
    }

    // Ghi đè phương thức của lớp cha
    public override decimal CalculateShippingFee()
    {
        decimal basicFee = base.CalculateShippingFee();

        return basicFee * 1.5m + 20000;
    }
}


// ==========================================
// 4. ECO DELIVERY
// ==========================================

class EcoDelivery : DeliveryService
{
    public EcoDelivery(string orderId, double distanceKm)
        : base(orderId, distanceKm)
    {
    }

    // Ghi đè phương thức của lớp cha
    public override decimal CalculateShippingFee()
    {
        decimal basicFee = base.CalculateShippingFee();

        // Nếu khoảng cách lớn hơn 10km
        // thì giảm 10% phí cơ bản
        if (DistanceKm > 10)
        {
            return basicFee * 0.9m;
        }

        return basicFee;
    }
}


// ==========================================
// 5. MAIN
// ==========================================

class Program
{
    static void Main()
    {
        // Hiển thị được tiếng Việt trong Console
        Console.OutputEncoding =
            System.Text.Encoding.UTF8;


        // ======================================
        // METHOD OVERLOADING
        // ======================================

        DiscountCalculator calculator =
            new DiscountCalculator();

        decimal totalAmount = 1000000;


        // Gọi overload 1:
        // Giảm mặc định 5%
        decimal result1 =
            calculator.ApplyDiscount(totalAmount);


        // Gọi overload 2:
        // Giảm 15%
        decimal result2 =
            calculator.ApplyDiscount(totalAmount, 15);


        // Gọi overload 3:
        // Voucher 100.000 nếu đơn từ 800.000
        decimal result3 =
            calculator.ApplyDiscount(
                totalAmount,
                100000,
                800000);


        Console.WriteLine("===== METHOD OVERLOADING =====");

        Console.WriteLine(
            $"Đơn hàng ban đầu: {totalAmount:N0} VNĐ");

        Console.WriteLine(
            $"Giảm mặc định 5%: {result1:N0} VNĐ");

        Console.WriteLine(
            $"Giảm 15%: {result2:N0} VNĐ");

        Console.WriteLine(
            $"Voucher 100.000 VNĐ: {result3:N0} VNĐ");


        // ======================================
        // METHOD OVERRIDING
        // ======================================

        Console.WriteLine();
        Console.WriteLine("===== METHOD OVERRIDING =====");


        // Danh sách có kiểu lớp cha
        List<DeliveryService> deliveries =
            new List<DeliveryService>();


        // Thêm đối tượng ExpressDelivery
        deliveries.Add(
            new ExpressDelivery("DH001", 8));


        // Thêm đối tượng EcoDelivery
        deliveries.Add(
            new EcoDelivery("DH002", 8));


        // Thêm ExpressDelivery với khoảng cách > 10km
        deliveries.Add(
            new ExpressDelivery("DH003", 15));


        // Thêm EcoDelivery với khoảng cách > 10km
        deliveries.Add(
            new EcoDelivery("DH004", 15));


        // ======================================
        // RUNTIME POLYMORPHISM
        // ======================================

        foreach (DeliveryService delivery in deliveries)
        {
            Console.WriteLine(
                $"Mã đơn: {delivery.OrderId}");

            Console.WriteLine(
                $"Khoảng cách: {delivery.DistanceKm} km");

            Console.WriteLine(
                $"Phí vận chuyển: " +
                $"{delivery.CalculateShippingFee():N0} VNĐ");

            Console.WriteLine();
        }
    }
}