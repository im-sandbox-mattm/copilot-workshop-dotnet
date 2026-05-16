// LAB 7 REFERENCE ANSWER — revealed at the 20-minute mark.
// To run: copy this file to tests/UnitTests/ApplicationCore/Services/
// then: dotnet test tests/UnitTests/

using Microsoft.eShopWeb.ApplicationCore.Services;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services;

public class BillingCalculationServiceTests_Reference
{
    private readonly BillingCalculationService _sut = new();

    // -------------------------------------------------------------------------
    // Guard clauses
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_NegativeCatalogSubtotal_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            _sut.CalculateOrderTotal("C001", "BASIC", -1m, false, 0, null));
        Assert.Contains("negative", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CalculateOrderTotal_EmptyCustomerId_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            _sut.CalculateOrderTotal("", "BASIC", 50m, false, 0, null));
        Assert.Contains("Customer ID", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CalculateOrderTotal_NullCustomerId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _sut.CalculateOrderTotal(null!, "BASIC", 50m, false, 0, null));
    }

    // -------------------------------------------------------------------------
    // Membership tiers — discount rate and shipping base
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("BASIC",       30, 38.99)]
    [InlineData("STANDARD",    30, 35.49)]
    [InlineData("PREMIUM",     30, 31.99)]
    [InlineData("ENTERPRISE",  30, 28.49)]
    [InlineData("EMPLOYEE",    30, 24.00)]
    public void CalculateOrderTotal_MembershipTiers_ApplyCorrectDiscountAndShipping(
        string tier, decimal subtotal, decimal expected)
    {
        // catalogSubtotal ≤ 50 → no volume discount; non-peak; no loyalty; no promo
        var result = _sut.CalculateOrderTotal("C001", tier, subtotal, false, 0, null);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateOrderTotal_UnknownMembershipTier_UsesDefaultHighestShipping()
    {
        // Unknown tier: memberDiscount=0.00, shippingBase=9.99 → 30 + 9.99 = 39.99
        var result = _sut.CalculateOrderTotal("C001", "MYSTERY", 30m, false, 0, null);
        Assert.Equal(39.99m, result);
    }

    // -------------------------------------------------------------------------
    // Volume discount tiers
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_SubtotalAtTier1Boundary_NoVolumeDiscount()
    {
        // catalogSubtotal = 50 (exactly at boundary, ≤50) → volumeDiscount = 0
        // BASIC: 50.00 + 8.99 = 58.99
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, null);
        Assert.Equal(58.99m, result);
    }

    [Fact]
    public void CalculateOrderTotal_SubtotalInTier2_AppliesTwoPercentVolumeDiscount()
    {
        // catalogSubtotal = 100 → volumeDiscount = (100-50)*0.02 = 1.00
        // BASIC: 100.00 - 1.00 + 8.99 = 107.99
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 100m, false, 0, null);
        Assert.Equal(107.99m, result);
    }

    [Fact]
    public void CalculateOrderTotal_SubtotalInTier3_AppliesFivePercentVolumeDiscount()
    {
        // catalogSubtotal = 200 → volumeDiscount = (100*0.02) + (50*0.05) = 4.50
        // BASIC: 200.00 - 4.50 + 8.99 = 204.49
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 200m, false, 0, null);
        Assert.Equal(204.49m, result);
    }

    [Fact]
    public void CalculateOrderTotal_SubtotalAboveTier4_AppliesEightPercentVolumeDiscount()
    {
        // catalogSubtotal = 600 → volumeDiscount = (100*0.02) + (350*0.05) + (100*0.08) = 27.50
        // BASIC: 600.00 - 27.50 + 8.99 = 581.49
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 600m, false, 0, null);
        Assert.Equal(581.49m, result);
    }

    // -------------------------------------------------------------------------
    // Seasonal multipliers
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_PeakSeason_UnknownTier_AppliesSurgeMultiplier()
    {
        // BASIC (else branch) peak: seasonalMultiplier = 1.05
        // $100: afterVolume = 99.00 → subtotal = (99*1.05) + 8.99 = 112.94
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 100m, true, 0, null);
        Assert.Equal(112.94m, result);
    }

    [Fact]
    public void CalculateOrderTotal_PeakSeason_StandardTier_NoSeasonalChange()
    {
        // STANDARD peak: seasonalMultiplier = 1.00 (no change)
        // $100: afterMember=95.00, afterVolume=94.00 → subtotal = (94*1.00) + 6.99 = 100.99
        var result = _sut.CalculateOrderTotal("C001", "STANDARD", 100m, true, 0, null);
        Assert.Equal(100.99m, result);
    }

    [Fact]
    public void CalculateOrderTotal_PeakSeason_PremiumTier_AppliesLoyaltyRewardMultiplier()
    {
        // PREMIUM peak: seasonalMultiplier = 0.95
        // $100: afterMember=90.00, afterVolume=89.00 → subtotal = (89*0.95) + 4.99 = 89.54
        var result = _sut.CalculateOrderTotal("C001", "PREMIUM", 100m, true, 0, null);
        Assert.Equal(89.54m, result);
    }

    [Fact]
    public void CalculateOrderTotal_PeakSeason_EnterpriseTier_AppliesLoyaltyRewardMultiplier()
    {
        // ENTERPRISE peak: seasonalMultiplier = 0.95
        // $100: afterMember=85.00, afterVolume=84.00 → subtotal = (84*0.95) + 2.99 = 82.79
        var result = _sut.CalculateOrderTotal("C001", "ENTERPRISE", 100m, true, 0, null);
        Assert.Equal(82.79m, result);
    }

    [Fact]
    public void CalculateOrderTotal_NonPeakSeason_NoSeasonalAdjustment()
    {
        var peak    = _sut.CalculateOrderTotal("C001", "BASIC",    50m, true,  0, null);
        var nonPeak = _sut.CalculateOrderTotal("C001", "STANDARD", 50m, false, 0, null);
        // Confirm that non-peak STANDARD == peak STANDARD (both seasonalMultiplier=1.0)
        var peakStandard    = _sut.CalculateOrderTotal("C001", "STANDARD", 50m, true,  0, null);
        var nonPeakStandard = _sut.CalculateOrderTotal("C001", "STANDARD", 50m, false, 0, null);
        Assert.Equal(nonPeakStandard, peakStandard);
    }

    // -------------------------------------------------------------------------
    // Loyalty discounts
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(0,  58.99)] // no discount below 2 years
    [InlineData(1,  58.99)] // still no discount at 1 year — easy to miss!
    [InlineData(2,  58.40)] // 1% discount kicks in at exactly 2 years
    [InlineData(5,  57.22)] // 3% at 5 years
    [InlineData(10, 56.04)] // 5% at 10 years
    [InlineData(15, 56.04)] // capped at 5% — no higher tier above 10
    public void CalculateOrderTotal_LoyaltyYears_ApplyCorrectDiscount(
        int loyaltyYears, decimal expected)
    {
        // BASIC, catalogSubtotal=50 (no volume discount), non-peak, no promo
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, loyaltyYears, null);
        Assert.Equal(expected, result);
    }

    // -------------------------------------------------------------------------
    // Promo codes
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_Summer2026PromoCode_AppliesTenPercentDiscount()
    {
        // BASIC $50, subtotal=58.99 → 58.99 * 0.90 = 53.09
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, "SUMMER2026");
        Assert.Equal(53.09m, result);
    }

    [Fact]
    public void CalculateOrderTotal_NewCustPromoCode_AppliesFifteenPercentDiscount()
    {
        // BASIC $50, subtotal=58.99 → 58.99 * 0.85 = 50.14
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, "NEWCUST");
        Assert.Equal(50.14m, result);
    }

    [Fact]
    public void CalculateOrderTotal_EmployeePromoCode_AppliesTwentyFivePercentDiscount()
    {
        // BASIC $50, subtotal=58.99 → 58.99 * 0.75 = 44.24
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, "EMPLOYEE");
        Assert.Equal(44.24m, result);
    }

    [Fact]
    public void CalculateOrderTotal_UnknownPromoCode_SilentlyIgnoredNoException()
    {
        // Unknown promo codes are silently ignored — no exception, no discount applied
        var withPromo    = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, "INVALID99");
        var withoutPromo = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, null);
        Assert.Equal(withoutPromo, withPromo);
    }

    // ⚠️  DEBRIEF DISCUSSION — Is LOYALTY50 intentional or a bug?
    //
    //  LOYALTY50 gives 50% off. The next highest promo is EMPLOYEE at 25%.
    //  There is no validation, no comment explaining the intent, and no test
    //  in the original codebase that would catch if this value were changed.
    //  Ask the room: "Did Copilot flag this when it generated your tests?
    //  If not — what would a good test look like to surface this as suspicious?"
    [Fact]
    public void CalculateOrderTotal_Loyalty50PromoCode_AppliesFiftyPercentDiscount()
    {
        // BASIC $50, subtotal=58.99 → 58.99 * 0.50 = 29.50 (banker's rounding)
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 50m, false, 0, "LOYALTY50");
        Assert.Equal(29.50m, result);
    }

    // -------------------------------------------------------------------------
    // Minimum order charge enforcement
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_AfterPromoBelowShippingBase_EnforcesMinimumCharge()
    {
        // BASIC ($8.99 shipping), $0 order, LOYALTY50 promo:
        // subtotal=8.99, after 50% promo=4.495 → below shippingBase → enforced to 8.99
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 0m, false, 0, "LOYALTY50");
        Assert.Equal(8.99m, result);
    }

    [Fact]
    public void CalculateOrderTotal_ZeroCatalogSubtotal_ReturnsShippingBaseOnly()
    {
        // No promo: afterPromo = shippingBase, minimum check uses < (not <=), so no enforcement
        var result = _sut.CalculateOrderTotal("C001", "BASIC", 0m, false, 0, null);
        Assert.Equal(8.99m, result);
    }

    [Fact]
    public void CalculateOrderTotal_EmployeeTier_ZeroShippingBase_MinimumIsZero()
    {
        // EMPLOYEE shippingBase=0.00 — minimum charge is effectively zero
        var result = _sut.CalculateOrderTotal("C001", "EMPLOYEE", 0m, false, 0, null);
        Assert.Equal(0.00m, result);
    }

    // -------------------------------------------------------------------------
    // Combination / integration
    // -------------------------------------------------------------------------

    [Fact]
    public void CalculateOrderTotal_AllDiscountsStacked_CalculatesCorrectly()
    {
        // PREMIUM + peak (0.95) + 10 loyalty years (5%) + NEWCUST (15%) + $200 order
        // afterMember = 200*0.90 = 180.00
        // volumeDiscount = (100*0.02)+(50*0.05) = 4.50 → afterVolume = 175.50
        // subtotal = (175.50*0.95) + 4.99 = 171.715
        // afterLoyalty = 171.715*0.95 = 163.12925
        // afterPromo = 163.12925*0.85 = 138.6598625 → 138.66
        var result = _sut.CalculateOrderTotal("C001", "PREMIUM", 200m, true, 10, "NEWCUST");
        Assert.Equal(138.66m, result);
    }
}
