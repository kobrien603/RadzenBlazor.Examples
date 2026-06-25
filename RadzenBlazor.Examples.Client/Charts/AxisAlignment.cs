namespace RadzenBlazor.Examples.Client.Charts;

/// <summary>
/// Pure, testable math behind the dual-axis tick-alignment tester.
///
/// The core idea: two value axes line up tick-for-tick when they produce the
/// same number of intervals — i.e. <c>(Max - Min) / Step</c> is a whole number
/// on both axes and the two whole numbers are equal. The actual Min/Max/Step
/// values are irrelevant; only the interval count matters.
/// </summary>
public static class AxisAlignment
{
    /// <summary>Tolerance for treating a near-integer interval count as whole.</summary>
    public const double Tolerance = 1e-9;

    /// <summary>Safety cap so a tiny step over a huge range can't produce a runaway tick list.</summary>
    public const int MaxTicks = 200;

    /// <summary>An axis can only be evaluated when its range is positive and its step is positive.</summary>
    public static bool IsValid(double min, double max, double step)
        => step > 0 && max > min && !double.IsNaN(min) && !double.IsNaN(max) && !double.IsNaN(step);

    /// <summary>Number of intervals an axis produces: <c>(max - min) / step</c>. May be fractional.</summary>
    public static double IntervalCount(double min, double max, double step)
        => (max - min) / step;

    /// <summary>True when <paramref name="value"/> is a whole number within <see cref="Tolerance"/>.</summary>
    public static bool IsWhole(double value)
        => !double.IsNaN(value) && !double.IsInfinity(value)
           && Math.Abs(value - Math.Round(value)) < Tolerance;

    /// <summary>
    /// The tick values an axis renders: Min, Min+Step, …, up to (and including) Max.
    /// Returns an empty list for invalid input and is capped at <see cref="MaxTicks"/>
    /// entries so pathological input can never hang the UI.
    /// </summary>
    public static IReadOnlyList<double> TickValues(double min, double max, double step)
    {
        var ticks = new List<double>();
        if (!IsValid(min, max, step))
            return ticks;

        var count = IntervalCount(min, max, step);
        if (double.IsNaN(count) || double.IsInfinity(count))
            return ticks;

        // Number of whole steps that fit inside the range.
        var steps = (int)Math.Min(MaxTicks, Math.Floor(count + Tolerance));
        for (var i = 0; i <= steps; i++)
            ticks.Add(min + i * step);

        return ticks;
    }

    /// <summary>True when <see cref="TickValues"/> was truncated by <see cref="MaxTicks"/>.</summary>
    public static bool IsTruncated(double min, double max, double step)
        => IsValid(min, max, step) && Math.Floor(IntervalCount(min, max, step) + Tolerance) > MaxTicks;

    /// <summary>
    /// Evaluates whether the two axes' ticks line up and explains why / why not.
    /// </summary>
    public static AlignmentResult IsAligned(
        double lMin, double lMax, double lStep,
        double rMin, double rMax, double rStep)
    {
        if (!IsValid(lMin, lMax, lStep))
            return new AlignmentResult(false, InvalidReason("Left", lMin, lMax, lStep));
        if (!IsValid(rMin, rMax, rStep))
            return new AlignmentResult(false, InvalidReason("Right", rMin, rMax, rStep));

        var countL = IntervalCount(lMin, lMax, lStep);
        var countR = IntervalCount(rMin, rMax, rStep);

        if (!IsWhole(countL))
            return new AlignmentResult(false, "Left step doesn't divide its range evenly");
        if (!IsWhole(countR))
            return new AlignmentResult(false, "Right step doesn't divide its range evenly");

        var nL = (int)Math.Round(countL);
        var nR = (int)Math.Round(countR);

        if (nL != nR)
            return new AlignmentResult(false, $"Left has {nL} intervals, right has {nR} — counts must match");

        return new AlignmentResult(true, $"Both axes have {nL} intervals — ticks line up");
    }

    private static string InvalidReason(string axis, double min, double max, double step)
    {
        if (step <= 0)
            return $"{axis} step must be greater than 0";
        return $"{axis} max must be greater than min";
    }
}

/// <summary>Result of an alignment evaluation: whether the ticks line up, and a human-readable reason.</summary>
public readonly record struct AlignmentResult(bool Aligned, string Reason);
