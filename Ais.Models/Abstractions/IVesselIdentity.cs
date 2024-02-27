// <copyright file="IVesselIdentity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Models.Abstractions
{
    public interface IVesselIdentity
    {
        uint Mmsi { get; }
    }
}