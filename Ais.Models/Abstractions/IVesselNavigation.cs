// <copyright file="IVesselNavigation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Models.Abstractions
{
    public interface IVesselNavigation
    {
        float? CourseOverGroundDegrees { get; }

        Position? Position { get; }

        bool PositionAccuracy { get; }

        float? SpeedOverGround { get; }

        uint TimeStampSecond { get; }

        uint TrueHeadingDegrees { get; }
    }
}