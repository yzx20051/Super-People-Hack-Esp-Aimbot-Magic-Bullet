// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BreatheFreshPageMapper.cs" company="Magic Bullet Ltd">
//     Copyright (c) Magic Bullet Ltd. All rights reserved.  
// </copyright>
// <summary>
//   The breathe fresh page mapper.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace MagicBullet.Sample.Forms.Infrastructure
{
    using System;

    using FreshMvvm;

    /// <summary>
    /// The breathe fresh page mapper.
    /// </summary>
    public class BreatheFreshPageMapper : IFreshPageModelMapper
    {
        /// <summary>
        /// The get page type name.
        /// </summary>
        /// <param name="pageModelType">
        /// The page model type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName
                .Replace("ViewModels", "Views")
                .Replace("ViewModel", string.Empty);
        }
    }
}