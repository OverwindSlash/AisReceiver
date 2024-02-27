// <copyright file="IAisMultipartMessage.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Models.Abstractions
{
    public interface IAisMultipartMessage
    {
        uint PartNumber { get; }
    }
}