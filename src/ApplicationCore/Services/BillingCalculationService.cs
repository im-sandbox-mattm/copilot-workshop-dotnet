namespace Microsoft.eShopWeb.ApplicationCore.Services;

/// <summary>
/// Monthly billing calculation — legacy code migrated from VB6.
/// Cyclomatic complexity: 22 (target: reduce to &lt;10 per method)
/// Technical debt ticket: GS-891
/// </summary>
public class BillingCalculationService
{
    // God method — cyclomatic complexity 22, 85 lines
    public decimal CalculateMonthlyBill(
        string accountId,
        string rateCode,
        decimal usageTherm,
        bool isPeakSeason,
        int loyaltyYears,
        string? promoCode)
    {
        // Magic numbers everywhere
        if (usageTherm < 0) throw new ArgumentException("Usage cannot be negative");
        if (string.IsNullOrEmpty(accountId)) throw new ArgumentException("Account required");

        decimal baseRate;
        decimal deliveryCharge;
        decimal seasonalMultiplier = 1.0m;

        // Complex conditional — rate determination
        if (rateCode == "RES01")
        {
            baseRate = 0.65m;
            deliveryCharge = 12.50m;
        }
        else if (rateCode == "RES02")
        {
            baseRate = 0.58m;
            deliveryCharge = 14.00m;
        }
        else if (rateCode == "COM01")
        {
            baseRate = 0.52m;
            deliveryCharge = 25.00m;
        }
        else if (rateCode == "COM02")
        {
            baseRate = 0.48m;
            deliveryCharge = 30.00m;
        }
        else if (rateCode == "IND01")
        {
            baseRate = 0.42m;
            deliveryCharge = 50.00m;
        }
        else
        {
            baseRate = 0.70m; // Default — highest rate
            deliveryCharge = 15.00m;
        }

        // Seasonal adjustment
        if (isPeakSeason)
        {
            if (rateCode.StartsWith("RES"))
                seasonalMultiplier = 1.15m;
            else if (rateCode.StartsWith("COM"))
                seasonalMultiplier = 1.10m;
            else
                seasonalMultiplier = 1.08m;
        }

        // Usage tier calculation — nested conditionals
        decimal usageCharge;
        if (usageTherm <= 50)
        {
            usageCharge = usageTherm * baseRate;
        }
        else if (usageTherm <= 150)
        {
            usageCharge = (50 * baseRate) + ((usageTherm - 50) * baseRate * 0.90m);
        }
        else if (usageTherm <= 500)
        {
            usageCharge = (50 * baseRate) + (100 * baseRate * 0.90m) + ((usageTherm - 150) * baseRate * 0.80m);
        }
        else
        {
            usageCharge = (50 * baseRate) + (100 * baseRate * 0.90m) + (350 * baseRate * 0.80m) + ((usageTherm - 500) * baseRate * 0.70m);
        }

        // Loyalty discount
        decimal loyaltyDiscount = 0;
        if (loyaltyYears >= 10)
            loyaltyDiscount = 0.05m;
        else if (loyaltyYears >= 5)
            loyaltyDiscount = 0.03m;
        else if (loyaltyYears >= 2)
            loyaltyDiscount = 0.01m;

        // Promo code processing — more magic strings
        decimal promoDiscount = 0;
        if (!string.IsNullOrEmpty(promoCode))
        {
            if (promoCode == "SUMMER2026")
                promoDiscount = 0.10m;
            else if (promoCode == "LOYALTY50")
                promoDiscount = 0.50m; // 50% off — likely a bug, too generous
            else if (promoCode == "NEWCUST")
                promoDiscount = 0.15m;
            else if (promoCode == "EMPLOYEE")
                promoDiscount = 0.25m;
            // Unknown promo codes are silently ignored — no validation
        }

        // Final calculation
        var subtotal = (usageCharge * seasonalMultiplier) + deliveryCharge;
        var afterLoyalty = subtotal * (1 - loyaltyDiscount);
        var afterPromo = afterLoyalty * (1 - promoDiscount);

        // Minimum bill enforcement
        if (afterPromo < deliveryCharge)
            afterPromo = deliveryCharge;

        return Math.Round(afterPromo, 2, MidpointRounding.ToEven);
    }
}
