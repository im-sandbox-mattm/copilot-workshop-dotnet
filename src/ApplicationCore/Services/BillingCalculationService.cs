using System;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

// WORKSHOP ARTIFACT — used in Labs 7, 10, 12 and Day 3 demos. Not production code.
/// <summary>
/// Order total calculation — legacy pricing engine ported from the v1 storefront.
/// Cyclomatic complexity: 22 (target: reduce to &lt;10 per method)
/// Technical debt ticket: ESW-2847
/// </summary>
public class BillingCalculationService
{
    // God method — cyclomatic complexity 22, 85 lines
    public decimal CalculateOrderTotal(
        string customerId,
        string membershipTier,
        decimal catalogSubtotal,
        bool isPeakSeason,
        int loyaltyYears,
        string? promoCode)
    {
        // Magic numbers everywhere
        if (catalogSubtotal < 0) throw new ArgumentException("Order total cannot be negative");
        if (string.IsNullOrEmpty(customerId)) throw new ArgumentException("Customer ID required");

        decimal memberDiscount;
        decimal shippingBase;
        decimal seasonalMultiplier = 1.0m;

        // Complex conditional — membership tier determination
        if (membershipTier == "BASIC")
        {
            memberDiscount = 0.00m;
            shippingBase = 8.99m;
        }
        else if (membershipTier == "STANDARD")
        {
            memberDiscount = 0.05m;
            shippingBase = 6.99m;
        }
        else if (membershipTier == "PREMIUM")
        {
            memberDiscount = 0.10m;
            shippingBase = 4.99m;
        }
        else if (membershipTier == "ENTERPRISE")
        {
            memberDiscount = 0.15m;
            shippingBase = 2.99m;
        }
        else if (membershipTier == "EMPLOYEE")
        {
            memberDiscount = 0.20m;
            shippingBase = 0.00m;
        }
        else
        {
            memberDiscount = 0.00m; // Default — no discount
            shippingBase = 9.99m;
        }

        // Seasonal adjustment — peak season (Black Friday through Dec 31)
        if (isPeakSeason)
        {
            if (membershipTier == "PREMIUM" || membershipTier == "ENTERPRISE")
                seasonalMultiplier = 0.95m; // Loyalty reward during peak
            else if (membershipTier == "STANDARD")
                seasonalMultiplier = 1.00m; // No change
            else
                seasonalMultiplier = 1.05m; // Surge for non-members
        }

        // Volume tier calculation — nested conditionals
        decimal volumeDiscount;
        if (catalogSubtotal <= 50)
        {
            volumeDiscount = 0m;
        }
        else if (catalogSubtotal <= 150)
        {
            volumeDiscount = (catalogSubtotal - 50) * 0.02m;
        }
        else if (catalogSubtotal <= 500)
        {
            volumeDiscount = (100 * 0.02m) + ((catalogSubtotal - 150) * 0.05m);
        }
        else
        {
            volumeDiscount = (100 * 0.02m) + (350 * 0.05m) + ((catalogSubtotal - 500) * 0.08m);
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
        var afterMemberDiscount = catalogSubtotal * (1 - memberDiscount);
        var afterVolumeDiscount = afterMemberDiscount - volumeDiscount;
        var subtotal = (afterVolumeDiscount * seasonalMultiplier) + shippingBase;
        var afterLoyalty = subtotal * (1 - loyaltyDiscount);
        var afterPromo = afterLoyalty * (1 - promoDiscount);

        // Minimum order charge
        if (afterPromo < shippingBase)
            afterPromo = shippingBase;

        return Math.Round(afterPromo, 2, MidpointRounding.ToEven);
    }
}
